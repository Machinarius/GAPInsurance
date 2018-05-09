import { Component } from "@angular/core";
import { OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";

@Component({
  templateUrl: './authcallback.component.html',
  styleUrls: ['../../styles/herocard.scss']
})
export class AuthCallbackComponent implements OnInit {
  constructor(
    private authService: AuthService
  ) { }

  public async ngOnInit(): Promise<any> {
    await this.authService.finishLogin();
  }
}