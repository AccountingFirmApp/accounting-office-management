import { Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home';
import { WorkerCompaniesComponent } from '../components/worker-companies/worker-companies';

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
  { path: '', component: HomeComponent, title: 'דף הבית' },
  { path: 'workers/:id/companies', component: WorkerCompaniesComponent, title: 'חברות עובדת' },
  { 
    path: 'reports', 
    loadChildren: () => import('../components/reports-module').then(m => m.ReportsModule)
  },
  { path: '**', redirectTo: '' }
];
