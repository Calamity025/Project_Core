import { Injectable } from '@angular/core';
import { BehaviorSubject, of, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user';
import { LoginModel } from '../models/login-model';
import { JwtService } from './jwt.service';
import { HttpClient } from '@angular/common/http';
import { UserLoginResponse } from '../models/userLoginResponse';
import { UserRegistrationModel } from '../models';

@Injectable()
export class AuthService {
  isAuthorized$ = new BehaviorSubject<boolean>(false);
  currentUser$ = new BehaviorSubject<User>(null);

  constructor(private httpClient: HttpClient,
    private jwtService: JwtService) { 
    }
  
    public signIn(loginModel: LoginModel) {
      const PATH = 'https://localhost:44324/Token';
      this.httpClient.post<UserLoginResponse>(PATH, loginModel)
      .subscribe(val => {
          this.jwtService.persistToken(val.access_token);
          this.getCurrentUser();
        },
        catchError(error => {
          alert(error);
          return of(null);
        })
    );
    }
  
    public signOut() {
      this.isAuthorized$.next(null);
      this.currentUser$.next(null);
      this.jwtService.clearToken();
    }

    public signUp(userRegistration : UserRegistrationModel) : Observable<User>{
      const PATH = 'https://localhost:44324/Register';
      return this.httpClient.post<User>(PATH, userRegistration, {observe : 'response'})
      .pipe(catchError(error => {
        alert(error);
        return of(null);
      }))
    }
  
    public getCurrentUser() {    
      if(!this.jwtService.isExpired()){
        this.httpClient.get<User>('https://localhost:44324/Account/Current')
        .subscribe(x => {
          this.currentUser$.next(x);
          this.isAuthorized$.next(true);
        });
      }
    }
}
