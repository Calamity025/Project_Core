import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService, JwtService } from './services'
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { TokenInterceptor } from './interceptor/token.interceptor';
import { ProfileCreationComponent } from './profile/profile-creation/profile-creation.component';
import { AuthorizationComponent } from './authorization/authorization.component';
import { SearchComponent } from './home/search/search-property/search-property.component';
import { SlotsComponent } from './home/slots/slots/slots.component';
import { SearchService } from './services/search.service';
import { ProfileComponent } from './profile/profile/profile.component';
import { ProfileService } from './services/profile.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SearchComponent,
    SlotsComponent,
    ProfileCreationComponent,
    AuthorizationComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [
    AuthService,
    JwtService,
    SearchService,
    ProfileService,
    { provide: Window, useValue: window },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
