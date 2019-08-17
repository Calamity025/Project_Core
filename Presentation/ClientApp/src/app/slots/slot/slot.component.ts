import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { SearchService } from 'src/app/services/search.service';
import { Slot } from 'src/app/models/slot';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from 'src/app/services';

@Component({
  selector: 'app-slot',
  templateUrl: './slot.component.html',
  styleUrls: ['./slot.component.css']
})
export class SlotComponent implements OnInit {
  slotId? : string;
  slot? : Slot;
  timeDifference? : number;
  days? : number;
  hours? : number;
  minutes? : number;
  timeinterval?;
  isMakeBetClicked : boolean = false;
  bet? : number;
  isNotValid? : boolean = false;
  userBet? : number;
  isAuthorized? : boolean;
  isSeller? : boolean;
  isFollowing? = false;

  constructor(private route: ActivatedRoute,
    private router : Router,
    private searchService : SearchService,
    private httpClient : HttpClient,
    private authService : AuthService) { }

  ngOnInit() {
    this.slotId = this.route.snapshot.paramMap.get('id');
    this.searchService.getSlot(this.slotId).subscribe(val => {
      this.slot = val;
      this.updateTimer();
      this.timeinterval = setInterval(() => {
      let t = this.getRemainingTime();
      if(this.timeDifference <= 0){
        clearInterval(this.timeinterval);
      }}, 1000);
      this.authService.currentUser$.subscribe(val => {
        if(val){
          (val.Id == this.slot.user.id) ? this.isSeller = true : this.isSeller = false;
          val.FollowingSlots.find(i => i == this.slotId) ? this.isFollowing = true : this.isFollowing = false;
        }
      });
    });
    
    this.authService.isAuthorized$.subscribe(val => {
      val? this.updateUserBet() : this.userBet = null; 
      this.isAuthorized = val;
    });
  }

  updateUserBet(){
    this.searchService.getUserBet(this.slotId).subscribe(val => this.userBet = val);
  }

  onBackClick(){
    this.router.navigate(['/']);
  }

  getRemainingTime(){
    let t = Date.parse(this.slot.endTime) - Date.now();
    this.minutes = Math.floor((t / 1000 / 60) % 60);
    this.hours = Math.floor((t / (1000 * 60 * 60)) % 24);
    this.days = Math.floor((t / (1000 * 60 * 60 * 24)));
    this.timeDifference = t;
  }

  updateTimer(){
    let t = this.getRemainingTime();
    if(this.timeDifference <= 0){
      clearInterval(this.timeinterval);
    }
  }

  onBetClick(){
    this.isMakeBetClicked = true;
  }

  onMakeBetClick(){
    this.httpClient.put<any>('https://localhost:44324/api/Slot/makeBet/' + this.slotId, `"${this.bet}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
    .subscribe(() => {
      this.isMakeBetClicked = false;
      this.searchService.updatePrice(this.slotId).subscribe(val => this.slot.price = val);
    });
  }

  onBetInput(value : number){
    if (value < this.slot.price + this.slot.step){
      this.isNotValid = true;
    }
    else{
      this.bet = value;
      this.isNotValid = false;
    }
  }

  onRefreshClick(){
    this.searchService.updatePrice(this.slotId).subscribe(val => this.slot.price = val);
  }

  onDeleteClick() {
    this.httpClient.delete<any>('https://localhost:44324/api/Slot/' + this.slotId)
      .subscribe(() => this.router.navigate(['/']));
  }

  onFollowClick(){
    if(this.isFollowing){
      this.httpClient.put<any>('https://localhost:44324/api/Profile/unfollow/' + this.slotId, null)
        .subscribe(() => this.isFollowing = false);
    }
    else{
      this.httpClient.put<any>('https://localhost:44324/api/Profile/follow/' + this.slotId, null)
        .subscribe(() => this.isFollowing = true);
    }
  }

  onEditClick(){
    this.router.navigate(['/edit/' + this.slotId])
  }
}
