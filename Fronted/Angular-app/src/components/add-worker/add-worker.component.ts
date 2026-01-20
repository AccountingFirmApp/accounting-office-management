// import { Component, OnInit } from '@angular/core';
// import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';
// import { Router, ActivatedRoute } from '@angular/router';
// import { HttpClientModule } from '@angular/common/http';

// import { WorkerService } from '../../services/worker';

// @Component({
//   selector: 'app-worker-form',
//   standalone: true,
//   imports: [
//     CommonModule,
//     ReactiveFormsModule,
//     HttpClientModule
//   ],
//   templateUrl: './add-worker.component.html',
//   styleUrls: ['./add-worker.component.css']
// })
// export class WorkerFormComponent implements OnInit {

//   isEditMode = false;

//   roles = [
//     { id: 1, name: 'Admin' },
//     { id: 2, name: 'Manager' },
//     { id: 3, name: 'Employee' }
//   ];

//   workerForm!: FormGroup;

//   constructor(
//     private fb: FormBuilder,
//     private workerService: WorkerService,
//     private router: Router,
//     private route: ActivatedRoute
//   ) {
//     this.workerForm = this.fb.group({
//       id: [null],
//       firstName: ['', Validators.required],
//       lastName: ['', Validators.required],
//       email: ['', [Validators.required, Validators.email]],
//       roleId: [null, Validators.required],
//       firmId: [null, Validators.required],
//       isActive: [true]
//     });
//   }

//   ngOnInit(): void {
//     const workerId = this.route.snapshot.paramMap.get('id');

//     if (workerId) {
//       this.isEditMode = true;

//       this.workerService.getWorkerById(+workerId).subscribe({
//         next: (worker) => {
//           console.log('roleName from server:', worker.roleId);
// console.log('mapped roleId:', this.mapRoleNameToId(worker.roleName));

//           this.workerForm.patchValue({
//             id: worker.id,
//             firstName: worker.firstName,
//             lastName: worker.lastName,
//             email: worker.email,
//             roleId: this.mapRoleNameToId(worker.roleName),
//             firmId: worker.firmId,
//             isActive: worker.isActive
//           });
//         },
//         error: (err) => {
//           console.error('שגיאה בטעינת עובד', err);
//         }
//       });
//     }
//   }

//   onSubmit(): void {
//     if (this.workerForm.invalid) {
//       this.workerForm.markAllAsTouched();
//       return;
//     }

//     const payload = this.workerForm.value;
//     console.log('payload:', payload);

//     if (this.isEditMode && payload.id) {
//       this.workerService.updateWorker(payload.id, payload)
//         .subscribe(() => this.router.navigate(['/workers']));
//     } else {
//       this.workerService.addWorker(payload)
//         .subscribe(() => this.router.navigate(['/workers']));
//     }
//   }

//   private mapRoleNameToId(roleName: string): number | null {
//     if (!roleName) return null;
  
//     const role = this.roles.find(
//       r => r.name.toLowerCase() === roleName.toLowerCase()
//     );
  
//     return role ? role.id : null;
//   }
  

//   goback(): void {
//     this.router.navigate(['/workers']);
//   }
// }
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { WorkerService } from '../../services/worker';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-worker-form',
  templateUrl: './add-worker.component.html',
  styleUrls: ['./add-worker.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    CommonModule
  ],
})
export class WorkerFormComponent implements OnInit {
  @Input() worker: any = null; // אם יש - מצב עריכה
  @Output() saved = new EventEmitter<void>();

  isEditMode = false;

  // רשימת תפקידים
  roles = [
    { id: 1, name: 'Admin' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Employee' }
  ];

  workerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private workerService: WorkerService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.workerForm = this.fb.group({
      id: [null],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      roleId: [null, Validators.required],   // ✅ עכשיו יש roleId
      firmId: [null, Validators.required],
      isActive: [true]
    });
  }

  ngOnInit() {
    const workerId = this.route.snapshot.paramMap.get('id');

    if (workerId) {
      this.isEditMode = true;
      this.workerService.getWorkerById(+workerId).subscribe(worker => {
        // עכשיו אפשר פשוט לפאטש בלי המרה
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
    console.log('payload:', payload);

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
