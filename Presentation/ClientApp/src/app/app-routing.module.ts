import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileCreationComponent } from './profile/profile-creation/profile-creation.component';
import { ProfileComponent } from './profile/profile/profile.component';


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
      path : 'slot',
      loadChildren: () => import('./slots/slot.module').then(m => m.SlotModule)
    },
    {
      path : 'profile',
      component : ProfileComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }