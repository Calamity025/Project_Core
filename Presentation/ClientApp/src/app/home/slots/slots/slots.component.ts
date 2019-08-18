import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from '../../../services/search.service';
import { Observable, fromEvent } from 'rxjs';
import { map, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { slotMinimum } from 'src/app/models/slotMinimum';
import { AuthService } from 'src/app/services';

@Component({
  selector: 'app-slots',
  templateUrl: './slots.component.html',
  styleUrls: ['./slots.component.css']
})
export class SlotsComponent implements OnInit {
  isAuthorized? : boolean;
  slots : slotMinimum[] = [];

  constructor(private router : Router,
    private authService : AuthService,
    private searchService : SearchService) {
    }

  ngOnInit() {
    this.authService.isAuthorized$.subscribe(val => this.isAuthorized = val);
    this.searchService.selectedSlots.subscribe(val => this.slots = val);
  }

  ngAfterViewInit(): void {
    let input = document.getElementById('search');
    let observable = fromEvent(input, 'input');

    observable.pipe(
      map(event => (<HTMLInputElement>event.target).value),
      debounceTime(500),
      distinctUntilChanged()
    )
    .subscribe({next: value => this.searchService.getSlotsByName(value)});
  }

  onSlotClick(slot : slotMinimum){
    this.router.navigate(['slot/' + slot.id]);
  }

  next(){
    this.searchService.next();
  }

  previous(){
    this.searchService.previous();
  }
}
