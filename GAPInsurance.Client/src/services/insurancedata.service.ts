import { Injectable } from "@angular/core";
import { Policy } from "../models/policy";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "../environments/environment";
import { PolicyCreationRequest } from "../models/policycreation.request";

@Injectable()
export class InsuranceDataService {
  constructor(
    private httpClient: HttpClient
  ) { }

  public getPolicies(): Observable<[Policy]> {
    let policiesUrl = environment.apiBaseUrl + "/api/Policies";
    // The bearer interceptor automatically adds the relevant JWT
    return this.httpClient.get<[Policy]>(policiesUrl);
  }

  public createPolicy(request: PolicyCreationRequest): Observable<Object> {
    let policiesUrl = environment.apiBaseUrl + "/api/Policies";
    // The bearer interceptor automatically adds the relevant JWT
    return this.httpClient.post(policiesUrl, request);
  }

  public deletePolicy(id: string): Observable<Object> {
    let policiesUrl = environment.apiBaseUrl + "/api/Policies/" + id;
    // The bearer interceptor automatically adds the relevant JWT
    return this.httpClient.delete(policiesUrl);
  }
}
