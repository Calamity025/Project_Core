import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ReqResService } from '../services/req-res.service';
import { LoginModel } from '../models/login-model';

@Component({
  selector: 'app-interceptor',
  templateUrl: './interceptor.component.html',
  styleUrls: ['./interceptor.component.css']
})
export class InterceptorComponent implements OnInit {

  user: LoginModel = new LoginModel();

  constructor(private authService: AuthService, private reqResService: ReqResService) { }

  ngOnInit() {
  }


  logIn() {
    this.authService.signIn(this.user).subscribe(console.log);
  }

  sendSomeRequests() {
    this.reqResService.getUser(1).subscribe(console.log);
  }

}
