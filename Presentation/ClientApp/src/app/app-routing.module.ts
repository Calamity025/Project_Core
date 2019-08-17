import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileCreationComponent } from './profile-creation/profile-creation.component';
import { SlotCreationComponent } from './slots/slot-creation/slot-creation.component';
import { SlotComponent } from './slots/slot/slot.component';
import { SlotEditComponent } from './slots/slot-edit/slot-edit.component';


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
    },
    {
      path : 'slot/:id',
      component : SlotComponent
    },
    {
      path : 'edit/:id',
      component : SlotEditComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }