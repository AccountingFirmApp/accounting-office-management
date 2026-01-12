import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportsDashboardComponent } from './reports-dashboard/reports-dashboard';
import { ReportsListComponent } from './reports-list/reports-list';
import { ReportFormComponent } from './report-form/report-form';

const routes: Routes = [
  {
    path: '',
    component: ReportsDashboardComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: ReportsListComponent },
      { path: 'new', component: ReportFormComponent },
      { path: 'edit/:id', component: ReportFormComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }