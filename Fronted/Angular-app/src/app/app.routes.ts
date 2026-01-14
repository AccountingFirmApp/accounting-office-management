import { Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home';
import { WorkerCompaniesComponent } from '../components/worker-companies/worker-companies';
import { LoginComponent } from '../components/login/login';
import { ReportFormComponent } from '../components/report-form/report-form';
import { CompanyListComponent } from './components/companies/company-list.component';
import { CompanyCreateComponent } from './components/company-create/company-create';
import { CompanyTasksComponent } from './components/company-tasks/company-tasks';

// export const routes: Routes = [
//   { path: '', component: HomeComponent,title:'דף הבית' },  // ⬅️ דף הבית
//   { path: 'workers/:id/companies', component: WorkerCompaniesComponent ,title:'חברות עובדת'},  // ⬅️ דף חברות עובדת
//   { path: '**', redirectTo: '' },  // ⬅️ כל דף לא קיים -> חזרה לדף הבית
//  { 
//     path: 'reports', 
//     loadChildren: () => import('../components/reports-module').then(m => m.ReportsModule)
//   }
// ,];

// import { Routes } from '@angular/router';
// import { HomeComponent } from '../components/home/home';

// export const routes: Routes = [
//   { path: '', component: HomeComponent },
//   { 
//     path: 'reports',
//     loadChildren: () => import('../components/reports-module').then(m => m.REPORTS_ROUTES)
//   },
//   { path: '**', redirectTo: '' }
// ];

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'התחברות' },  // ⬅️ דף התחברות
  { path: 'home', component: HomeComponent, title: 'דף הבית' },  // ⬅️ דף הבית
  { path: 'workers/:id/companies', component: WorkerCompaniesComponent, title: 'חברות עובדת' },  // ⬅️ דף חברות עובדת
  {
    path: 'reports',
    loadChildren: () => import('../components/reports-module').then(m => m.ReportsModule)
  }, 
    // { path: 'reports/new', component: ReportFormComponent, title: 'חדש' },  // ⬅️ דף הבית
    { path: 'companies', component: CompanyListComponent,runGuardsAndResolvers: 'always'  },
    { path: 'companies/create', component: CompanyCreateComponent },  // ← חייב להיות לפני :id
    { path: 'companies/:id/edit', component: CompanyCreateComponent }, // ← עריכה
    { path: 'companies/:id/tasks', component: CompanyTasksComponent }, // ← משימות
  
  { path: '', redirectTo: '/login', pathMatch: 'full' },  // ⬅️ דף ראשי -> התחברות
  { path: '**', redirectTo: '/login' }  // ⬅️ כל דף לא קיים -> חזרה להתחברות
];
