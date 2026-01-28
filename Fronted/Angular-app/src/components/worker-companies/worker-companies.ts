import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { Router } from '@angular/router';
import { WorkerService } from '../../services/worker';
import { WorkerInfoDto } from '../../models/auth';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';

@Component({
  selector: 'app-worker-companies',
  standalone: true,
  imports: [CommonModule, BackButtonComponent],
  templateUrl: './worker-companies.html',
  styleUrls: ['./worker-companies.css']
})
export class WorkerCompaniesComponent implements OnInit {
  companies: any = null;
  loading = false;
  error = '';
  workerId = 3;

  constructor(
    private workerService: WorkerService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private location: Location
  ) { 
  }
  
  currentWorker!: WorkerInfoDto;

  ngOnInit(): void {
    this.currentWorker = this.workerService.currentWorker;

    if (this.currentWorker) {
      this.loadData();
    }
  }

  goHome(): void {
    this.location.back();
  }

  loadData(): void {
    this.loading = true;
    this.cdr.detectChanges();

    this.workerService.getWorkerCompanies(this.currentWorker.id).subscribe({
      next: (response) => {
        this.companies = response;
        console.log('🏢 טענת חברות עבור עובדת:', this.currentWorker.id, this.companies); // ← הוסף
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = `שגיאה: ${err.status} - ${err.message}`;
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  changeWorker(id: number): void {
    this.workerId = id;
    this.companies = null;
    this.error = '';
    this.cdr.detectChanges();
    this.loadData();
    this.router.navigate([`./workers/${this.workerId}/companies`]);
  }

  // מעבר למשימות של חברה
  goToCompanyTasks(companyId: number): void {
    this.router.navigate(['/companies', companyId, 'tasks']);
  }
goToCompanyReports(companyId: number): void {
  console.log('🏢 ID של החברה שלחצתי עליה:', companyId);
  this.router.navigate(['/reports'], { 
    queryParams: { companyId: companyId } 
  });
}

}