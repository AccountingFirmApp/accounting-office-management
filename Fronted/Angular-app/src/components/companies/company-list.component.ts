import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company';
import { CompanyDto } from '../../models/Company';
import { AuthService } from '../../services/auth.service';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { LoadingComponent } from '../../app/components/shared/loading/loading.component';
import { ErrorMessageComponent } from '../../app/components/shared/error-message/error-message.component';
import { ConfirmationModalComponent } from '../../app/components/shared/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [CommonModule, BackButtonComponent, LoadingComponent, ErrorMessageComponent, ConfirmationModalComponent],
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  companies: CompanyDto[] | null = null;
  loading = false;
  error: string | null = null;
  successMessage: string | null = null;

  showDeleteConfirmation = false;
  companyToDelete: number | null = null;

  constructor(
    private companyService: CompanyService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private location: Location,
    public auth: AuthService
  ) { }

  ngOnInit(): void {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.loading = true;
    this.error = null;
    this.cdr.detectChanges();

    this.companyService.getAllCompanies().subscribe({
      next: (data) => {
        this.companies = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'שגיאה בטעינת החברות';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  viewCompanyTasks(id: number): void {
    this.router.navigate(['/companies', id, 'tasks']);
  }

  viewCompanyReports(companyId: number): void {
    const queryParams: { companyId: number; adminMode?: string } = { companyId };
    if (this.auth.isAdmin()) {
      queryParams.adminMode = 'true';
    }
    this.router.navigate(['/reports'], { queryParams: {returnUrl: '/companies'} });
  }

  editCompany(id: number): void {
    this.router.navigate(['/companies', id, 'edit']);
  }

  deleteCompany(id: number): void {
    this.companyToDelete = id;
    this.showDeleteConfirmation = true;
  }

  confirmDelete(): void {
    if (this.companyToDelete !== null) {
      this.companyService.deleteCompany(this.companyToDelete).subscribe({
        next: () => {
          this.showDeleteConfirmation = false;
          this.companyToDelete = null;
          this.loadCompanies();
        },
        error: () => {
          this.error = 'שגיאה במחיקת החברה';
          this.showDeleteConfirmation = false;
        }
      });
    }
  }

  cancelDelete(): void {
    this.showDeleteConfirmation = false;
    this.companyToDelete = null;
  }

  addCompany(): void {
    this.router.navigate(['/companies/create']);
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }
}