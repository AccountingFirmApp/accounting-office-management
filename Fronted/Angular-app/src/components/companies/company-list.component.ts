import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule ,Location} from '@angular/common';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company';
import { CompanyDto } from '../../models/Company';
import { AuthService } from '../../services/auth.service';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';

@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [CommonModule, BackButtonComponent],
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  companies: CompanyDto[] | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private companyService: CompanyService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private location:Location,
    public auth: AuthService

  ) { 
  }

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
    const queryParams: any = { companyId: companyId };
  
  if (this.auth.isAdmin()) {
    queryParams.adminMode = 'true';
  }
  
  this.router.navigate(['/reports'], { 
    queryParams: queryParams
  });
}

  editCompany(id: number): void {
    this.router.navigate(['/companies', id, 'edit']);
  }

  deleteCompany(id: number): void {
    if (confirm('האם אתה בטוח שברצונך למחוק חברה זו?')) {
      this.companyService.deleteCompany(id).subscribe({
        next: () => {
          this.loadCompanies();
        },
        error: (err) => {
        }
      });
    }
  }

  addCompany(): void {
    this.router.navigate(['/companies/create']);
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }
}