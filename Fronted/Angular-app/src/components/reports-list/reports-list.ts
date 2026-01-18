import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ReportService } from '../../services/report';
import { ReportInstanceDetail } from '../../models/report-instance';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportViewModalComponent } from "../report-view/report-view";
// import { ReportViewModalComponent } from '../../models/reportview';

@Component({
  standalone: true,
  selector: 'app-reports-list',
  imports: [CommonModule, FormsModule, ReportViewModalComponent],
  templateUrl: './reports-list.html',
  styleUrls: ['./reports-list.css']
})
export class ReportsListComponent implements OnInit {
  
  reports: ReportInstanceDetail[] = [];
  filteredReports: ReportInstanceDetail[] = [];
  loading = true;
  
  searchTerm = '';
  selectedStatus: string | null = null;
  
  sortColumn: string = 'period';
  sortDirection: 'asc' | 'desc' = 'desc';

  constructor(
    private reportService: ReportService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    this.loading = true;
    this.reportService.getAll().subscribe({
      next: (data) => {
        this.reports = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading reports:', err);
        alert('שגיאה בטעינת דוחות');
        this.loading = false;
      }
    });
  }

  applyFilters() {
    let filtered = [...this.reports];

    // חיפוש - ✅ תוקן! עכשיו גישה ישירה ל-companyName
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(r => 
        r.companyName?.toLowerCase().includes(term) ||
        r.reportTypeName?.toLowerCase().includes(term)
      );
    }

    // סינון לפי סטטוס
    if (this.selectedStatus) {
      if (this.selectedStatus === 'overdue') {
        filtered = filtered.filter(r => this.isOverdue(r));
      } else {
        filtered = filtered.filter(r => r.status === this.selectedStatus);
      }
    }

    this.filteredReports = filtered;
    this.applySorting();
  }

  filterByStatus(status: string | null) {
    this.selectedStatus = status;
    this.applyFilters();
  }

  sortBy(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.applySorting();
  }

  applySorting() {
    this.filteredReports.sort((a, b) => {
      let aVal: any;
      let bVal: any;

      // ✅ תוקן! גישה ישירה לשדות
      switch(this.sortColumn) {
        case 'companyName':
          aVal = a.companyName;
          bVal = b.companyName;
          break;
        case 'reportTypeName':
          aVal = a.reportTypeName;
          bVal = b.reportTypeName;
          break;
        case 'period':
          aVal = new Date(a.period).getTime();
          bVal = new Date(b.period).getTime();
          break;
        case 'amount':
          aVal = a.amount || 0;
          bVal = b.amount || 0;
          break;
        default:
          return 0;
      }

      if (aVal < bVal) return this.sortDirection === 'asc' ? -1 : 1;
      if (aVal > bVal) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
  }

  getSortIcon(column: string): string {
    if (this.sortColumn !== column) return '⇅';
    return this.sortDirection === 'asc' ? '↑' : '↓';
  }

  isOverdue(report: ReportInstanceDetail): boolean {
    if (!report.period) return false;
    const today = new Date();
    const period = new Date(report.period);
    return (report.status === 'Pending' || report.status === 'Reported') && period < today;
  }

  formatDate(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    return d.toLocaleDateString('he-IL', { year: 'numeric', month: '2-digit' });
  }

  getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'Pending': 'pending',
      'Reported': 'reported',
      'Paid': 'paid',
      'Approved': 'approved',
      'NotRequired': 'not-required'
    };
    return statusMap[status] || '';
  }

  getStatusLabel(status: string): string {
    const labels: Record<string, string> = {
      'Pending': 'ממתין',
      'Reported': 'דווח',
      'Paid': 'שולם',
      'Approved': 'אושר',
      'NotRequired': 'לא נדרש'
    };
    return labels[status] || status;
  }

  getPaymentMethodLabel(method: string): string {
    const labels: Record<string, string> = {
      'Credit': 'כרטיס אשראי',
      'Transfer': 'העברה בנקאית',
      'Check': 'צ\'ק',
      'Online': 'תשלום מקוון',
      'Cash': 'מזומן'
    };
    return labels[method] || method;
  }

  // Modal state
  selectedReport: ReportInstanceDetail | null = null;
  isModalOpen = false;

  viewReport(id: number) {
    // מצא את הדוח לפי ID
    const report = this.filteredReports.find(r => r.id === id);
    if (report) {
      this.selectedReport = report;
      this.isModalOpen = true;
    }
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedReport = null;
  }

  onEditFromModal(id: number) {
    this.router.navigate(['/reports/edit', id]);
  }

  editReport(id: number) {
    this.router.navigate(['/reports/edit', id]);
  }

  deleteReport(id: number) {
    if (confirm('האם אתה בטוח שברצונך למחוק דוח זה?')) {
      this.reportService.delete(id).subscribe({
        next: () => {
          alert('הדוח נמחק בהצלחה! ✅');
          this.loadReports();
        },
        error: (err) => {
          console.error('Error deleting report:', err);
          alert('שגיאה במחיקת הדוח');
        }
      });
    }
  }

  getEmptyStateMessage(): string {
    if (this.selectedStatus === 'Pending') {
      return 'אין דוחות ממתינים להגשה 🎉';
    } else if (this.selectedStatus === 'overdue') {
      return 'אין דוחות באיחור - מצוין! 👏';
    } else if (this.selectedStatus === 'Reported') {
      return 'אין דוחות שדווחו';
    } else if (this.selectedStatus === 'Paid') {
      return 'אין דוחות ששולמו';
    } else if (this.searchTerm) {
      return `לא נמצאו תוצאות עבור "${this.searchTerm}"`;
    } else {
      return 'לא קיימים דוחות במערכת';
    }
  }
}