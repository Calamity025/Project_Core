import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileCreationComponent } from './profile-creation/profile-creation.component';


const routes: Routes = [
    {
        path : 'home',
        component : HomeComponent
    },
    {
      path : 'register/profile',
      component : ProfileCreationComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }