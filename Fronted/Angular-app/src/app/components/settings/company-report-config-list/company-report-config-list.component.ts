import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CompanyReportConfigService } from '../../../../services/company-report-config.service';
import { CompanyReportConfigDto } from '../../../../models/company-report-config';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { CompanyService } from '../../../../services/company';
import { CompanyDto } from '../../../../models/Company';
import { ReportService } from '../../../../services/report';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-company-report-config-list',
  standalone: true,
  imports: [CommonModule, BackButtonComponent],
  templateUrl: './company-report-config-list.component.html',
  styleUrls: ['./company-report-config-list.component.css']
})
export class CompanyReportConfigListComponent implements OnInit {
  configs: CompanyReportConfigDto[] = []; // ⭐ רק פעילות
  deletedConfigs: CompanyReportConfigDto[] = []; // ⭐ רק מחוקות
  companies: CompanyDto[] = [];
  reportTypes: any[] = [];
  
  // מטריצה: companyId -> reportTypeId -> config פעיל
  configMatrix: { [companyId: number]: { [reportTypeId: number]: CompanyReportConfigDto | null } } = {};
  
  // ⭐ מטריצה למחוקות: companyId -> reportTypeId -> config מחוק
  deletedMatrix: { [companyId: number]: { [reportTypeId: number]: CompanyReportConfigDto | null } } = {};
  
  loading = false;
  selectedYear: number;
  currentYear: number;

  constructor(
    private configService: CompanyReportConfigService,
    private router: Router,
    private companySer: CompanyService,
    private reportService: ReportService
  ) {
    this.currentYear = new Date().getFullYear();
    this.selectedYear = this.currentYear + 1;
  }

  ngOnInit(): void {
    this.loadAll();
  }

  /**
   * טוען את כל הנתונים: חברות, סוגי דיווחים, והגדרות
   */
  loadAll(): void {
    this.loading = true;
    Promise.all([
      this.loadCompanies(),
      this.loadReportTypes(),
      this.loadAllConfigs()
    ]).then(() => {
      this.buildMatrix();
      this.buildDeletedMatrix(); // ⭐
      this.loading = false;
    }).catch(err => {
      console.error(err);
      alert('שגיאה בטעינת הנתונים');
      this.loading = false;
    });
  }

  /**
   * בונה מטריצה של חברות X סוגי דיווחים לשנה הנבחרת - רק פעילות ⭐
   */
  buildMatrix(): void {
    this.configMatrix = {};
    
    this.companies.forEach(company => {
      this.configMatrix[company.id] = {};

      this.reportTypes.forEach(reportType => {
        // מחפש הגדרה פעילה ⭐
        const config = this.configs.find(c =>
          c.companyId === company.id &&
          c.reportTypeId === reportType.id &&
          c.year === this.selectedYear &&
          c.isActive === true // ⭐ רק פעילות
        );

        this.configMatrix[company.id][reportType.id] = config || null;
      });
    });
  }

  /**
   * ⭐ בונה מטריצה של הגדרות מחוקות
   */
  buildDeletedMatrix(): void {
    this.deletedMatrix = {};
    
    this.companies.forEach(company => {
      this.deletedMatrix[company.id] = {};

      this.reportTypes.forEach(reportType => {
        // מחפש הגדרה מחוקה ⭐
        const config = this.deletedConfigs.find(c =>
          c.companyId === company.id &&
          c.reportTypeId === reportType.id &&
          c.year === this.selectedYear &&
          c.isActive === false // ⭐ רק מחוקות
        );

        this.deletedMatrix[company.id][reportType.id] = config || null;
      });
    });
  }

