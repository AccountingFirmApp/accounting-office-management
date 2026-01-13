import { Routes } from '@angular/router';
import { CompanyListComponent } from './components/companies/company-list.component';
 import { CompanyCreateComponent } from './components/company-create/company-create';
 import { CompanyTasksComponent } from './components/company-tasks/company-tasks';

export const routes: Routes = [
  { path: '', redirectTo: '/companies', pathMatch: 'full' },
  { path: 'companies', component: CompanyListComponent,runGuardsAndResolvers: 'always'  },
  { path: 'companies/create', component: CompanyCreateComponent },  // ← חייב להיות לפני :id
  { path: 'companies/:id/edit', component: CompanyCreateComponent }, // ← עריכה
  { path: 'companies/:id/tasks', component: CompanyTasksComponent }, // ← משימות
  { path: '**', redirectTo: '/companies' }
];