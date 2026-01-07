import { Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home';
import { WorkerCompaniesComponent } from '../components/worker-companies/worker-companies';

export const routes: Routes = [
  { path: '', component: HomeComponent,title:'דף הבית' },  // ⬅️ דף הבית
  { path: 'workers/:id/companies', component: WorkerCompaniesComponent ,title:'חברות עובדת'},  // ⬅️ דף חברות עובדת
  { path: '**', redirectTo: '' }  // ⬅️ כל דף לא קיים -> חזרה לדף הבית
];