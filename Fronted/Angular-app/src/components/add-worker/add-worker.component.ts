
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

  // רשימת תפקידים
  roles = [
    { id: 1, name: 'Admin' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Employee' }
  ];

  workerForm: FormGroup;
  companies: { id: number; name: string }[] = [];
  constructor(
    private fb: FormBuilder,
    private workerService: WorkerService,
    private companyService: CompanyService, 
    private router: Router,
    private route: ActivatedRoute,
    
  ) {
    this.workerForm = this.fb.group({
      id: [null],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      roleId: [null, Validators.required],  
      firmId: [null, Validators.required],
      isActive: [true],
      PasswordHash: ['', this.isEditMode ? [] : [Validators.minLength(6)]]
    });
  }

  ngOnInit() {
    this.companyService.getAllCompanies().subscribe(data => {
      this.companies = data;
      });
    const workerId = this.route.snapshot.paramMap.get('id');

    if (workerId) {
      this.isEditMode = true;
      this.workerService.getWorkerById(+workerId).subscribe(worker => {
        this.workerForm.patchValue(worker);
      });
    } else if (this.worker) {
      this.isEditMode = true;
      this.workerForm.patchValue(this.worker);
    }
  }

  onSubmit() {
    if (this.workerForm.invalid) return;

    const payload = this.workerForm.value;

    const request$ = this.isEditMode
      ? this.workerService.updateWorker(payload.id!, payload)
      : this.workerService.addWorker(payload);

    request$.subscribe(() => {
      this.saved.emit();
      this.router.navigate(['/workers']);
    });
  }

  goback(): void {
    this.router.navigate(['/workers']);
  }
}
