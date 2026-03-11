import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CompanyReportConfigService } from '../../../../services/company-report-config.service';
import { CompanyService } from '../../../../services/company';
import { ReportService } from '../../../../services/report';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { LoadingComponent } from '../../shared/loading/loading.component';
import { ErrorMessageComponent } from '../../shared/error-message/error-message.component';
import { ConfirmationModalComponent } from '../../shared/confirmation-modal/confirmation-modal.component';
import { Observable } from 'rxjs';
import { CompanyDto } from '../../../../models/company.dto';
import { ReportTypeDto, FrequencyDto } from '../../../../models/report-type.dto';

@Component({
  selector: 'app-company-report-config-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BackButtonComponent, LoadingComponent, ErrorMessageComponent, ConfirmationModalComponent],
  templateUrl: './company-report-config-form.component.html',
  styleUrls: ['./company-report-config-form.component.css']
})
export class CompanyReportConfigFormComponent implements OnInit {
  configForm: FormGroup;
  isEditMode = false;
  configId: number | null = null;
  loading = false;
  submitting = false;
  isFixedYear = false; // האם השנה קבועה (לא ניתנת לשינוי)
  successMessage: string | null = null;
  errorMessage: string | null = null;

  // Confirmation modal state
  showDeleteConfirmation = false;

  companies: CompanyDto[] = [];
  reportTypes: ReportTypeDto[] = [];
  frequencies: FrequencyDto[] = [];
  years: number[] = [];

  constructor(
    private fb: FormBuilder,
    private configService: CompanyReportConfigService,
    private companyService: CompanyService,
    private reportService: ReportService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.configForm = this.fb.group({
      companyId: ['', Validators.required],
      reportTypeId: ['', Validators.required],
      frequencyId: [1, Validators.required],
      dayOfMonth: [1, [Validators.min(1), Validators.max(31)]],
      year: [new Date().getFullYear(), Validators.required]
    });
  }
isAdmin: boolean =false;
  ngOnInit(): void {
    this.initializeYears();
    this.loadData();

  this.isAdmin = this.route.snapshot.queryParamMap.get('isAdmin') === 'true'? true : false;
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.configId = +params['id'];
      }
    });

    this.route.queryParams.subscribe(queryParams => {
      if (queryParams['fixedYear'] === 'true') {
        this.isFixedYear = true;

        const companyId = queryParams['companyId'];
        const reportTypeId = queryParams['reportTypeId'];
        const year = +queryParams['year'];

        this.configForm.patchValue({
          companyId: companyId ? +companyId : '',
          reportTypeId: reportTypeId ? +reportTypeId : '',
          year: year || new Date().getFullYear() + 1
        });

        // הופך את השדות לקבועים (לא ניתנים לעריכה)
        this.configForm.get('companyId')?.disable();
        this.configForm.get('reportTypeId')?.disable();
        this.configForm.get('year')?.disable();
      }
    });
  }

  initializeYears(): void {
    const currentYear = new Date().getFullYear();
    for (let i = 0; i < 5; i++) {
      this.years.push(currentYear + i);
    }
  }

  loadData(): void {
    this.loading = true;
    Promise.all([
      this.loadCompanies(),
      this.loadReportTypes(),
      this.loadFrequencies()
    ]).then(() => {
      this.loading = false;
      if (this.isEditMode) {
        this.loadConfig();
      }
    }).catch(err => {
    
      this.loading = false;
    });
  }

  loadCompanies(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.companyService.getAllCompanies().subscribe({
        next: (data) => {
          this.companies = data;
          resolve();
        },
        error: reject
      });
    });
  }

  loadReportTypes(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.reportService.getReportTypes().subscribe({
        next: (data) => {
          this.reportTypes = data;
          resolve();
        },
        error: reject
      });
    });
  }

  loadFrequencies(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.configService.getFrequencies().subscribe({
        next: (data) => {
          this.frequencies = data;
          resolve();
        },
        error: reject
      });
    });
  }

  loadConfig(): void {
    if (!this.configId) return;
    this.configService.getById(this.configId).subscribe({
      next: (data) => {
        this.configForm.patchValue({
          companyId: data.companyId,
          reportTypeId: data.reportTypeId,
          frequencyId: data.frequencyId,
          dayOfMonth: data.dayOfMonth,
          year: data.year
        });

        // במצב עריכה - השבתת שדות שלא ניתנים לשינוי
        this.configForm.get('companyId')?.disable();
        this.configForm.get('reportTypeId')?.disable();
        this.configForm.get('year')?.disable();
      },
      error: (err) => {
      }
    });
  }

  onSubmit(): void {
    if (this.configForm.valid || this.isFixedYear || this.isEditMode) {
      this.submitting = true;

      const formData = (this.isFixedYear || this.isEditMode)
        ? this.configForm.getRawValue() 
        : this.configForm.value;

      let request$: Observable<any>;

      request$ =
        this.isEditMode && this.configId
          ? this.configService.update(this.configId, { id: this.configId, ...formData })
          : this.configService.create(formData);

      request$.subscribe({
        next: () => {
          this.successMessage = this.isEditMode ? 'ההגדרה עודכנה בהצלחה' : 'ההגדרה נוצרה בהצלחה';
          setTimeout(() => {
            this.router.navigate([this.isAdmin ? '/settings/report-config' : '/dashboard/report-config']);
          }, 1500);
        },
        error: (err: any) => {
          this.errorMessage = 'שגיאה בשמירת ההגדרה';
          this.submitting = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate([this.isAdmin ? '/settings/report-config' : '/dashboard/report-config']);
  }

  /**
   * פותח modal אישור למחיקת תצורה
   */
  openDeleteConfirmation(): void {
    this.showDeleteConfirmation = true;
  }

  /**
   * מבטל מחיקה וסגר modal
   */
  cancelDelete(): void {
    this.showDeleteConfirmation = false;
  }

  /**
   * מאשר ומבצע מחיקת תצורה
   */
  confirmDelete(): void {
    if (!this.configId) return;

    this.showDeleteConfirmation = false;
    this.submitting = true;

    this.configService.delete(this.configId).subscribe({
      next: () => {
        this.successMessage = 'ההגדרה נמחקה בהצלחה';
        setTimeout(() => {
         this.router.navigate([this.isAdmin ? '/settings/report-config' : '/dashboard/report-config']);
        }, 1500);
      },
      error: (err) => {
        this.errorMessage = 'שגיאה במחיקת ההגדרה';
        this.submitting = false;
      }
    });
  }
}
