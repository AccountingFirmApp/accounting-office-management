import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WorkerService } from '../../../services/worker';
import { WorkerInfoDto } from '../../../models/auth';
import { AuthService } from '../../../services/auth.service';
import { BackButtonComponent } from '../shared/back-button/back-button.component';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [CommonModule, BackButtonComponent],
  templateUrl: './management.component.html',
  styleUrl: './management.component.css'
})
export class ManagementComponent implements OnInit {
  currentWorker!: WorkerInfoDto;

  constructor(
    private router: Router,
    private workerService: WorkerService,
    public auth: AuthService
  ) { }

  ngOnInit(): void {
    this.currentWorker = this.workerService.currentWorker;
    // אם לא מנהל, חזור לדף הבית
    if (!this.auth.isAdmin()) {
      this.router.navigate(['/home']);
    }
  }

  navigateToCompanies(): void {
    this.router.navigate(['/companies']);
  }

  navigateToWorkers(): void {
    this.router.navigate(['/workers']);
  }

  // navigateToReports(): void {
  //   this.router.navigate(['/reports']);
  // }

 navigateToReports(): void {
 // 🔥 מוסיפים adminMode=true כשנכנסים מפאנל ניהול
  this.router.navigate(['/reports'], { 
    queryParams: { adminMode: 'true' } 
  });}

  navigateToStatistics(): void {
    // בקרוב
  }

  navigateToSettings(): void {
    this.router.navigate(['/settings/report-config']);
  }
}
