import { Component } from '@angular/core';
import { AuthService, ReqResService } from '../authorization/services';
import { LoginModel } from '../authorization/models';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isLoginShown = false;
  user: LoginModel = new LoginModel();

  constructor(private authService: AuthService, private reqResService: ReqResService) { }

  ngOnInit() {
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  toggleLoginForm() {
    this.isLoginShown = !this.isLoginShown;
    this.user.login = null;
    this.user.password = null;
  }

  logIn() {
    if(this.authService.signIn(this.user).subscribe(console.log) != null){
      this.toggleLoginForm();
    }
  }

  sendSomeRequests() {
    this.reqResService.getUser(1).subscribe(console.log);
  }
}
