import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportsRoutingModule } from './reports-routing-module';

import { ReportsDashboardComponent } from './reports-dashboard/reports-dashboard';
import { ReportsListComponent } from './reports-list/reports-list';
import { ReportFormComponent } from './report-form/report-form';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReportsRoutingModule,
    ReportsDashboardComponent,
    ReportsListComponent,
    ReportFormComponent,
    
  ]
})
export class ReportsModule { }
