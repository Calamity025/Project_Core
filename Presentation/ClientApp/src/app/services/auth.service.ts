import { Injectable } from '@angular/core';
import { BehaviorSubject, of, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user';
import { LoginModel } from '../models/login-model';
import { JwtService } from './jwt.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLoginResponse } from '../models/userLoginResponse';
import { UserRegistrationModel } from '../models';

@Injectable()
export class AuthService {
  isAuthorized$ = new BehaviorSubject<boolean>(false);
  currentUser$ = new BehaviorSubject<User>(null);
  private currentUser : User;

  constructor(private httpClient: HttpClient,
    private jwtService: JwtService) { 
      this.currentUser$.subscribe(val => this.currentUser = val);
    }
  
    public signIn(loginModel: LoginModel) {
      const PATH = 'https://localhost:44324/Token';
      this.httpClient.post<UserLoginResponse>(PATH, loginModel)
      .subscribe(val => {
          this.jwtService.persistToken(val.access_token);
          this.getCurrentUser();
        },
        err => err.status == 400 ? alert(err.error) : console.log(err.error)
    );
    }
  
    public signOut() {
      this.isAuthorized$.next(null);
      this.currentUser$.next(null);
      this.jwtService.clearToken();
    }

    public signUp(userRegistration : UserRegistrationModel) : Observable<User>{
      const PATH = 'https://localhost:44324/Register';
      return this.httpClient.post<User>(PATH, userRegistration);
    }
  
    public getCurrentUser() {    
      if(!this.jwtService.isExpired()){
        this.httpClient.get<User>('https://localhost:44324/Account/Current')
        .subscribe(x => {
          this.currentUser$.next(x);
          this.isAuthorized$.next(true);
        },
        err => err.status == 400 ? alert(err.error) : console.log(err.error));
      }
    }

    public addMoney(value: number){
      this.httpClient.put<string>('https://localhost:44324/Account/addMoney', `"${value}"`, 
      { headers: new HttpHeaders({'Content-Type': 'application/json'})})
      .subscribe(val => {
        this.currentUser.balance = parseFloat(val);
        this.currentUser$.next(this.currentUser);
      },
      err => err.status == 400 ? alert(err.error) : console.log(err.error))
    }
}
