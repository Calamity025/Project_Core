import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileCreationComponent } from './profile-creation/profile-creation.component';
import { SlotCreationComponent } from './slots/slot-creation/slot-creation.component';


const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {
      path : 'register/profile',
      component : ProfileCreationComponent
    },
    {
      path : 'home',
      component : HomeComponent
    },
    {
      path : 'create',
      component : SlotCreationComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }