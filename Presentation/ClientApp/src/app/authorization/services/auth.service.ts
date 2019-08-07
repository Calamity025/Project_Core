import { Injectable } from '@angular/core';
import { BehaviorSubject, of, throwError, Observable } from 'rxjs';
import { delay, catchError, tap, map } from 'rxjs/operators';
import { User } from '../models/user';
import { LoginModel } from '../models/login-model';
import { JwtService } from '../services/jwt.service';
import { HttpClient } from '@angular/common/http';
import { UserResponse } from '../models/userResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser$ = new BehaviorSubject<User>(null);
  constructor(private httpClient: HttpClient,
    private jwtService: JwtService) { }

    public isSignedIn(): Observable<boolean> {
      return this.currentUser$.pipe(
        map(currentUser => !!currentUser)
      );
    }
  
    public signIn(loginModel: LoginModel): Observable<User> {
      const PATH = 'https://localhost:44324/Token';
      return this.httpClient.post<UserResponse>(PATH, loginModel)
      .pipe(
        tap(({user, access_token}) => {
          this.jwtService.persistToken(access_token);
          this.currentUser$.next(user as User);
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
          this.currentUser$.next(null);
          this.jwtService.clearToken();
        })
      );
    }
  
    public getCurrentUser(): Observable<User> {
      return this.currentUser$.asObservable();
    }
}
