import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';  // ⬅️ הוסף
import { WorkerService } from '../../services/worker';

@Component({
  selector: 'app-worker-companies',
  standalone: true,
  imports: [CommonModule],
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
    private router: Router  // ⬅️ הוסף
  ) { 
    console.log('🚀 Component נוצר');
  }

  ngOnInit(): void {
    console.log('🚀 ngOnInit התחיל');
    this.loadData();
  }

  // ⬅️ הוסף פונקציה חדשה
  goHome(): void {
    this.router.navigate(['/']);
  }

  loadData(): void {
    this.loading = true;
    this.cdr.detectChanges();
    
    this.workerService.getWorkerCompanies(this.workerId).subscribe({
      next: (response) => {
        this.companies = response;
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
          this.router.navigate([`./workers/${this.workerId}/companies`])

  }
}