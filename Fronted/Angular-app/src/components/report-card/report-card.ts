import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ReportInstanceDetail } from '../../models/report-instance';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-report-card',
  templateUrl: './report-card.html',
  styleUrls: ['./report-card.css']
})
export class ReportCardComponent {
  
  @Input() report!: ReportInstanceDetail;
  @Output() view = new EventEmitter<number>();
  @Output() edit = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();

  get isOverdue(): boolean {
    if (!this.report.period) return false;
    const today = new Date();
    const period = new Date(this.report.period);
    return (this.report.status === 'Pending' || this.report.status === 'Reported') && period < today;
  }

  formatPeriod(): string {
    if (!this.report.period) return '-';
    const d = new Date(this.report.period);
    return d.toLocaleDateString('he-IL', { year: 'numeric', month: 'long' });
  }

  formatAmount(): string {
    if (!this.report.amount) return '-';
    return new Intl.NumberFormat('he-IL', {
      style: 'currency',
      currency: 'ILS'
    }).format(this.report.amount);
  }

  getStatusClass(): string {
    const statusMap: Record<string, string> = {
      'Pending': 'pending',
      'Reported': 'reported',
      'Paid': 'paid',
      'Approved': 'approved',
      'NotRequired': 'not-required'
    };
    return statusMap[this.report.status] || '';
  }

  getStatusLabel(): string {
    const labels: Record<string, string> = {
      'Pending': 'ממתין',
      'Reported': 'דווח',
      'Paid': 'שולם',
      'Approved': 'אושר',
      'NotRequired': 'לא נדרש'
    };
    return labels[this.report.status] || this.report.status;
  }

  getPaymentMethodLabel(): string {
    if (!this.report.paymentMethod) return '-';
    const labels: Record<string, string> = {
      'Credit': 'כרטיס אשראי',
      'Transfer': 'העברה בנקאית',
      'Check': 'צ\'ק',
      'Online': 'תשלום מקוון',
      'Cash': 'מזומן'
    };
    return labels[this.report.paymentMethod] || this.report.paymentMethod;
  }

  getDaysOverdue(): number {
    if (!this.isOverdue) return 0;
    const today = new Date();
    const period = new Date(this.report.period);
    const diffTime = Math.abs(today.getTime() - period.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  onView() {
    this.view.emit(this.report.id);
    console.log('View report:', this.report.id);
  }

  onEdit() {
    this.edit.emit(this.report.id);
  }

  onDelete() {
    this.delete.emit(this.report.id);
  }
}