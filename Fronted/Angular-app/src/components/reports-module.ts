import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportsRoutingModule } from './reports-routing-module';

// קומפוננטות standalone
import { ReportsDashboardComponent } from './reports-dashboard/reports-dashboard';
import { ReportsListComponent } from './reports-list/reports-list';
import { ReportFormComponent } from './report-form/report-form';
// import { ReportCardComponent } from './report-card/report-card';

@NgModule({
  declarations: [
    // אין קומפוננטות standalone ב-declarations
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReportsRoutingModule,
    ReportsDashboardComponent,
    ReportsListComponent,
    ReportFormComponent,
    // ReportCardComponent
  ]
})
export class ReportsModule { }
