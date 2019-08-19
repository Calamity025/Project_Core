import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services';
import { ProfileService } from '../../services/profile.service';
import { User, Profile } from 'src/app/models';
import { slotMinimum } from 'src/app/models/slotMinimum';
import { Router } from '@angular/router';
import { Tag } from 'src/app/models/tag';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  isAuthorized? = false;
  currentUser? : User;
  profile? : Profile;
  users? : User[] = [];

  constructor(private authService : AuthService,
    private profileService : ProfileService,
    private router : Router,
    private httpClient : HttpClient) { }

  ngOnInit() {
    this.authService.currentUser$.subscribe(val => this.currentUser = val);
    this.authService.isAuthorized$.subscribe(val => {
      this.isAuthorized = val; 
      if(!val) {
        this.profile = null;
        this.users = [];
      } 
      else {
        this.profileService.getProfile().subscribe(val => this.profile = val,
          err => alert(err.error));
      }});
  }

  onSlotClick(slot : slotMinimum){
    this.router.navigate(['slot/' + slot.id]);
  }

  onUnsubscribe(slot : slotMinimum){
    this.profileService.unfollowSlot(slot.id).subscribe(() => this.profileService.getProfile().subscribe(val => this.profile = val,
      err => alert(err.error)),
    err => alert(err.error));
  }

  onDeleteClick(){
    this.profileService.deleteUser().subscribe(val => {
      this.authService.signOut();
      this.router.navigate(['/']);
    },
    err => alert(err.error))
  }

  onGetUsers(){
    this.httpClient.get<User[]>('https://localhost:44324/Account')
      .subscribe(val => {this.users = val},
        err => alert(err.error));
  }

  onPromote(user : User){
    alert(user.name)
    this.httpClient.put<any>('https://localhost:44324/Account', `"${user.name}"`, 
    { headers: new HttpHeaders({'Content-Type': 'application/json'})})
      .subscribe(val => this.onGetUsers()
      ,err => alert(err.error));
  }
}
