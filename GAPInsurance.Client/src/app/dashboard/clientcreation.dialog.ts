import { Component } from "@angular/core";
import { InsuranceDataService } from "../../services/insurancedata.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { Inject } from "@angular/core";
import { Client } from "../../models/client";
import { Observable } from "rxjs";

@Component({
  templateUrl: './clientcreation.dialog.html'
})
export class ClientCreationDialog {
  public clientName: string;

  public loadingData: boolean;
  public creationError: boolean;

  private sourceClient: Client;

  constructor(
    private insuranceService: InsuranceDataService,
    private dialogRef: MatDialogRef<ClientCreationDialog>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) { 
    this.sourceClient = data.sourceClient as Client;
    if (this.sourceClient) {
      this.clientName = this.sourceClient.name;
    }
  }

  public onCreateClicked(): void {
    this.loadingData = true;
    this.creationError = false;

    var jobObservable: Observable<Client>;
    if (this.sourceClient) {
      jobObservable = this.insuranceService.updateClient(this.sourceClient.id, this.clientName);
    } else {
      jobObservable = this.insuranceService.createClient(this.clientName);
    }

    jobObservable
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

export enum ClientCreationResult {
  Success,
  Cancellation
}
