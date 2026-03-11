import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyReportConfigService } from '../../../../services/company-report-config.service';
import { CompanyReportConfigDto } from '../../../../models/company-report-config';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { LoadingComponent } from '../../shared/loading/loading.component';
import { ErrorMessageComponent } from '../../shared/error-message/error-message.component';
import { CompanyService } from '../../../../services/company';
import { CompanyDto } from '../../../../models/company.dto';
import { ReportTypeDto } from '../../../../models/report-type.dto';
import { ReportService } from '../../../../services/report';
import { Observable } from 'rxjs';
import { AuthService } from '../../../../services/auth.service';
import { WorkerService } from '../../../../services/worker';
import { log } from 'console';

@Component({
  selector: 'app-company-report-config-list',
  standalone: true,
  imports: [CommonModule, BackButtonComponent, LoadingComponent, ErrorMessageComponent],
  templateUrl: './company-report-config-list.component.html',
  styleUrls: ['./company-report-config-list.component.css']
})
export class CompanyReportConfigListComponent implements OnInit {
  @Input() mode: 'admin' | 'employee' = 'employee';
  configs: CompanyReportConfigDto[] = []; //  רק פעילות
  deletedConfigs: CompanyReportConfigDto[] = []; //  רק מחוקות
  companies: CompanyDto[] = [];
  reportTypes: ReportTypeDto[] = [];

  // מטריצה: companyId -> reportTypeId -> config פעיל
  configMatrix: { [companyId: number]: { [reportTypeId: number]: CompanyReportConfigDto | null } } = {};

  //  מטריצה למחוקות: companyId -> reportTypeId -> config מחוק
  deletedMatrix: { [companyId: number]: { [reportTypeId: number]: CompanyReportConfigDto | null } } = {};

  loading = false;
  selectedYear: number;
  currentYear: number;

  // Success/Error messages
  successMessage: string | null = null;
  errorMessage: string | null = null;

  // Modal להצגת פרטי דיווח
  isModalOpen: boolean = false;
  selectedConfig: CompanyReportConfigDto | null = null;
 isAdmin: boolean=false;
   returnUrl: string = '/home'; // ברירת מחדל

  constructor(
    private configService: CompanyReportConfigService,
    private router: Router,
    private companySer: CompanyService,
    private reportService: ReportService,
     public auth: AuthService,
     public workerService: WorkerService,
     private route: ActivatedRoute
  ) {
    this.currentYear = new Date().getFullYear();
    this.selectedYear = this.currentYear + 1;
    this.isAdmin = this.auth.isAdmin();

  }
  ngOnInit(): void {
    this.mode = this.route.snapshot.data['mode'] ?? 'employee'; // ← קודם
    
    this.route.queryParams.subscribe(params => {
        this.returnUrl = params['returnUrl'] || '/home';
        this.loadAll();
    });
}
  loadAll(): void {
    this.loading = true;
    Promise.all([
      this.loadCompanies(),
      this.loadReportTypes(),
      this.loadAllConfigs()
    ]).then(() => {
      this.buildMatrix();
      this.buildDeletedMatrix(); 
      this.loading = false;
    }).catch(err => {
      this.loading = false;
    });
  }

  /**
   * בונה מטריצה של חברות X סוגי דיווחים לשנה הנבחרת - רק פעילות 
   */
  buildMatrix(): void {
    this.configMatrix = {};
    
    this.companies.forEach(company => {
      this.configMatrix[company.id] = {};

      this.reportTypes.forEach(reportType => {
        const config = this.configs.find(c =>
          c.companyId === company.id &&
          c.reportTypeId === reportType.id &&
          c.year === this.selectedYear &&
          c.isactive === true 
        );

        this.configMatrix[company.id][reportType.id] = config || null;
      });
    });
  }

  /**
   *  בונה מטריצה של הגדרות מחוקות
   */
  buildDeletedMatrix(): void {
    this.deletedMatrix = {};
    
    this.companies.forEach(company => {
      this.deletedMatrix[company.id] = {};

      this.reportTypes.forEach(reportType => {
        // מחפש הגדרה מחוקה 
        const config = this.deletedConfigs.find(c =>
          c.companyId === company.id &&
          c.reportTypeId === reportType.id &&
          c.year === this.selectedYear &&
          c.isactive === false 
        );

        this.deletedMatrix[company.id][reportType.id] = config || null;
      });
    });
  }

