import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";

@Component({ 
  template: '<p>Loading data...</p>'
})
export class EntryPointComponent implements OnInit {
  public constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (this.authService.userIsLoggedIn()) {
      this.router.navigateByUrl("/dashboard");
    } else {
      this.router.navigateByUrl("/landing");
    }
  }
}
