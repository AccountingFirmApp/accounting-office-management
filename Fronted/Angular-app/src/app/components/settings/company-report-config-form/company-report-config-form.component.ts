import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CompanyReportConfigService } from '../../../../services/company-report-config.service';
import { CompanyService } from '../../../../services/company';
import { ReportService } from '../../../../services/report';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-company-report-config-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BackButtonComponent],
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

  companies: any[] = [];
  reportTypes: any[] = [];
  frequencies: any[] = [];
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
      frequencyId: ['', Validators.required],
      dayOfMonth: ['', [Validators.min(1), Validators.max(31)]],
      year: [new Date().getFullYear(), Validators.required]
    });
  }

  ngOnInit(): void {
    this.initializeYears();
    this.loadData();

    // בדיקה אם זה מצב עריכה
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.configId = +params['id'];
      }
    });

    // בדיקה אם יש query parameters (מגיעים מכפתור "הוסף")
    this.route.queryParams.subscribe(queryParams => {
      if (queryParams['fixedYear'] === 'true') {
        this.isFixedYear = true;

        // ממלא את השדות מראש
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
        console.error(err);
        alert('שגיאה בטעינת ההגדרה');
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
          alert(this.isEditMode ? 'ההגדרה עודכנה בהצלחה' : 'ההגדרה נוצרה בהצלחה');
          this.router.navigate(['/settings/report-config']);
        },
        error: (err: any) => {
          console.error(err);
          alert('שגיאה בשמירה: ' + (err.error?.message || 'שגיאה לא ידועה'));
          this.submitting = false;
        }
      });
    } else {
      alert('נא למלא את כל השדות החובה');
    }
  }

  cancel(): void {
    this.router.navigate(['/settings/report-config']);
  }

 
  deleteConfig(): void {
    if (!this.configId) return;

    const formData = this.configForm.getRawValue();
    const companyName = this.companies.find(c => c.id === formData.companyId)?.name || 'החברה';
    const reportTypeName = this.reportTypes.find(rt => rt.id === formData.reportTypeId)?.name || 'הדיווח';

    const confirmed = confirm(
      `⚠️ האם אתה בטוח שברצונך למחוק את ההגדרה?\n\n` +
      `חברה: ${companyName}\n` +
      `סוג דיווח: ${reportTypeName}\n` +
      `שנה: ${formData.year}\n\n` +
      `פעולה זו לא ניתנת לביטול!`
    );

    if (!confirmed) return;

    this.submitting = true;
    this.configService.delete(this.configId).subscribe({
      next: () => {
        alert('✅ ההגדרה נמחקה בהצלחה');
        this.router.navigate(['/settings/report-config']);
      },
      error: (err) => {
        console.error(err);
        alert('❌ שגיאה במחיקת ההגדרה: ' + (err.error?.message || 'שגיאה לא ידועה'));
        this.submitting = false;
      }
    });
  }
}
