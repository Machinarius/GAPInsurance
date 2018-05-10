import { Component } from "@angular/core";

@Component({
  templateUrl: './policycreation.dialog.html',
  styleUrls: ['./dialogs.scss']
})
export class PolicyCreationDialog {

}

export class PolicyCreationArguments {

}

export enum PolicyCreationResult {
  Failure,
  Cancellation,
  Success
}
