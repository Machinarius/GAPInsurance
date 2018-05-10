import { Component } from "@angular/core";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { PolicyCreationRequest } from "../../models/policycreation.request";
import { MatDialogRef } from "@angular/material";

@Component({
  templateUrl: './policycreation.dialog.html',
  styleUrls: ['./dialogs.scss']
})
export class PolicyCreationDialog {
  public name: string;
  public description: string;
  public premiumPrice: number;
  public startDate: string;
  public coverageLength: number;
  public riskLevelId: number;
  public earthquakeCoverage: number;
  public fireCoverage: number;
  public theftCoverage: number;
  public lossCoverage: number;

  public loadingData: boolean;
  public creationError: boolean;

  constructor(
    private insuranceService: InsuranceDataService,
    private dialogRef: MatDialogRef<PolicyCreationDialog>
  ) { }

  public onCreateClicked(): void {
    this.loadingData = true;
    this.creationError = false;

    let request = new PolicyCreationRequest(this.name, this.description,
      this.premiumPrice, this.startDate, this.coverageLength, this.riskLevelId,
      this.earthquakeCoverage, this.fireCoverage, this.theftCoverage, this.lossCoverage);
    this.insuranceService.createPolicy(request)
      .subscribe((resultPolicy) => {
        this.dialogRef.close({
          result: PolicyCreationResult.Success,
          policy: resultPolicy
        });
      }, (error) => {
        this.loadingData = false;
        this.creationError = true;
      });
  }

  public onCancelClicked(): void {
    this.dialogRef.close({
      result: PolicyCreationResult.Cancellation
    });
  }
}

export class PolicyCreationArguments {

}

export enum PolicyCreationResult {
  Cancellation,
  Success
}
