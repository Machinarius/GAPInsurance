import { Component, ViewChild, ElementRef, OnInit } from "@angular/core";
import { MatDialog, MatSnackBar } from "@angular/material";
import { PolicyCreationDialog, PolicyCreationResult } from "./policycreation.dialog";
import { ClientCreationDialog, ClientCreationResult } from "./clientcreation.dialog";
import { Policy } from "../../models/policy";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { Client } from "../../models/client";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";
import { PolicyAssignmentDialog, PolicyAssignmentResult } from "./policyassignment.dialog";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['../../styles/maincolumn.scss', './dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  constructor(
    private dataService: InsuranceDataService,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    private authService: AuthService,
    private router: Router
  ) { }

  public policies: Policy[];
  public loadingPolicies: boolean;
  public policiesLoadError: boolean;

  public clients: Client[];
  public loadingClients: boolean;
  public clientsLoadError: boolean;

  ngOnInit(): void {
    if (!this.authService.userIsLoggedIn()) {
      this.router.navigateByUrl("/");
      return;
    }

    this.loadPolicies();
    this.loadClients();
  }

  public loadPolicies(): void {
    this.policies = null;
    this.loadingPolicies = true;
    this.policiesLoadError = false;

    this.dataService.getPolicies()
      .subscribe(remotePolicies => {
        this.loadingPolicies = false;
        this.policies = remotePolicies;
      }, error => {
        this.loadingPolicies = false;
        this.policiesLoadError = true;
      });
  }

  public loadClients(): void {
    this.clients = null;
    this.loadingClients = true;
    this.clientsLoadError = false;

    this.dataService.getClients()
      .subscribe(remoteClients => {
        this.loadingClients = false;
        this.clients = remoteClients;
      }, error => {
        this.loadingClients = false;
        this.clientsLoadError = true;
      })
  }

  public onCreatePolicyClicked(): void {
    let creationDialog = this.dialog.open(PolicyCreationDialog);
    creationDialog
      .afterClosed()
      .subscribe(dialogResult => {
        if (!dialogResult) {
          return;
        }
    
        let creationResult = dialogResult.result as PolicyCreationResult;
        if (creationResult != PolicyCreationResult.Success) {
          return;
        }
    
        let createdPolicy = dialogResult.policy as Policy;
        this.snackbar.open(`"${createdPolicy.name}" was created successfully`, "OK", {
          duration: 5000
        });
        this.policies.unshift(createdPolicy);
      });
  }

  public onCreateClientClicked(): void {
    let creationDialog = this.dialog.open(ClientCreationDialog);
    creationDialog
      .afterClosed()
      .subscribe(dialogResult => {
        if (!dialogResult) {
          return;
        }
    
        let creationResult = dialogResult.Result as ClientCreationResult;
        if (creationResult != ClientCreationResult.Success) {
          return;
        }
    
        let createdClient = dialogResult.client as Client;
        this.snackbar.open(`'${createdClient.name}' was created successfully`, "OK", {
          duration: 5000
        });
        this.clients.unshift(createdClient);
      });
  }

  public onDeletePolicyClicked(policy: Policy): void {
    this.snackbar.open(`Deleting policy '${policy.name}'...`, "OK", {
      duration: 1000
    });

    this.dataService.deletePolicy(policy.id)
      .subscribe(result => {
        let policyIndex = this.policies.indexOf(policy);
        this.policies.splice(policyIndex, 1);

        this.snackbar.open(`Policy '${policy.name}' deleted`, "OK", {
          duration: 3000
        });
      }, error => {
        this.snackbar.open(`Policy '${policy.name}' could not be deleted. Try again later.`, "OK", {
          duration: 1000
        });
      });
  }

  public onDeleteClientClicked(client: Client): void {
    this.snackbar.open(`Deleting client '${client.name}'...`, "OK", {
      duration: 1000
    });

    this.dataService.deleteClient(client.id)
      .subscribe(result => {
        let clientIndex = this.clients.indexOf(client);
        this.clients.splice(clientIndex, 1);

        this.snackbar.open(`Client '${client.name}' deleted`, "OK", {
          duration: 3000
        });
      }, error => {
        this.snackbar.open(`Client '${client.name}' could not be deleted. Try again later.`, "OK", {
          duration: 1000
        });
      });
  }

  public onEditClientClicked(client: Client): void {
    let updateDialog = this.dialog.open(ClientCreationDialog, {
      data: {
        sourceClient: client
      }
    });

    updateDialog
      .afterClosed()
      .subscribe(dialogResult => {
        if (!dialogResult) {
          return;
        }
    
        let updateResult = dialogResult.result as ClientCreationResult;
        if (updateResult != ClientCreationResult.Success) {
          return;
        }
    
        let updatedClient = dialogResult.client as Client;
        this.snackbar.open(`'${updatedClient.name}' was updated successfully`, "OK", {
          duration: 5000
        });

        let clientIndex = this.clients.indexOf(client);
        this.clients[clientIndex] = updatedClient;
      });
  }

  public onModifyPoliciesClicked(client: Client): void {
    if (!this.policies || this.loadingPolicies) {
      this.snackbar.open("Please wait until the policies have been loaded", "OK", {
        duration: 1000
      });

      return;
    }

    if (this.policies.length == 0) {
      this.snackbar.open("Please create a policy to continue", "OK", {
        duration: 1000
      });

      return;
    }

    let assignmentDialog = this.dialog.open(PolicyAssignmentDialog, {
      data: {
        client: client,
        policies: this.policies
      }
    });

    assignmentDialog
      .afterClosed()
      .subscribe(dialogResult => {
        if (!dialogResult) {
          return;
        }

        let assignmentResult = dialogResult.result as PolicyAssignmentResult;
        if (assignmentResult != PolicyAssignmentResult.Success) {
          return;
        }

        let clientResult = dialogResult.client as Client;
        let clientIndex = this.clients.indexOf(client);
        this.clients[clientIndex] = clientResult;
        this.loadPolicies();

        this.snackbar.open(`${clientResult.name}'s policies have been updated successfully`, "OK", {
          duration: 5000
        });

        /*
        this.policies.forEach(policy => {
          let matchingClientPolicies = clientResult.assignedPolicies
            .map((p, index) => { return { policy: p, index: index }})
            .filter(p => p.policy.id == policy.id);
          let matchingPolicyClients = policy.coveredClients
            .map((c, index) => { return { client: c, index: index }})
            .filter(c => c.client.id == clientResult.id);
        });
        */
      });
  }
}
