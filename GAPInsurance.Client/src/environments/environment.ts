// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  auth0Config: {
    clientID: 'JDiNJXS0iHGb2EFceFANkMwprc74bw5l',
    domain: 'machinarius.auth0.com',
    responseType: 'token id_token',
    audience: 'https://machinarius.auth0.com/userinfo',
    redirectUri: 'http://localhost:4200/auth-callback',
    scope: 'openid'
  }
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
