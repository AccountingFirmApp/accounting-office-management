

import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ReportService } from '../../services/report';
import { ReportInstanceDetail } from '../../models/report-instance';
import { WorkerService } from '../../services/worker';
import { ReportViewModalComponent } from '../report-view/report-view';
import { LoadingComponent } from '../../app/components/shared/loading/loading.component';
import { ErrorMessageComponent } from '../../app/components/shared/error-message/error-message.component';

@Component({
  selector: 'app-reports-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReportViewModalComponent, LoadingComponent, ErrorMessageComponent],
  templateUrl: './reports-list.html',
  styleUrls: ['./reports-list.css']
})
export class ReportsListComponent implements OnInit, OnDestroy {
  reports: ReportInstanceDetail[] = [];
  filteredReports: ReportInstanceDetail[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  
  selectedReport: ReportInstanceDetail | null = null;
  isModalOpen: boolean = false;
  
  openWorkerPopoverId: number | null = null;
  selectedReportWorkers: string[] = [];

  // פילטרים
  searchTerm: string = '';
  selectedStatus: string = 'all';
  selectedCompany: string = 'all';
  selectedReportType: string = 'all';
  
  filterByCompanyId: number | null = null;
  isAdminMode: boolean = false;
  
  companies: string[] = [];
  reportTypes: string[] = [];
  statuses: string[] = ['Pending', 'Reported', 'Approved', 'Paid'];

  constructor(
    private reportService: ReportService,
    public workerService: WorkerService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.filterByCompanyId = params['companyId'] ? +params['companyId'] : null;
      this.isAdminMode = params['adminMode'] === 'true';
      
    
      
      this.loadReports();
    });
    
    document.addEventListener('click', () => {
      this.closeWorkerPopover();
    });
  }

  ngOnDestroy(): void {
    document.removeEventListener('click', () => {
      this.closeWorkerPopover();
    });
  }

  openWorkerPopover(report: ReportInstanceDetail, event: Event): void {
    event.stopPropagation();
    this.selectedReportWorkers = report.workerNames || [];
    this.openWorkerPopoverId = report.id;
  }

  closeWorkerPopover(): void {
    this.openWorkerPopoverId = null;
    this.selectedReportWorkers = [];
  }

  getWorkerCount(report: ReportInstanceDetail): number {
    return report.workerNames?.length || 0;
  }

  hasWorkers(report: ReportInstanceDetail): boolean {
    return report.workerNames && report.workerNames.length > 0;
  }

  // ========================================
  // טעינת דוחות
  // ========================================

  loadReports(): void {
    this.isLoading = true;
    this.errorMessage = '';

    

    this.reportService.getAll(this.isAdminMode).subscribe({
      next: (data) => {
      
        
        this.reports = data;

        if (this.filterByCompanyId) {
      
          this.reports = data.filter(r => r.companyId === this.filterByCompanyId);
    
        }

        this.filteredReports = this.reports;
        this.populateFilterOptions();
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'שגיאה בטעינת הדוחות';
      }
    });
  }

  populateFilterOptions(): void {
    this.companies = [...new Set(this.reports.map(r => r.companyName))].sort();
    this.reportTypes = [...new Set(this.reports.map(r => r.reportTypeName))].sort();
    
  
  }

  // ========================================
  // פילטרים
  // ========================================

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

  // ========================================
  // פונקציות עזר
  // ========================================

  isReportDelayed(report: ReportInstanceDetail): boolean {
    return report.daysOverdue !== null && report.daysOverdue !== undefined && report.daysOverdue > 0;
  }

  getDelayText(report: ReportInstanceDetail): string {
    if (!this.isReportDelayed(report)) return '';
    const days = report.daysOverdue!;
    return days === 1 ? 'איחור של יום אחד' : `איחור של ${days} ימים`;
  }

  // ========================================
  // פעולות על דוחות
  // ========================================

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

  // ========================================
  // פורמט וטקסטים
  // ========================================

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