  loadCompanies(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.companySer.getAllCompanies().subscribe({
        next: (data) => {
          this.companies = data;
          console.log('חברות:', data);
          resolve();
        },
        error: (err) => {
          console.error(err);
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
          console.log('סוגי דיווחים:', data);
          resolve();
        },
        error: (err) => {
          console.error(err);
          reject(err);
        }
      });
    });
  }

  loadAllConfigs(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.configService.getAll().subscribe({
        next: (data) => {
          // ⭐ הפרד בין פעילות למחוקות
          this.configs = data.filter(c => c.isActive === true);
          this.deletedConfigs = data.filter(c => c.isActive === false);
          
          console.log('הגדרות פעילות:', this.configs);
          console.log('הגדרות מחוקות:', this.deletedConfigs);
          resolve();
        },
        error: (err) => {
          console.error(err);
          reject(err);
        }
      });
    });
  }

  navigateToCreate(): void {
    this.router.navigate(['/settings/report-config/create']);
  }

  /**
   * ⭐ מחיקת הגדרה (שינוי סטטוס ל-false)
   */
  deleteConfig(configId: number): void {
    const confirmed = confirm(
      `האם למחוק את הגדרת ?\n\n(ניתן לשחזר את ההגדרה בכל עת)`
    );

    if (!confirmed) {
      return;
    }

    this.configService.delete(configId).subscribe({
      next: () => {
        alert('✅ ההגדרה נמחקה בהצלחה');
        this.loadAll(); // רענן
      },
      error: (err) => {
        console.error('שגיאה במחיקת הגדרה:', err);
        alert('❌ שגיאה במחיקת ההגדרה');
      }
    });
  }

  /**
   * ⭐ שחזור הגדרה מחוקה (שינוי סטטוס ל-true)
   */
  restoreConfig(configId: number, companyName: string, reportTypeName: string): void {
    const confirmed = confirm(
      `האם לשחזר את הגדרת "${reportTypeName}" עבור "${companyName}"?`
    );

    if (!confirmed) {
      return;
    }

    // עדכן רק את הסטטוס ל-true
    const updateDto = {
      isActive: true
    };

    this.configService.update(configId, updateDto).subscribe({
      next: () => {
        alert('✅ ההגדרה שוחזרה בהצלחה');
        this.loadAll(); // רענן
      },
      error: (err) => {
        console.error('שגיאה בשחזור הגדרה:', err);
        alert('❌ שגיאה בשחזור ההגדרה');
      }
    });
  }

  /**
   * ⭐ בדיקה אם יש הגדרה מחוקה
   */
  hasDeletedConfig(companyId: number, reportTypeId: number): boolean {
    return !!(this.deletedMatrix[companyId] && this.deletedMatrix[companyId][reportTypeId]);
  }

  /**
   * ⭐ קבלת הגדרה מחוקה
   */
  getDeletedConfig(companyId: number, reportTypeId: number): CompanyReportConfigDto | null {
    if (this.deletedMatrix[companyId] && this.deletedMatrix[companyId][reportTypeId]) {
      return this.deletedMatrix[companyId][reportTypeId];
    }
    return null;
  }

  /**
   * מעתיק את כל ההגדרות של חברה מהשנה הקודמת לשנה הנבחרת
   */
  copyFromLastYear(companyId: number, companyName: string): void {
    const previousYear = this.selectedYear - 1;

    this.configService.getByCompanyId(companyId).subscribe({
      next: (configs) => {
        // ⭐ מסנן רק הגדרות פעילות מהשנה הקודמת
        const previousYearConfigs = configs.filter(c => 
          c.year === previousYear && c.isActive === true
        );

        if (previousYearConfigs.length === 0) {
          this.router.navigate(['/settings/report-config/create'], {
            queryParams: {
              companyId: companyId,
              year: this.selectedYear,
              fixedYear: true
            }
          });
          return;
        }

        const configNames = previousYearConfigs
          .map(c => `${c.reportTypeName} (${c.frequencyName})`)
          .join('\n- ');

        const confirmed = confirm(
          `נמצאו ${previousYearConfigs.length} הגדרות לחברה "${companyName}" משנת ${previousYear}:\n\n- ${configNames}\n\nלהעתיק את כל ההגדרות לשנת ${this.selectedYear}?`
        );

        if (!confirmed) {
          return;
        }

        let successCount = 0;
        let errorCount = 0;
        const totalConfigs = previousYearConfigs.length;

        previousYearConfigs.forEach((config) => {
          const newConfig = {
            companyId: config.companyId,
            reportTypeId: config.reportTypeId,
            frequencyId: config.frequencyId,
            dayOfMonth: config.dayOfMonth,
            year: this.selectedYear
          };

          this.configService.create(newConfig).subscribe({
            next: () => {
              successCount++;
              if (successCount + errorCount === totalConfigs) {
                this.handleCopyComplete(successCount, errorCount, companyName, this.selectedYear);
              }
            },
            error: (err) => {
              errorCount++;
              console.error('שגיאה ביצירת הגדרה:', err);
              if (successCount + errorCount === totalConfigs) {
                this.handleCopyComplete(successCount, errorCount, companyName, this.selectedYear);
              }
            }
          });
        });
      },
      error: (err) => {
        console.error(err);
        alert(`שגיאה בטעינת הגדרות החברה "${companyName}"`);
      }
    });
  }

  private handleCopyComplete(successCount: number, errorCount: number, companyName: string, year: number): void {
    if (errorCount === 0) {
      alert(`✅ הצלחה!\nכל ${successCount} ההגדרות הועתקו בהצלחה לשנת ${year} עבור "${companyName}"`);
    } else {
      alert(`⚠️ הושלם עם שגיאות:\n- ${successCount} הגדרות הועתקו בהצלחה\n- ${errorCount} הגדרות נכשלו`);
    }
    this.loadAll();
  }

  addConfig(companyId: number, reportTypeId: number): void {
    this.router.navigate(['/settings/report-config/create'], {
      queryParams: {
        companyId: companyId,
        reportTypeId: reportTypeId,
        year: this.selectedYear,
        fixedYear: true
      }
    });
  }

  editConfig(configId: number): void {
    this.router.navigate([`/settings/report-config/${configId}/edit`]);
  }

  viewConfig(config: CompanyReportConfigDto): void {
    const details = `
📋 פרטי הגדרת דיווח:
━━━━━━━━━━━━━━━━━━━━
🏢 חברה: ${config.companyName}
📊 סוג דיווח: ${config.reportTypeName} (${config.reportTypeShortCode})
🔄 תדירות: ${config.frequencyName}
📅 יום בחודש: ${config.dayOfMonth || 'לא מוגדר'}
📆 שנה: ${config.year}
✅ סטטוס: ${config.isActive ? 'פעיל' : 'לא פעיל'}
    `.trim();

    alert(details);
  }

  previousYear(): void {
    this.selectedYear--;
    this.buildMatrix();
    this.buildDeletedMatrix(); // ⭐
  }

  nextYear(): void {
    this.selectedYear++;
    this.buildMatrix();
    this.buildDeletedMatrix(); // ⭐
  }

  isReadOnly(): boolean {
    return this.selectedYear < this.currentYear;
  }
}