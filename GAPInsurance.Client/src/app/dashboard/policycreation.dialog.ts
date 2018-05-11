import { Component } from "@angular/core";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { PolicyCreationRequest } from "../../models/policycreation.request";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { Policy } from "../../models/policy";
import { Inject } from "@angular/core";
import { Observable } from "rxjs";

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

  private sourcePolicy: Policy;

  constructor(
    private insuranceService: InsuranceDataService,
    private dialogRef: MatDialogRef<PolicyCreationDialog>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.sourcePolicy = data.sourcePolicy as Policy; 
    if (!this.sourcePolicy) {
      return;
    }

    this.name = this.sourcePolicy.name;
    this.description = this.sourcePolicy.description;
    this.premiumPrice = this.sourcePolicy.premiumPrice;
    this.startDate = this.sourcePolicy.coverageStartDate;
    this.coverageLength = this.sourcePolicy.coverageLength;
    this.riskLevelId = this.sourcePolicy.riskLevelId;
    this.earthquakeCoverage = this.sourcePolicy.earthquakeCoverage;
    this.fireCoverage = this.sourcePolicy.fireCoverage;
    this.theftCoverage = this.sourcePolicy.theftCoverage;
    this.lossCoverage = this.sourcePolicy.lossCoverage;
  }

  public onCreateClicked(): void {
    this.loadingData = true;
    this.creationError = false;

    let request = new PolicyCreationRequest(this.name, this.description,
      this.premiumPrice, this.startDate, this.coverageLength, this.riskLevelId,
      this.earthquakeCoverage, this.fireCoverage, this.theftCoverage, this.lossCoverage);

    var jobObservable: Observable<Policy>;
    if (this.sourcePolicy) {
      jobObservable = this.insuranceService.updatePolicy(this.sourcePolicy.id, request);
    } else {
      jobObservable = this.insuranceService.createPolicy(request);
    }

    jobObservable
      .subscribe(resultPolicy => {
        this.dialogRef.close({
          result: PolicyCreationResult.Success,
          policy: resultPolicy
        });
      }, error => {
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
