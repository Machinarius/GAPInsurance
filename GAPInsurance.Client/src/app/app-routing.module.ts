import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { EntryPointComponent } from './entrypoint/entrypoint.component';
import { NotFoundComponent } from './notfound/notfound.component';
import { AuthCallbackComponent } from './authcallback/authcallback.component';

const routes: Routes = [
  { path: 'landing', component: LandingComponent },
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: '', component: EntryPointComponent, pathMatch: 'full' },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
