import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CompanyService } from '../../services/company';

@Component({
  selector: 'app-company-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './company-create.html',
  styleUrls: ['./company-create.css']
})
export class CompanyCreateComponent implements OnInit {
  companyForm: FormGroup;
  isEditMode = false;
  companyId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required],
      taxId: ['', Validators.required],
      firmId: [1, Validators.required],
      phone: [''],
      email: ['', Validators.email],
      address: ['']
    });
  }

  ngOnInit(): void {
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
}