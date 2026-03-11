import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService } from '../../services/report';
import { CreateReportInstance, UpdateReportInstance } from '../../models/report-instance';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { LoadingComponent } from '../../app/components/shared/loading/loading.component';
import { ErrorMessageComponent } from '../../app/components/shared/error-message/error-message.component';
import { CompanyDto } from '../../models/company.dto';
import { ReportTypeDto } from '../../models/report-type.dto';

interface ReportFormData {
  id?: number;
  companyId: string | number;
  reportTypeId: string | number;
  configId: string | number;
  frequencyId?: string | number;
  period: Date;
  amount: number | null | undefined;
  status: string;
  paymentMethod: string;
  receiptDate: Date | null;
  reportedDate: Date | null;
  paidDate: Date | null;
  comments: string;
}

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, BackButtonComponent, LoadingComponent, ErrorMessageComponent],
  selector: 'app-report-form',
  templateUrl: './report-form.html',
  styleUrls: ['./report-form.css']
})
export class ReportFormComponent implements OnInit {

  isEditMode = false;
  reportId: number | null = null;
  loading = false;
  submitting = false;

  // Success/Error messages
  successMessage: string | null = null;
  errorMessage: string | null = null;

  // Form data
  formData: ReportFormData = {
    companyId: '',
    reportTypeId: '',
    configId: '',
    period: new Date(),
    amount: null,
    status: 'Pending',
    paymentMethod: '',
    receiptDate: null,
    reportedDate: null,
    paidDate: null,
    comments: ''
  };

  // Date strings for input binding
  periodString = '';
  receiptDateString = '';
  reportedDateString = '';
  paidDateString = '';

  companies: any[] = [];
  reportTypes: any[] = [];
  availableReportTypes: any[] = [];
  configs: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reportService: ReportService
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    
    // טען את כל הנתונים במקביל
    Promise.all([
      this.loadCompanies(),
      this.loadReportTypes(),
      this.loadConfigs()
    ]).then(() => {
     
      this.route.params.subscribe(params => {
        if (params['id']) {
          this.isEditMode = true;
          this.reportId = +params['id'];
          this.loadReport();
        } else {
          this.initNewReport();
          this.loading = false;
        }
      });
    }).catch(err => {
      this.loading = false;
    });
  }

  loadCompanies(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.reportService.getCompanies().subscribe({
        next: (data) => {
          this.companies = data;
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  loadReportTypes(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.reportService.getReportTypes().subscribe({
        next: (data) => {
          this.reportTypes = data;
          this.availableReportTypes = data; 
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  loadConfigs(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.reportService.getConfigs().subscribe({
        next: (data) => {
          this.configs = data;
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  onCompanyChange() {
    this.formData.reportTypeId = '';
    this.formData.configId = '';
    this.updateConfigId();
  }

  onReportTypeChange() {
    this.updateConfigId();
  }

  updateConfigId() {
    if (this.formData.companyId && this.formData.reportTypeId) {
      const config = this.configs.find(c => 
        c.companyId === +this.formData.companyId && 
        c.reportTypeId === +this.formData.reportTypeId
      );
      
      if (config) {
        this.formData.configId = config.id;
      } else {
        this.formData.configId = '';
      }
    } else {
      this.formData.configId = '';
    }
  }

  initNewReport() {
    const now = new Date();
    this.periodString = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`;
    this.formData.period = now;
  }

  loadReport() {
    if (!this.reportId) return;
    
    this.reportService.getById(this.reportId).subscribe({
      next: (report) => {
        const config = this.configs.find(c => c.id === report.configId);
        
        this.formData = {
          id: report.id,
          configId: report.configId,
          companyId: config?.companyId || '',
          reportTypeId: config?.reportTypeId || '',
          period: new Date(report.period),
          amount: report.amount ?? null,
          status: report.status,
          paymentMethod: report.paymentMethod || '',
          receiptDate: report.receiptDate ? new Date(report.receiptDate) : null,
          reportedDate: report.reportedDate ? new Date(report.reportedDate) : null,
          paidDate: report.paidDate ? new Date(report.paidDate) : null,
          comments: report.comments || ''
        };

        this.periodString = this.dateToMonthString(this.formData.period);
        this.receiptDateString = this.dateToDateString(this.formData.receiptDate);
        this.reportedDateString = this.dateToDateString(this.formData.reportedDate);
        this.paidDateString = this.dateToDateString(this.formData.paidDate);

        this.loading = false;
      },
      error: (err) => {
        this.goBack();
      }
    });
  }

  onPeriodChange(value: string) {
    if (value) {
      const [year, month] = value.split('-');
      this.formData.period = new Date(+year, +month - 1, 1);
    }
  }

  onReceiptDateChange(value: string) {
    this.formData.receiptDate = value ? new Date(value) : null;
  }

  onReportedDateChange(value: string) {
    this.formData.reportedDate = value ? new Date(value) : null;
  }

  onPaidDateChange(value: string) {
    this.formData.paidDate = value ? new Date(value) : null;
  }

  dateToMonthString(date: Date | null): string {
    if (!date) return '';
    const d = new Date(date);
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
  }

  dateToDateString(date: Date | null): string {
    if (!date) return '';
    const d = new Date(date);
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
  }

  onSubmit() {
   

    this.submitting = true;

    if (this.isEditMode) {
      this.updateReport();
    } else {
      this.createReport();
    }
  }
createReport() {
  const data: CreateReportInstance = {
    companyId: +this.formData.companyId,        
    reportTypeId: +this.formData.reportTypeId,  
    frequencyId: this.formData.frequencyId ? +this.formData.frequencyId : undefined,
    period: this.formData.period,
    amount: this.formData.amount ?? undefined,
    paymentMethod: this.formData.paymentMethod || undefined,
    receiptDate: this.formData.receiptDate || undefined,
    comments: this.formData.comments || ''
  };

  this.reportService.create(data).subscribe({
    next: () => {
      this.successMessage = 'הדיווח נוצר בהצלחה';
      setTimeout(() => {
        this.goBack();
      }, 1500);
    },
    error: (err) => {
      this.errorMessage = 'שגיאה ביצירת הדיווח';
      this.submitting = false;
    }
  });
}






  updateReport() {
    const data: UpdateReportInstance = {
      id: this.reportId!,
      amount: this.formData.amount || undefined,
      status: this.formData.status,
      paymentMethod: this.formData.paymentMethod || undefined,
      receiptDate: this.formData.receiptDate || undefined,
      reportedDate: this.formData.reportedDate || undefined,
      paidDate: this.formData.paidDate || undefined,
      comments: this.formData.comments || ''
    };

    this.reportService.update(this.reportId!, data).subscribe({
      next: () => {
        this.successMessage = 'הדיווח עודכן בהצלחה';
        setTimeout(() => {
          this.goBack();
        }, 1500);
      },
      error: (err) => {
        this.errorMessage = 'שגיאה בעדכון הדיווח';
        this.submitting = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/reports'], { 
    queryParams: { adminMode: 'true' } 
  });}
  
}