  loadCompanies(): Promise<void> {
    if(this.mode === 'admin') {
    return new Promise((resolve, reject) => {
      this.companySer.getAllCompanies().subscribe({
        next: (data) => {
          this.companies = data;
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
    } else {
          return new Promise((resolve, reject) => {
      this.companySer.getCompanyByWorkerId(this.workerService.currentWorker.id).subscribe({
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
  }

  loadReportTypes(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.reportService.getReportTypes().subscribe({
        next: (data) => {
          this.reportTypes = data;
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  loadAllConfigs(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.configService.getAll().subscribe({
        next: (data) => {
          this.configs = data.filter(c => c.isactive === true);
          this.deletedConfigs = data.filter(c => c.isactive === false);
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  navigateToCreate(): void {
    
this.router.navigate(['/settings/report-config/create'], { queryParams: { isAdmin: this.mode === 'admin' } });
  }
  
  deleteConfig(configId: number): void {
    this.configService.delete(configId).subscribe({
      next: () => {
        this.successMessage = 'ההגדרה נמחקה בהצלחה';
        this.loadAll(); 
        setTimeout(() => {
          this.successMessage = null;
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = 'שגיאה במחיקת ההגדרה';
      }
    });
  }


  restoreConfig(configId: number, companyName: string, reportTypeName: string): void {
    const updateDto = {
      isactive: true
    };

    this.configService.update(configId, updateDto).subscribe({
      next: () => {
        this.successMessage = 'ההגדרה שוחזרה בהצלחה';
        this.loadAll(); 
        setTimeout(() => {
          this.successMessage = null;
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = 'שגיאה בשחזור ההגדרה';
      }
    });
  }

 
  hasDeletedConfig(companyId: number, reportTypeId: number): boolean {
    return !!(this.deletedMatrix[companyId] && this.deletedMatrix[companyId][reportTypeId]);
  }


  getDeletedConfig(companyId: number, reportTypeId: number): CompanyReportConfigDto | null {
    if (this.deletedMatrix[companyId] && this.deletedMatrix[companyId][reportTypeId]) {
      return this.deletedMatrix[companyId][reportTypeId];
    }
    return null;
  }

  copyFromLastYear(companyId: number, companyName: string): void {
    const previousYear = this.selectedYear - 1;

    this.configService.getByCompanyId(companyId).subscribe({
      next: (configs) => {
        const previousYearConfigs = configs.filter(c => 
          c.year === previousYear && c.isactive === true
        );

        if (previousYearConfigs.length === 0) {
          this.router.navigate(['/settings/report-config/create'], {
            queryParams: {
              companyId: companyId,
              year: this.selectedYear,
              fixedYear: true,
              isAdmin: this.mode === 'admin'
            }
          });
          return;
        }

        const configNames = previousYearConfigs
          .map(c => `${c.reportTypeName} (${c.frequencyName})`)
          .join('\n- ');


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
              if (successCount + errorCount === totalConfigs) {
                this.handleCopyComplete(successCount, errorCount, companyName, this.selectedYear);
              }
            }
          });
        });
      },
      error: (err) => {
       
      }
    });
  }

  private handleCopyComplete(successCount: number, errorCount: number, companyName: string, year: number): void {
    if (errorCount === 0) {
    } else {
    }
    this.loadAll();
  }

  addConfig(companyId: number, reportTypeId: number): void {
    this.router.navigate(['/settings/report-config/create'], {
      queryParams: {
        companyId: companyId,
        reportTypeId: reportTypeId,
        year: this.selectedYear,
        fixedYear: true,
        isAdmin: this.mode === 'admin'
      }
    });
  }

  editConfig(configId: number): void {
    this.router.navigate([`/settings/report-config/${configId}/edit`], {
      queryParams: {
        isAdmin: this.mode === 'admin'
      }
    });
  }

  viewConfig(config: CompanyReportConfigDto): void {
    this.selectedConfig = config;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedConfig = null;
  }

  previousYear(): void {
    this.selectedYear--;
    this.buildMatrix();
    this.buildDeletedMatrix();
  }

  nextYear(): void {
    this.selectedYear++;
    this.buildMatrix();
    this.buildDeletedMatrix(); 
  }

  isReadOnly(): boolean {
    return this.selectedYear < this.currentYear;
  }
}