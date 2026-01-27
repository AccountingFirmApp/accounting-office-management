import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CompanyService } from '../../services/company';
import { AccountingFirmService } from '../../services/accounting-firm.ts.service'
import { BackButtonComponent } from "../../app/components/shared/back-button/back-button.component";
@Component({
  selector: 'app-company-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BackButtonComponent],
  templateUrl: './company-create.html',
  styleUrls: ['./company-create.css']
})
export class CompanyCreateComponent implements OnInit {
  companyForm: FormGroup;
  isEditMode = false;
  companyId: number | null = null;
  loading = false;
  accountingFirms: any[] = [];
  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private accountingFirmService: AccountingFirmService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required],
      taxId: ['', Validators.required],
      firmId: [null, Validators.required],
      phone: [''],
      email: ['', Validators.email],
      address: ['']
    });
  }

  ngOnInit(): void {
      // טען את רשימת המשרדים
      this.loadAccountingFirms();
    
      // בדוק אם זה מצב עריכה
      
    // בדוק אם זה מצב עריכה
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.companyId = +params['id'];
        this.loadCompany();
      }
    });
  }

  loadCompany(): void {
    if (this.companyId) {
      this.companyService.getCompanyById(this.companyId).subscribe({
        next: (data) => {
          this.companyForm.patchValue({
            name: data.name,
            taxId: data.taxId,
            firmId: data.firmId,
            phone: data.phone,
            email: data.email,
            address: data.address
          });
        },
        error: (err) => {
          alert('שגיאה בטעינת החברה');
          console.error(err);
        }
      });
    }
  }

  onSubmit(): void {
    if (this.companyForm.valid) {
      this.loading = true;

      if (this.isEditMode && this.companyId) {
        // עדכון חברה קיימת
        const updateCommand = {
          id: this.companyId,
          ...this.companyForm.value
        };
        
        this.companyService.updateCompany(this.companyId, updateCommand).subscribe({
          next: () => {
            alert('החברה עודכנה בהצלחה');
            this.router.navigate(['/companies']);
          },
          error: (err) => {
            alert('שגיאה בעדכון החברה');
            console.error(err);
            this.loading = false;
          }
        });
      } else {
        // יצירת חברה חדשה
        this.companyService.createCompany(this.companyForm.value).subscribe({
          next: () => {
            alert('החברה נוצרה בהצלחה');
            this.router.navigate(['/companies']);
          },
          error: (err) => {
            alert('שגיאה ביצירת החברה');
            console.error(err);
            this.loading = false;
          }
        });
      }
    } else {
      alert('נא למלא את כל השדות החובה');
    }
  }

  cancel(): void {
    this.router.navigate(['/companies']);
  }
  loadAccountingFirms(): void {
    this.accountingFirmService.getAll().subscribe({
      next: (data) => {
        this.accountingFirms = data;
      },
      error: (err) => {
        console.error(err);
        alert('שגיאה בטעינת רשימת המשרדים');
      }
    });
  }
}