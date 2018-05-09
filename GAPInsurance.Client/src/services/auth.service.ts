import { Injectable, Inject } from "@angular/core";
import { Router } from "@angular/router";
import {
  HttpInterceptor, HttpRequest, HttpHandler, HttpSentEvent,
  HttpHeaderResponse, HttpProgressEvent, HttpResponse,
  HttpUserEvent, HttpEvent
} from '@angular/common/http';

import { environment } from "../environments/environment";

import { GAPUser } from "../models/gapuser";

import * as auth0 from 'auth0-js';
import * as jwtDecode from 'jwt-decode';

import { Observable } from "rxjs";

@Injectable()
export class AuthService {
  private webAuth: auth0.WebAuth;

  private Constants = {
    AccessTokenKey: "access_token",
    IdTokenKey: "id_token",
    ExpirationDateKey: "expiration_date"
  };

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

    let expirationDate = (authResult.expiresIn * 1000) + new Date().getTime();
    let accessToken = authResult.accessToken;
    let idToken = authResult.idToken;

    localStorage.setItem(this.Constants.AccessTokenKey, accessToken);
    localStorage.setItem(this.Constants.ExpirationDateKey, expirationDate.toString());
    localStorage.setItem(this.Constants.IdTokenKey, idToken);

    this.router.navigateByUrl("/dashboard");
  }

  public logout() {
    localStorage.setItem(this.Constants.AccessTokenKey, null);
    localStorage.setItem(this.Constants.ExpirationDateKey, null);
    localStorage.setItem(this.Constants.IdTokenKey, null);

    this.router.navigateByUrl("/");
  }

  public userIsLoggedIn(): boolean {
    var idToken = localStorage.getItem(this.Constants.IdTokenKey);
    if (!idToken) {
      return false;
    }

    var expirationDateString = localStorage.getItem(this.Constants.ExpirationDateKey);
    if (!expirationDateString) {
      return false;
    }

    var expirationDate = parseInt(expirationDateString);
    if (expirationDate < new Date().getTime()) {
      return false;
    }

    return true;
  }

  public currentUserProfile(): GAPUser {
    if (!this.userIsLoggedIn()) {
      throw "Cannot read the profile when there is no user";
    }

    let idToken = localStorage.getItem(this.Constants.IdTokenKey);
    let idPayload = jwtDecode(idToken);

    let name = idPayload['nickname'];
    let email = idPayload['email'];
    let user = new GAPUser(name, email);

    return user;
  }

  public currentIdToken(): string {
    if (!this.userIsLoggedIn()) {
      throw "Cannot emit an id token if the user has not logged in";
    }

    let idToken = localStorage.getItem(this.Constants.IdTokenKey);
    return idToken;
  }

  private loginInternal(email: string, password: string): Promise<auth0.Auth0Error> {
    return new Promise((resolve, reject) => {
      this.webAuth.login({
        realm: environment.auth0Config.realm,
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
}

@Injectable()
export class BearerHttpInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.authService.userIsLoggedIn()) {
      return next.handle(req);
    }

    let idToken = this.authService.currentIdToken();
    let authenticatedRequest = req.clone({
      setHeaders: {
        "Authorization": `Bearer ${idToken}`
      }
    });

    return next.handle(authenticatedRequest);
  }
}
