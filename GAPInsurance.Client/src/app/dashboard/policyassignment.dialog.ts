import { Component } from "@angular/core";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { Inject } from "@angular/core";
import { Client } from "../../models/client";
import { Policy } from "../../models/policy";

@Component({
  templateUrl: './policyassignment.dialog.html',
  styleUrls: ['./policyassignment.dialog.css']
})
export class PolicyAssignmentDialog {
  public client: Client;
  public policies: [Policy];
  public assignments: PolicyAssignment[];

  public loadingData: boolean;
  public updateError: boolean;

  constructor(
    private insuranceService: InsuranceDataService,
    private dialogRef: MatDialogRef<PolicyAssignmentDialog>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) { 
    this.client = data.client as Client;
    this.policies = data.policies as [Policy];

    this.assignments = this.policies.map(policy => {
      let matchingPolicies = this.client.assignedPolicies.filter(_policy => _policy.id == policy.id);
      return new PolicyAssignment(policy.id, policy.name, matchingPolicies.length > 0);
    });
  }

  public onSaveClicked(): void {
    let desiredPolicyIds = this.assignments
      .filter(assignment => assignment.active)
      .map(assignment => assignment.policyId);

    this.loadingData = true;
    this.updateError = false;

    this.insuranceService.updateClientPolicies(this.client.id, desiredPolicyIds)
      .subscribe(resultClient => {
        this.dialogRef.close({
          result: PolicyAssignmentResult.Success,
          client: resultClient
        });
      }, error => {
        this.loadingData = false;
        this.updateError = true;
      });
  }

  public onCancelClicked(): void {
    this.dialogRef.close({
      result: PolicyAssignmentResult.Canceled
    });
  }
}

class PolicyAssignment {
  constructor(
    public policyId: string,
    public policyName: string,
    public active: boolean
  ) { }
}

export enum PolicyAssignmentResult {
  Success,
  Canceled
}
