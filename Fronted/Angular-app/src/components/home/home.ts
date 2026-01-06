import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent {
  
  constructor(private router: Router) { }

  // ניווט לדף חברות עובדת
  navigateToWorkerCompanies(): void {
    this.router.navigate(['/workers/3/companies']);
  }
}