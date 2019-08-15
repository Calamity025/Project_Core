import { Injectable } from '@angular/core';
import { BehaviorSubject, of, throwError, Observable } from 'rxjs';
import { delay, catchError, tap, map } from 'rxjs/operators';
import { User } from '../../models/user';
import { LoginModel } from '../../models/login-model';
import { JwtService } from '../services/jwt.service';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { UserLoginResponse } from '../../models/userLoginResponse';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import { Router } from '@angular/router';
import { UserRegistrationModel } from '../../models';

@Injectable()
export class AuthService {
  private currentUser? : User;
  isAuthorized : boolean;

  constructor(private httpClient: HttpClient,
    private jwtService: JwtService) { }
  
    public signIn(loginModel: LoginModel): Observable<User> {
      const PATH = 'https://localhost:44324/Token';
      return this.httpClient.post<UserLoginResponse>(PATH, loginModel)
      .pipe(
        tap(({access_token}) => {
          this.jwtService.persistToken(access_token);
        }),
        catchError(error => {
          alert(error);
          return of(null);
        })
    );
    }
  
    public signOut(): Observable<void> {
      return of(null).pipe(
        delay(1500),
        tap(() => {
          this.currentUser = null;
          this.jwtService.clearToken();
        })
      );
    }

    public signUp(userRegistration : UserRegistrationModel) : Observable<User>{
      const PATH = 'https://localhost:44324/Register';
      return this.httpClient.post<User>(PATH, userRegistration, {observe : 'response'})
      .pipe(catchError(error => {
        alert(error);
        return of(null);
      }))
    }
  
    public getCurrentUser(): Observable<User> {
      if(!this.jwtService.isExpired()){
        if(!!this.currentUser){
          console.log(this.currentUser);
          return of(this.currentUser);
        }
        
        return this.httpClient.get<User>('https://localhost:44324/Account/Current')
        .pipe(tap(x => this.currentUser = x));
      }
      return of(null);
    }
}
