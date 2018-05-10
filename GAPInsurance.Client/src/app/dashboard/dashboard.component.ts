import { Component, ViewChild, ElementRef } from "@angular/core";
import { MatDialog } from "@angular/material";
import { PolicyCreationDialog } from "./policycreation.dialog";
import { ClientCreationDialog } from "./clientcreation.dialog";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['../../styles/maincolumn.scss', './dashboard.component.scss']
})
export class DashboardComponent {
  constructor(
    private dialog: MatDialog
  ) { }

  public onCreatePolicyClicked(): void {
    let creationDialog = this.dialog.open(PolicyCreationDialog);
  }

  public onCreateClientClicked(): void {
    let creationDialog = this.dialog.open(ClientCreationDialog);
  }
}
