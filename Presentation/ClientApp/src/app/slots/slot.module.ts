import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SlotCreationComponent } from './slot-creation/slot-creation.component';
import { SlotComponent } from './slot/slot.component';
import { SlotEditComponent } from './slot-edit/slot-edit.component';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../authorization/interceptor/token.interceptor';

const routes : Routes = [
  {
    path : 'create',
    component : SlotCreationComponent
  },
  {
    path : ':id',
    component : SlotComponent
  },
  {
    path : 'edit/:id',
    component : SlotEditComponent
  }
]

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    SlotCreationComponent,
    SlotComponent,
    SlotEditComponent,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})
export class SlotModule { }
