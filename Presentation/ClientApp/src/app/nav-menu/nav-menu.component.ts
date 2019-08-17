import { Component, ChangeDetectorRef } from '@angular/core';
import { AuthService, ReqResService } from '../services';
import { LoginModel, User } from '../models';
import { map, tap, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  
  constructor() {
      
   }

  ngOnInit() {
    
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
