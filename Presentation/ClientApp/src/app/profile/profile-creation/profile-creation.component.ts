import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ProfileModel } from '../../models/ProfileModel';
import { AuthService } from '../../services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-creation',
  templateUrl: './profile-creation.component.html',
  styleUrls: ['./profile-creation.component.css']
})
export class ProfileCreationComponent implements OnInit {
  firstName? : string;
  lastName? : string;
  deliveryAddress? : string;
  previewLink = '';
  fileName = "Choose file";
  uploadForm: FormGroup;  
  isLoading = false;
  isAuthorized? : boolean;

  constructor(private formBuilder: FormBuilder, 
    private httpClient: HttpClient,
    private authService : AuthService,
    private router : Router) { }

  ngOnInit() {
    this.uploadForm = this.formBuilder.group({
      profile: ['']
    });
    this.authService.isAuthorized$.subscribe(val => {
      this.isAuthorized = val;
      if(!this.isAuthorized){
        this.router.navigate(['/']);
      }
    });
  }

  onFirstNameInput(value: string) {
    this.firstName = value;
  }

  onLastNameInput(value: string) {
    this.lastName = value;
  }

  onAddressInput(value: string) {
    this.deliveryAddress = value;
  }

  onSubmitClick() {
    this.isLoading = true;

    const user : ProfileModel = {
      FirstName : this.firstName,
      LastName : this.lastName,
      DeliveryAddress : this.deliveryAddress
    };

    this.httpClient.post<any>('https://localhost:44324/api/Profile/', user)
      .subscribe(() =>
        this.router.navigate(["/"]));

    if(this.uploadForm.get('profile').value){
      const formData = new FormData();
      formData.append('file', this.uploadForm.get('profile').value);
      this.httpClient.post<any>('https://localhost:44324/api/Profile/image', formData).subscribe(
        (res) => console.log(res),
        (err) => console.log(err)
      );
    }
  }

  onFileSelect(event) {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      this.uploadForm.get('profile').setValue(file);
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]); 
      reader.onload = (event:any) => { 
        this.previewLink = event.target.result;
      }
      this.fileName = file.name;
    }
  }
}
