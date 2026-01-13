import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportInstanceDetail } from '../../models/report-instance';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-report-view-modal',
  templateUrl: './report-view.html',
  styleUrl:'./report-view.css'
 })  
export class ReportViewModalComponent {
  @Input() report: ReportInstanceDetail | null = null;
  @Input() isOpen = false;
  @Output() closeModal = new EventEmitter<void>();
  @Output() editClicked = new EventEmitter<number>();

  constructor(private router: Router) {}

  close() {
    this.closeModal.emit();
  }

  editReport() {
    if (this.report) {
      this.editClicked.emit(this.report.id);
      this.close();
    }
  }

  isOverdue(): boolean {
    if (!this.report || !this.report.period) return false;
    const today = new Date();
    const period = new Date(this.report.period);
    return (this.report.status === 'Pending' || this.report.status === 'Reported') && period < today;
  }

  getDaysOverdue(): number {
    if (!this.isOverdue() || !this.report) return 0;
    const today = new Date();
    const period = new Date(this.report.period);
    const diffTime = Math.abs(today.getTime() - period.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  formatPeriod(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    return d.toLocaleDateString('he-IL', { year: 'numeric', month: 'long' });
  }

  formatDate(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    return d.toLocaleDateString('he-IL');
  }

  formatDateTime(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    return d.toLocaleDateString('he-IL') + ' ' + d.toLocaleTimeString('he-IL', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
  }

  formatAmount(amount: number): string {
    return new Intl.NumberFormat('he-IL', {
      style: 'currency',
      currency: 'ILS'
    }).format(amount);
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
}