import { Component } from "@angular/core";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { MatDialogRef } from "@angular/material";

@Component({
  templateUrl: './clientcreation.dialog.html'
})
export class ClientCreationDialog {
  public clientName: string;

  public loadingData: boolean;
  public creationError: boolean;

  constructor(
    private insuranceService: InsuranceDataService,
    private dialogRef: MatDialogRef<ClientCreationDialog>
  ) { }

  public onCreateClicked(): void {
    this.loadingData = true;
    this.creationError = false;

    this.insuranceService.createClient(this.clientName)
      .subscribe(resultClient => {
        this.dialogRef.close({
          result: ClientCreationResult.Success,
          client: resultClient
        });
      }, error => {
        this.loadingData = false;
        this.creationError = true;
      });
  }

  public onCancelClicked(): void {
    this.dialogRef.close({
      result: ClientCreationResult.Cancellation
    });
  }
}

export class ClientCreationArguments {

}

export enum ClientCreationResult {
  Success,
  Cancellation
}
