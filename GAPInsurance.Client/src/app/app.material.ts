import { MatButtonModule, MatToolbarModule, MatMenuModule,
         MatIconModule, MatSidenavModule, MatProgressBarModule,
         MatCardModule, MatInputModule, MatDialogModule,
         MatSelectModule, MatDatepickerModule, MatNativeDateModule } from '@angular/material';
import { NgModule } from "@angular/core";

let modules = [
  MatButtonModule,
  MatToolbarModule,
  MatMenuModule,
  MatIconModule,
  MatSidenavModule,
  MatProgressBarModule,
  MatCardModule,
  MatInputModule,
  MatDialogModule,
  MatSelectModule,
  MatDatepickerModule,
  MatNativeDateModule
];

@NgModule({
  imports: [modules],
  exports: [modules]
})
export class AppMaterialModule { }
