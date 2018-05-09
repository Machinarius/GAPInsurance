import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../environments/environment";
import * as auth0 from 'auth0-js';

@Injectable()
export class AuthService {
  private webAuth: auth0.WebAuth;

  constructor(
    private router: Router
  ) {
    this.webAuth = new auth0.WebAuth(environment.auth0Config);
  }

  public async beginLogin(email: string, password: string): Promise<any> {
    let loginError = await this.loginInternal(email, password);
    if (loginError) {
      console.log(loginError);
      throw 'Unknown error during auth0 login';
    }
  }

  public async finishLogin(): Promise<any> {
    var authResult: auth0.Auth0DecodedHash;
    try {
      authResult = await this.parseHashInternal();
    } catch (ex) {
      console.log(ex);
      throw 'Unknown error during auth0 callback execution';
    }

    
  }

  private loginInternal(email: string, password: string): Promise<auth0.Auth0Error> {
    return new Promise((resolve, reject) => {
      this.webAuth.login({
        email: email,
        password: password
      }, reject);
    });
  }

  private parseHashInternal(): Promise<auth0.Auth0DecodedHash> {
    return new Promise((resolve, reject) => {
      this.webAuth.parseHash((err, authResult) => {
        if (err) {
          reject(err);
        } else {
          resolve(authResult);
        }
      })
    })
  }

  public userIsLoggedIn(): boolean {
    // TODO
    return false;
  }
}