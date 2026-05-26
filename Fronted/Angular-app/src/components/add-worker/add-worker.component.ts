import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { WorkerService } from '../../services/worker';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { CompanyService } from '../../services/company';

@Component({
  selector: 'app-worker-form',
  templateUrl: './add-worker.component.html',
  styleUrls: ['./add-worker.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    CommonModule,
    BackButtonComponent
  ],
})
export class WorkerFormComponent implements OnInit {
  @Input() worker: any = null;
  @Output() saved = new EventEmitter<void>();

  isEditMode = false;
  showPassword = false; 

  roles = [
    { id: 1, name: 'Admin' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Employee' }
  ];

  companies: { id: number; name: string }[] = [];
  selectedCompanyIds: number[] = [];
  workerForm: FormGroup;
  location: any;

  constructor(
    private fb: FormBuilder,
    private workerService: WorkerService,
    private companyService: CompanyService,
    private router: Router,
    private route : ActivatedRoute
  ) {
    this.workerForm = this.fb.group({
      id: [null],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]], 
      roleid: [null], 
      employeeid: [''],
      phone: [''],
      hiredate: [null],
    });
  }

  ngOnInit() {
    this.companyService.getAllCompanies().subscribe(data => {
      this.companies = data;
    });

    const workerId = this.route.snapshot.paramMap.get('id');

    if (workerId) {
      this.isEditMode = true;

      this.workerForm.get('password')?.clearValidators();
      this.workerForm.get('password')?.updateValueAndValidity();

      this.workerForm.get('email')?.clearValidators();
      this.workerForm.get('email')?.updateValueAndValidity();

      this.workerForm.get('firstname')?.clearValidators();
      this.workerForm.get('firstname')?.updateValueAndValidity();

      this.workerForm.get('lastname')?.clearValidators();
      this.workerForm.get('lastname')?.updateValueAndValidity();

      this.workerForm.get('firstname')?.disable();
      this.workerForm.get('lastname')?.disable();
      this.workerForm.get('email')?.disable();
      this.workerForm.get('password')?.disable();
      this.workerForm.get('phone')?.disable();

      this.workerService.getWorkerById(+workerId!).subscribe(worker => {
          console.log('companyIds שהגיעו מהשרת:', worker.companyIds);

        let hireDateValue: string | null = null;
        if (worker.hireDate) {
          try {
            const dateObj = new Date(worker.hireDate);
            const year = dateObj.getFullYear();
            const month = String(dateObj.getMonth() + 1).padStart(2, '0');
            const day = String(dateObj.getDate()).padStart(2, '0');
            hireDateValue = `${year}-${month}-${day}`;

          } catch (e) {
          }
        }

        this.workerForm.patchValue({
          id: worker.id,
          firstname: worker.firstName,
          lastname: worker.lastName,
          email: worker.email,
          roleid: worker.roleId,
          employeeid: worker.employeeid,
          phone: worker.phone,
          hiredate: hireDateValue,
          isactive: worker.isactive
        });
        if (worker.companyIds && Array.isArray(worker.companyIds)) {
          this.selectedCompanyIds = [...worker.companyIds]; 

        }
      });
    }
  }

  isCompanySelected(companyId: number): boolean {
    return this.selectedCompanyIds.includes(companyId);
  }

  trackById(index: number, item: any) {
    return item.id;
  }

  onCompanyToggle(companyId: number, event: any) {
    if (event.target.checked) {
      if (!this.selectedCompanyIds.includes(companyId)) {
        this.selectedCompanyIds.push(companyId);
      }
    } else {
      const index = this.selectedCompanyIds.indexOf(companyId);
      if (index > -1) {
        this.selectedCompanyIds.splice(index, 1);
      }
    }

  }
togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }


showRestoreDialog = false;
pendingPayload: any = null;

onSubmit() {
  if (this.workerForm.invalid) return;

  const formValue = this.isEditMode
    ? this.workerForm.getRawValue()
    : this.workerForm.value;

  const payload: any = {};

  if (this.isEditMode) {
    payload.id = formValue.id;
    if (formValue.roleid) payload.roleid = formValue.roleid;
    if (formValue.employeeid) payload.employeeid = formValue.employeeid;
    if (formValue.hiredate) payload.hiredate = formValue.hiredate;
  } else {
    payload.roleid = formValue.roleid;
    payload.firstname = formValue.firstname;
    payload.lastname = formValue.lastname;
    payload.email = formValue.email;
    if (formValue.password) payload.password = formValue.password;
    if (formValue.employeeid) payload.employeeid = formValue.employeeid;
    if (formValue.phone) payload.phone = formValue.phone;
    if (formValue.hiredate) payload.hiredate = formValue.hiredate;
  }

  if (this.selectedCompanyIds.length > 0)
    payload.companyIds = this.selectedCompanyIds;

  if (!this.isEditMode) {
    // בדיקה אם עובד קיים ולא פעיל
    this.workerService.getInactiveWorkers().subscribe({
      next: (inactiveWorkers) => {
        const existing = inactiveWorkers.find(w => w.email === formValue.email);
        if (existing) {
          this.showRestoreDialog = true;
          this.pendingPayload = payload;
        } else {
          this.submitCreate({ ...payload, restoreExistingData: false });
        }
      },
      error: () => this.submitCreate({ ...payload, restoreExistingData: false })
    });
  } else {
    this.workerService.updateWorker(formValue.id!, payload).subscribe({
      next: () => this.router.navigate(['/workers']),
      error: () => {}
    });
  }
}

confirmRestore(restore: boolean): void {
  this.showRestoreDialog = false;
  this.submitCreate({ ...this.pendingPayload, restoreExistingData: restore });
}

private submitCreate(payload: any): void {
  this.workerService.addWorker(payload).subscribe({
    next: () => this.router.navigate(['/workers']),
    error: () => {}
  });
}
  goBack(): void {
    this.router.navigate(['/workers']);
  }
}