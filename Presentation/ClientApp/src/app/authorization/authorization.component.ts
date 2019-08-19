import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { User, LoginModel, UserRegistrationModel } from '../models';
import { AuthService } from '../services';
declare var $:any;

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css']
})
export class AuthorizationComponent implements OnInit {
  isLoading = false;
  isAuthorized? : boolean;
  currentUser? : User;
  password? : string;
  login? : string;
  email? : string;
  summ? : number;

  constructor(private authService: AuthService, 
    private cdRef: ChangeDetectorRef,
    private router : Router) {
   }

  ngOnInit() {
    this.authService.isAuthorized$.subscribe(val => this.isAuthorized = val);
    this.authService.currentUser$.subscribe(val => this.currentUser = val);
    this.authService.getCurrentUser()
  }

  onLoginInput(value: string) {
    this.login = value;
  }

  onPasswordInput(value: string) {
    this.password = value;
  }

  onValueInput(value : number){
    this.summ = value;
  }

  onLoginClick() {
    this.isLoading = true;

    const user : LoginModel = {
      login: this.login,
      password: this.password
    };
    this.authService.signIn(user);
  }

  logOut() {
    this.authService.signOut();
    
  }

  onEmailInput(value: string) {
    this.email = value;
  }

  onSignUpClick() {
    this.isLoading = true;

    const user : UserRegistrationModel = {
      Login: this.login,
      Password: this.password,
      Email: this.email
    };

    this.authService.signUp(user).subscribe(response => {
      if(!!response){
      const loginInfo : LoginModel = {
        login: this.login,
        password: this.password
      };
      this.authService.signIn(loginInfo);
      this.router.navigate(["/register/profile"]); 
      this.authService.isAuthorized$.next(true);
      }
    },
    err => alert(err.error))
  }

  onAddMoneyClick(){
    this.authService.addMoney(this.summ);
    $('#dropdownBalance').dropdown('toggle');
  }
}
