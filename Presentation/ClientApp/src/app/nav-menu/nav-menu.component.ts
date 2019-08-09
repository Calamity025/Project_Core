import { Component, ChangeDetectorRef } from '@angular/core';
import { AuthService, ReqResService } from '../authorization/services';
import { LoginModel, User } from '../authorization/models';
import { map, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isAuthorized : boolean;
  currentUser : User;
  userAvatar = "/UserAvatars/defaultUser.png";
  userName;
  user: LoginModel = new LoginModel();

  constructor(private authService: AuthService, 
    private reqResService: ReqResService,
    private cdRef: ChangeDetectorRef) {
      this.setUser()
   }

  ngOnInit() {
    
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  toggleLoginForm() {
    this.user.login = null;
    this.user.password = null;
  }

  logIn() {
    this.authService.signIn(this.user).subscribe(val => {
      this.setUser();
    });
  }

  logOut() {
    this.authService.signOut().subscribe(() => this.isAuthorized = false);
    
  }

  sendSomeRequests() {
    this.reqResService.getUser(1).subscribe(console.log);
  }

  setUser() {
    this.authService.getCurrentUser()
    .subscribe(
      (val : User) => {
        this.isAuthorized = true;
        this.currentUser = val;
        this.cdRef.detectChanges();
      }
    );
  }
}
