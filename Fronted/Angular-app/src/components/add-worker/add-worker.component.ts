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
  showPassword = false; // ✅ הוסף את זה!

  roles = [
    { id: 1, name: 'Admin' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Employee' }
  ];

  companies: { id: number; name: string }[] = [];
  selectedCompanyIds: number[] = []; // ✅ רשימת החברות שנבחרו
  workerForm: FormGroup;
  location: any;

  constructor(
    private fb: FormBuilder,
    private workerService: WorkerService,
    private companyService: CompanyService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.workerForm = this.fb.group({
      id: [null],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]], // ✅ חובה רק בהוספה
      roleid: [null], // ✅ אופציונלי במצב עריכה
      employeeid: [''],
      phone: [''],
      hiredate: [null],
      isactive: [true]
    });
  }

  ngOnInit() {
    // טעינת חברות
    this.companyService.getAllCompanies().subscribe(data => {
      this.companies = data;
    });

    // בדיקה אם זה מצב עריכה
    const workerId = this.route.snapshot.paramMap.get('id');

    if (workerId) {
      this.isEditMode = true;

      // ✅ במצב עריכה - הסר את הדרישה לכל השדות
      this.workerForm.get('password')?.clearValidators();
      this.workerForm.get('password')?.updateValueAndValidity();

      this.workerForm.get('email')?.clearValidators();
      this.workerForm.get('email')?.updateValueAndValidity();

      this.workerForm.get('firstname')?.clearValidators();
      this.workerForm.get('firstname')?.updateValueAndValidity();

      this.workerForm.get('lastname')?.clearValidators();
      this.workerForm.get('lastname')?.updateValueAndValidity();

      // ✅ הפיכת שדות אישיים ל-readonly במצב עריכה
      this.workerForm.get('firstname')?.disable();
      this.workerForm.get('lastname')?.disable();
      this.workerForm.get('email')?.disable();
      this.workerForm.get('password')?.disable();
      this.workerForm.get('phone')?.disable();

      this.workerService.getWorkerById(+workerId).subscribe(worker => {
        let hireDateValue: string | null = null;
        if (worker.hireDate) {
          try {
            // אם זה string כמו "2024-01-15T00:00:00"
            const dateObj = new Date(worker.hireDate);

            // המרה לפורמט YYYY-MM-DD
            const year = dateObj.getFullYear();
            const month = String(dateObj.getMonth() + 1).padStart(2, '0');
            const day = String(dateObj.getDate()).padStart(2, '0');
            hireDateValue = `${year}-${month}-${day}`;

            console.log('Converted hireDate:', hireDateValue); // ✅ בדיקה
          } catch (e) {
            console.error('Error parsing date:', e);
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
          isactive: worker.isActive
        });
        console.log('Form after patch:', this.workerForm.value); // ✅ בדיקה

        // ✅ טעינת החברות שנבחרו - הצגת סימון בchecklist
        if (worker.companyIds && Array.isArray(worker.companyIds)) {
          this.selectedCompanyIds = [...worker.companyIds]; // שכפול המערך
          console.log('Selected company IDs after load:', this.selectedCompanyIds);

        }
      });
    }
  }

  // ✅ פונקציה לבדיקה אם חברה נבחרה
  isCompanySelected(companyId: number): boolean {
    return this.selectedCompanyIds.includes(companyId);
  }

  // ✅ פונקציה להוספה/הסרה של חברה
  onCompanyToggle(companyId: number, event: any) {
    if (event.target.checked) {
      // הוספה
      if (!this.selectedCompanyIds.includes(companyId)) {
        this.selectedCompanyIds.push(companyId);
      }
    } else {
      // הסרה
      const index = this.selectedCompanyIds.indexOf(companyId);
      if (index > -1) {
        this.selectedCompanyIds.splice(index, 1);
      }
    }

    console.log('Selected companies:', this.selectedCompanyIds);
  }
togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
  onSubmit() {
    if (this.workerForm.invalid) {
      console.log('Form invalid:', this.workerForm.value);
      Object.keys(this.workerForm.controls).forEach(key => {
        const control = this.workerForm.get(key);
        if (control?.invalid) {
          console.log(`Invalid field: ${key}`, control.errors);
        }
      });
      return;
    }

    // ✅ במצב עריכה - צריך לקבל גם שדות disabled
    const formValue = this.isEditMode
      ? this.workerForm.getRawValue()
      : this.workerForm.value;

    // ✅ בנייה של payload
    const payload: any = {
      isactive: formValue.isactive
    };

    // ✅ במצב עריכה - שלח רק שדות שניתן לערוך
    if (this.isEditMode) {
      payload.id = formValue.id;
      // שדות שניתנים לעריכה
      if (formValue.roleid !== null && formValue.roleid !== undefined) {
        payload.roleid = formValue.roleid;
      }
      if (formValue.employeeid) payload.employeeid = formValue.employeeid;
      if (formValue.hiredate) payload.hiredate = formValue.hiredate;
    } else {
      // ✅ במצב הוספה - שלח את כל השדות הנדרשים
      payload.roleid = formValue.roleid;
      payload.firstname = formValue.firstname;
      payload.lastname = formValue.lastname;
      payload.email = formValue.email;

      if (formValue.password) {
        payload.password = formValue.password;
      }
      if (formValue.employeeid) payload.employeeid = formValue.employeeid;
      if (formValue.phone) payload.phone = formValue.phone;
      if (formValue.hiredate) payload.hiredate = formValue.hiredate;
    }

    // ✅ הוספת חברות שנבחרו (גם בהוספה וגם בעריכה)
    if (this.selectedCompanyIds.length > 0) {
      payload.companyIds = this.selectedCompanyIds;
    }

    console.log('Sending payload:', payload);

    const request$ = this.isEditMode
      ? this.workerService.updateWorker(formValue.id!, payload)
      : this.workerService.addWorker(payload);

    request$.subscribe({
      next: () => {
        this.saved.emit();
        this.router.navigate(['/workers']);
      },
      error: (err) => {
        console.error('Error saving worker:', err);
        alert('שגיאה בשמירת העובד: ' + (err.error?.message || err.message));
      }
    });
  }
  goBack(): void {
    this.router.navigate(['/workers']);
  }
  //  goBack(): void {
  //   if (this.route) {
  //     this.router.navigate([this.route]);
  //   } else {
  //     this.location.back();
  //   }
  // }
}