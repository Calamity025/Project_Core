import { Component, OnInit } from '@angular/core';
import { SearchService } from '../services/search.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(private searchService : SearchService){ 
  }

  ngOnInit(){
    this.searchService.refresh();
  }
}
