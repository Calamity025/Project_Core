import { Injectable } from '@angular/core';
import { BehaviorSubject, of, throwError, Observable } from 'rxjs';
import { delay, catchError, tap, map } from 'rxjs/operators';
import { User } from '../models/user';
import { LoginModel } from '../models/login-model';
import { JwtService } from '../services/jwt.service';
import { HttpClient } from '@angular/common/http';
import { UserLoginResponse } from '../models/userLoginResponse';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import { Router } from '@angular/router';

@Injectable()
export class AuthService {
  private currentUser = null;

  constructor(private httpClient: HttpClient,
    private jwtService: JwtService) { }

    public isSignedIn(): boolean {
      if(!!this.currentUser){
        return this.currentUser.isAuthorized;
      }
      let isAuthorized : boolean;
      this.getCurrentUser().subscribe(val => {
        isAuthorized = val.IsAuthorized;
        return isAuthorized;
      },
      catchError => (error => false));
    }
  
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
  
    public getCurrentUser(): Observable<User> {
      if(!!this.currentUser){
        console.log(this.currentUser);
        return new Observable(this.currentUser);
      }

      return this.httpClient.get<User>('https://localhost:44324/Account/Current')
      .pipe(tap(x => {
        this.currentUser = x;
      }));
    }
}
