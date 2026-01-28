import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ReportService } from '../../services/report';
import { CommonModule } from '@angular/common';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-reports-dashboard',
  templateUrl: './reports-dashboard.html',
  styleUrls: ['./reports-dashboard.css'],
  imports: [RouterOutlet, CommonModule, BackButtonComponent]
})
export class ReportsDashboardComponent implements OnInit {
  
  stats = {
    pending: 0,
    overdue: 0,
    reported: 0,
    paid: 0
  };

  constructor(
    private router: Router,
    private reportService: ReportService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.loadStatistics();
  }

  loadStatistics() {
    // נטען סטטיסטיקות מה-API
    this.reportService.getAll().subscribe({
      next: (reports) => {
        this.stats.pending = reports.filter(r => r.status === 'Pending').length;
        this.stats.reported = reports.filter(r => r.status === 'Reported').length;
        this.stats.paid = reports.filter(r => r.status === 'Paid').length;
        
        // חישוב דוחות באיחור
        const today = new Date();
        this.stats.overdue = reports.filter(r => {
          return (r.status === 'Pending' || r.status === 'Reported') &&
                 new Date(r.period) < today;
        }).length;
      },
      error: (err) => {
        console.error('Error loading stats:', err);
        // לא נציג alert כדי לא להפריע למשתמש
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }

  createNewReport() {
    this.router.navigate(['/reports/new']);
  }
}