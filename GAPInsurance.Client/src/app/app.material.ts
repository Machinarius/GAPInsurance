import { MatButtonModule, MatToolbarModule, MatMenuModule,
         MatIconModule, MatSidenavModule, MatProgressBarModule,
         MatCardModule, MatInputModule } from '@angular/material';
import { NgModule } from "@angular/core";

let modules = [
    MatButtonModule,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule,
    MatSidenavModule,
    MatProgressBarModule,
    MatCardModule,
    MatInputModule
];

@NgModule({
    imports: [modules],
    exports: [modules]
})
export class AppMaterialModule { }