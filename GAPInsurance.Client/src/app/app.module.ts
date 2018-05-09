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
import { AuthService } from '../services/auth.service';
import { AuthCallbackComponent } from './authcallback/authcallback.component';


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
  providers: [
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
