import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkerService } from '../../services/worker';

@Component({
  selector: 'app-worker-companies',
  standalone: true,  // ⬅️ זה הקומפוננט Standalone!
  imports: [CommonModule],  // ⬅️ ייבוא של ngIf, ngFor וכו'
  templateUrl: './worker-companies.html',
  styleUrls: ['./worker-companies.css']
})
export class WorkerCompaniesComponent implements OnInit {
  data: any;
  loading = false;
  error = '';

  constructor(private workerService: WorkerService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    
    this.workerService.getWorkerCompanies(3).subscribe({
      next: (response) => {
        this.data = response;
        this.loading = false;
        console.log('התקבל מהשרת:', response);
      },
      error: (err) => {
        this.error = err.message;
        this.loading = false;
        console.error('שגיאה:', err);
      }
    });
  }
}