import { Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home';
import { WorkerCompaniesComponent } from '../components/worker-companies/worker-companies';
import { LoginComponent } from '../components/login/login';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'התחברות' },  // ⬅️ דף התחברות
  { path: 'home', component: HomeComponent, title: 'דף הבית' },  // ⬅️ דף הבית
  { path: 'workers/:id/companies', component: WorkerCompaniesComponent, title: 'חברות עובדת' },  // ⬅️ דף חברות עובדת
  { path: '', redirectTo: '/login', pathMatch: 'full' },  // ⬅️ דף ראשי -> התחברות
  { path: '**', redirectTo: '/login' }  // ⬅️ כל דף לא קיים -> חזרה להתחברות
];