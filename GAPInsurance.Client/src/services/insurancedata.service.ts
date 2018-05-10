import { Injectable } from "@angular/core";
import { Policy } from "../models/policy";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "../environments/environment";
import { PolicyCreationRequest } from "../models/policycreation.request";
import { Client } from "../models/client";

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

  public createPolicy(request: PolicyCreationRequest): Observable<Policy> {
    let policiesUrl = environment.apiBaseUrl + "/api/Policies";
    // The bearer interceptor automatically adds the relevant JWT
    return this.httpClient.post<Policy>(policiesUrl, request);
  }

  public deletePolicy(id: string): Observable<Object> {
    let policiesUrl = environment.apiBaseUrl + "/api/Policies/" + id;
    // The bearer interceptor automatically adds the relevant JWT
    return this.httpClient.delete(policiesUrl);
  }

  public getClients(): Observable<[Client]> {
    let clientsUrl = environment.apiBaseUrl + "/api/Clients";
    return this.httpClient.get<[Client]>(clientsUrl);
  }

  public createClient(name: string): Observable<Client> {
    let clientsUrl = environment.apiBaseUrl + "/api/Clients";
    return this.httpClient.post<Client>(clientsUrl, { name: name });
  }

  public deleteClient(id: string): Observable<Object> {
    let clientUrl = environment.apiBaseUrl + "/api/Clients/" + id;
    return this.httpClient.delete(clientUrl);
  }

  public updateClientPolicies(clientId: string, desiredPolicyIds: [string]): Observable<Client> {
    let policiesUrl = environment.apiBaseUrl + "/api/Clients/" + clientId + "/policies";
    return this.httpClient.patch<Client>(policiesUrl, desiredPolicyIds);
  }
}
