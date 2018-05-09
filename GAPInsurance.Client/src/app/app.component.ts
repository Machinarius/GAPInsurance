import { Component, OnInit } from '@angular/core';
import { GAPUser } from '../models/gapuser';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  public get user(): GAPUser {
    if (!this.authService.userIsLoggedIn()) {
      return null;
    }

    return this.authService.currentUserProfile();
  }

  public onLogoutClicked() {
    this.authService.logout();
  }

  ngOnInit(): void {
    if (this.authService.userIsLoggedIn()) {
      this.router.navigateByUrl("/dashboard");
    }
  }
}
