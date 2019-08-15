import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuthService, ReqResService } from './services';
import { Router } from '@angular/router';
import { User, LoginModel, UserRegistrationModel } from '../models';
import { catchError } from 'rxjs/operators';
import { Globals } from '../Globals.component';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css']
})
export class AuthorizationComponent implements OnInit {
  isLoading = false;
  currentUser : User;
  password? : string;
  login? : string;
  email? : string;

  constructor(private authService: AuthService, 
    private globals : Globals,
    private cdRef: ChangeDetectorRef,
    private router : Router) {
      this.setUser()
   }

  ngOnInit() {
  }

  onLoginInput(value: string) {
    this.login = value;
  }

  onPasswordInput(value: string) {
    this.password = value;
  }

  onLoginClick() {
    this.isLoading = true;

    const user : LoginModel = {
      login: this.login,
      password: this.password
    };

    this.authService.signIn(user).subscribe(val => {
      this.setUser();
    });
  }

  logOut() {
    this.authService.signOut().subscribe(() => this.globals.isAuthorized = false);
    
  }

  setUser() {
    this.authService.getCurrentUser().subscribe(
      (val : User) => {
        if(!!val){
          this.globals.isAuthorized = val.isAuthorized;
          this.currentUser = val;
          this.cdRef.detectChanges();
        }
      }
    )
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
      this.authService.signIn(loginInfo)
      .subscribe(() => {
        this.authService.getCurrentUser()
          .subscribe(val => {
            this.router.navigate(["/register/profile"]); 
            this.globals.isAuthorized = val.isAuthorized;
            this.currentUser = val;});
        })
    }},
    error => console.log(error))
  }
}
