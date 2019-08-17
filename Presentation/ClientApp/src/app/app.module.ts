import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AuthService, JwtService, ReqResService } from './services'
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { TokenInterceptor } from './authorization/interceptor/token.interceptor';
import { ProfileCreationComponent } from './profile-creation/profile-creation.component';
import { AuthorizationComponent } from './authorization/authorization.component';
import { SlotCreationComponent } from './slots/slot-creation/slot-creation.component';
import { SearchComponent } from './home/search/search-property/search-property.component';
import { SlotsComponent } from './home/slots/slots/slots.component';
import { SearchService } from './services/search.service';
import { SlotComponent } from './slots/slot/slot.component';
import { SlotEditComponent } from './slots/slot-edit/slot-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchComponent,
    SlotsComponent,
    ProfileCreationComponent,
    AuthorizationComponent,
    SlotCreationComponent,
    SlotComponent,
    SlotEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [ReqResService,
    AuthService,
    JwtService,
    SearchService,
    { provide: Window, useValue: window },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
