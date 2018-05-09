import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppMaterialModule } from './app.material';
import { LandingComponent } from './landing/landing.component';
import { EntryPointComponent } from './entrypoint/entrypoint.component';
import { NotFoundComponent } from './notfound/notfound.component';
import { AuthService, BearerHttpInterceptor } from '../services/auth.service';
import { AuthCallbackComponent } from './authcallback/authcallback.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    EntryPointComponent,
    NotFoundComponent,
    AuthCallbackComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppMaterialModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: BearerHttpInterceptor,
    multi: true
  }, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
