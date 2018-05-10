import { Component, ViewChild, ElementRef, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material";
import { PolicyCreationDialog } from "./policycreation.dialog";
import { ClientCreationDialog } from "./clientcreation.dialog";
import { Policy } from "../../models/policy";
import { InsuranceDataService } from "../../services/insurancedata.service";

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['../../styles/maincolumn.scss', './dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  constructor(
    private dataService: InsuranceDataService,
    private dialog: MatDialog
  ) { }

  public policies: Policy[];
  public loadingPolicies: boolean;
  public policiesLoadError: boolean;

  ngOnInit(): void {
    this.loadPolicies();
  }

  public loadPolicies(): void {
    this.policies = null;
    this.loadingPolicies = true;
    this.policiesLoadError = false;

    this.dataService.getPolicies()
      .subscribe((data: [Policy]) => {
        this.loadingPolicies = false;
        this.policies = data;
      }, (error) => {
        this.loadingPolicies = false;
        this.policiesLoadError = true;
      });
  }

  public onCreatePolicyClicked(): void {
    let creationDialog = this.dialog.open(PolicyCreationDialog);
  }

  public onCreateClientClicked(): void {
    let creationDialog = this.dialog.open(ClientCreationDialog);
  }
}
