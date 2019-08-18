import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Profile } from '../models';
import { HttpClient } from '@angular/common/http';

const PATH = 'https://localhost:44324/api/Profile';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private httpClient : HttpClient) { }

  getProfile() : Observable<Profile> {
    return this.httpClient.get<Profile>(PATH);
  }

  unfollowSlot(id : number) : Observable<any>{
    return this.httpClient.put<any>(PATH + '/unfollow/' + id, null);
  }

  deleteUser() : Observable<any>{
    return this.httpClient.delete<any>('https://localhost:44324/Account');
  }
}
