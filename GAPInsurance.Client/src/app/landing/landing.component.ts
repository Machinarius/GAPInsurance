import { Component } from "@angular/core";
import { AuthService } from "../../services/auth.service";

@Component({
    templateUrl: './landing.component.html',
    styleUrls: ['./landing.component.scss', '../../styles/herocard.scss']
})
export class LandingComponent {
  public username: string;
  public password: string;

  constructor(
    private authService: AuthService
  ) { }

  public async login(): Promise<any> {
    await this.authService.beginLogin(this.username, this.password);
  }
}