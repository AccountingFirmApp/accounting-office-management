import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ReportService } from '../../services/report';
import { CommonModule } from '@angular/common';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { AuthService } from '../../services/auth.service';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-reports-dashboard',
  templateUrl: './reports-dashboard.html',
  styleUrls: ['./reports-dashboard.css'],
  imports: [RouterModule, CommonModule, BackButtonComponent]
})
export class ReportsDashboardComponent implements OnInit {

  stats = {
    pending: 0,
    overdue: 0,
    reported: 0,
    paid: 0
  };

  filterByCompanyId: number | null = null;
  companyName: string = '';
  isAdminMode: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private reportService: ReportService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    // קריאה לפרמטרים מה-URL
    this.route.queryParams.subscribe(params => {
      this.filterByCompanyId = params['companyId'] ? +params['companyId'] : null;
      this.isAdminMode = params['adminMode'] === 'true';
      this.loadStatistics();
    });
  }

  loadStatistics() {
    this.reportService.getAll(this.isAdminMode).subscribe({
      next: (reports) => {
        let filteredReports = reports;

        // פילטר לפי companyId אם קיים
        if (this.filterByCompanyId !== null) {
          filteredReports = reports.filter(r => r.companyId === this.filterByCompanyId);

          if (filteredReports.length > 0) {
            this.companyName = filteredReports[0].companyName;
          } else {
            this.companyName = '';
          }
        }

        // חישוב סטטיסטיקות
        this.stats.pending = filteredReports.filter(r => r.status === 'Pending').length;
        this.stats.reported = filteredReports.filter(r => r.status === 'Reported').length;
        this.stats.paid = filteredReports.filter(r => r.status === 'Paid').length;

        const today = new Date();
        this.stats.overdue = filteredReports.filter(r => 
          (r.status === 'Pending' || r.status === 'Reported') &&
          new Date(r.period) < today
        ).length;
      },
      error: (err) => {
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
