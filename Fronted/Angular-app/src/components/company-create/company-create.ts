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

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required],
      taxId: ['', Validators.required],
      phone: [''],
      email: ['', Validators.email],
      address: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
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
            phone: data.phone,
            email: data.email,
            address: data.address,
            notes: data.notes
          });
          
          // ✅ נעילת שדה taxId בעריכה (כדי למנוע שינויים)
          if (this.isEditMode) {
            this.companyForm.get('taxId')?.disable();
          }
        },
        error: (err) => {
        }
      });
    }
  }

  onSubmit(): void {
    if (this.companyForm.valid) {
      this.loading = true;
      
      if (this.isEditMode && this.companyId) {
        const updateCommand: any = {
          id: this.companyId,
          name: this.companyForm.value.name,
          phone: this.companyForm.value.phone,
          email: this.companyForm.value.email,
          address: this.companyForm.value.address,
          notes: this.companyForm.value.notes
        };
        
        this.companyService.updateCompany(this.companyId, updateCommand).subscribe({
          next: () => {
            this.router.navigate(['/companies']);
          },
          error: (err) => {
            this.loading = false;
          }
        });
      } else {
        this.companyService.createCompany(this.companyForm.value).subscribe({
          next: () => {
            this.router.navigate(['/companies']);
          },
          error: (err) => {
            this.loading = false;
          }
        });
      }
    } else {
    }
  }

  cancel(): void {
    this.router.navigate(['/companies']);
  }
}