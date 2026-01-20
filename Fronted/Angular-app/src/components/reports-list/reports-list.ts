import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ReportService } from '../../services/report';
import { ReportInstanceDetail } from '../../models/report-instance';
import { WorkerService } from '../../services/worker';
import { ReportViewModalComponent } from '../report-view/report-view';

@Component({
  selector: 'app-reports-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReportViewModalComponent],
  templateUrl: './reports-list.html',
  styleUrls: ['./reports-list.css']
})
export class ReportsListComponent implements OnInit {
  reports: ReportInstanceDetail[] = [];
  filteredReports: ReportInstanceDetail[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  
  selectedReport: ReportInstanceDetail | null = null;
  isModalOpen: boolean = false;
  
  // פילטרים
  searchTerm: string = '';
  selectedStatus: string = 'all';
  selectedCompany: string = 'all';
  selectedReportType: string = 'all';
  
  // רשימות ייחודיות לפילטרים
  companies: string[] = [];
  reportTypes: string[] = [];
  statuses: string[] = ['Pending', 'Reported', 'Approved', 'Paid'];

  constructor(
    private reportService: ReportService,
    public workerService: WorkerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.reportService.getAll().subscribe({
      next: (data) => {
        this.reports = data;
        this.filteredReports = data;
        this.companies = [...new Set(data.map(r => r.companyName))].sort();
        this.reportTypes = [...new Set(data.map(r => r.reportTypeName))].sort();
        this.isLoading = false;
        console.log('✅ נטענו', data.length, 'דוחות');
      },
      error: (error) => {
        this.isLoading = false;
        console.error('❌ שגיאה בטעינת דוחות:', error);
        
        if (error.status === 401) {
          this.errorMessage = 'אין הרשאה - נא להתחבר מחדש';
        } else if (error.status === 500) {
          this.errorMessage = 'שגיאה בשרת - נסה שוב מאוחר יותר';
        } else {
          this.errorMessage = 'שגיאה בטעינת הדוחות';
        }
      }
    });
  }

  applyFilters(): void {
    this.filteredReports = this.reports.filter(report => {
      const matchesSearch = !this.searchTerm || 
        report.companyName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        report.reportTypeName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        report.reportTypeShortCode?.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesStatus = this.selectedStatus === 'all' || 
        report.status === this.selectedStatus;

      const matchesCompany = this.selectedCompany === 'all' || 
        report.companyName === this.selectedCompany;

      const matchesReportType = this.selectedReportType === 'all' ||
        report.reportTypeName === this.selectedReportType;

      return matchesSearch && matchesStatus && matchesCompany && matchesReportType;
    });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = 'all';
    this.selectedCompany = 'all';
    this.selectedReportType = 'all';
    this.filteredReports = this.reports;
  }

  /**
   * 🔥 בדיקה האם הדוח באיחור
   */
  isReportDelayed(report: ReportInstanceDetail): boolean {
    return report.daysOverdue !== null && report.daysOverdue !== undefined && report.daysOverdue > 0;
  }

  /**
   * 🔥 קבלת טקסט איחור
   */
  getDelayText(report: ReportInstanceDetail): string {
    if (!this.isReportDelayed(report)) return '';
    const days = report.daysOverdue!;
    return days === 1 ? 'איחור של יום אחד' : `איחור של ${days} ימים`;
  }

  viewReport(reportId: number): void {
    const report = this.reports.find(r => r.id === reportId);
    if (report) {
      this.selectedReport = report;
      this.isModalOpen = true;
    }
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedReport = null;
  }

  onEditFromModal(reportId: number): void {
    this.router.navigate(['/reports/edit', reportId]);
  }

  editReport(reportId: number): void {
    this.router.navigate(['/reports/edit', reportId]);
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'Reported': return 'status-reported';
      case 'Approved': return 'status-approved';
      case 'Paid': return 'status-paid';
      default: return '';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Pending': return 'ממתין';
      case 'Reported': return 'דווח';
      case 'Approved': return 'אושר';
      case 'Paid': return 'שולם';
      default: return status;
    }
  }

  getPaymentMethodText(method: string): string {
    switch (method) {
      case 'Transfer': return 'העברה בנקאית';
      case 'Credit': return 'כרטיס אשראי';
      case 'Check': return 'צ\'ק';
      case 'Cash': return 'מזומן';
      case 'Online': return 'תשלום מקוון';
      default: return method;
    }
  }

  formatDate(date: Date | null | undefined): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('he-IL');
  }

  formatAmount(amount: number | null | undefined): string {
    if (!amount) return '-';
    return new Intl.NumberFormat('he-IL', {
      style: 'currency',
      currency: 'ILS'
    }).format(amount);
  }

  getPendingCount(): number {
    return this.filteredReports.filter(r => r.status === 'Pending').length;
  }

  getReportedCount(): number {
    return this.filteredReports.filter(r => r.status === 'Reported').length;
  }

  getPaidCount(): number {
    return this.filteredReports.filter(r => r.status === 'Paid').length;
  }
}