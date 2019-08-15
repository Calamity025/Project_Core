import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AuthService, JwtService, ReqResService } from '../app/authorization/services'
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { TokenInterceptor } from './authorization/interceptor/token.interceptor';
import { ProfileCreationComponent } from './profile-creation/profile-creation.component';
import { AuthorizationComponent } from './authorization/authorization.component';
import { Globals } from './Globals.component';
import { SlotCreationComponent } from './slots/slot-creation/slot-creation.component';
import { SearchComponent } from './home/search/search-property/search-property.component';
import { SlotsComponent } from './home/slots/slots/slots.component';
import { SearchService } from './home/search/search.service';
import { SlotsService } from './home/slots/slots.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchComponent,
    SlotsComponent,
    ProfileCreationComponent,
    AuthorizationComponent,
    SlotCreationComponent
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
    SlotsService,
    Globals,
    { provide: Window, useValue: window },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
