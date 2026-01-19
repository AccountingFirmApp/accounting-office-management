// import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
// import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
// import { WorkerService } from '../../services/worker';
// import { HttpClientModule } from '@angular/common/http';
// import { CommonModule } from '@angular/common';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-worker-form',
//   templateUrl: 'add-worker.component.html',
//   styleUrls: ['./add-worker.component.css'],
//   standalone: true,   // <-- הוסף את זה
//   imports: [
//     ReactiveFormsModule,
//     HttpClientModule,
//     CommonModule
    
//   ],
// })
// export class WorkerFormComponent implements OnInit {
//   @Input() worker: any = null; // אם יש - מצב עריכה
//   @Output() saved = new EventEmitter<void>(); // ליידע את ההורה שהעובד נשמר

//   workerForm: FormGroup;
//   errorMessage: string = '';
//   successMessage: string = '';

//   constructor(private fb: FormBuilder, private workerService: WorkerService,    private router: Router,
//   ) {
//     this.workerForm = this.fb.group({
//       name: ['', Validators.required],
//       email: ['', [Validators.required, Validators.email]],
//       role: ['', Validators.required],
//       firmId: [null, Validators.required]
//     });
//   }

//   ngOnInit() {
//     const workerId = this.route.snapshot.paramMap.get('id');
//     if (workerId) {
//       this.isEditMode = true;
//       this.workerService.getWorkerById(+workerId).subscribe(worker => {
//         this.workerForm.patchValue(worker);
//       });
//     }
//   }
  
//   onSubmit() {
//     if (this.workerForm.invalid) return;
  
//     const request$ = this.isEditMode
//       ? this.workerService.updateWorker(this.workerForm.value.id, this.workerForm.value)
//       : this.workerService.addWorker(this.workerForm.value);
  
//     request$.subscribe(() => this.router.navigate(['/workers']));
//   }
//   goback(): void {
//     this.router.navigate(['/workers']);
//   }
// }

import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkerService } from '../../services/worker';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-worker-form',
  templateUrl: 'add-worker.component.html',
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

  workerForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';
  isEditMode: boolean = false; // ← הוסף את זה

  constructor(
    private fb: FormBuilder,
    private workerService: WorkerService,
    private router: Router,
    private route: ActivatedRoute // ← הוסף את זה
  ) {
    this.workerForm = this.fb.group({
      firstName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      RoleId: ['', Validators.required],
      firmId: [null, Validators.required]
    });
  }

  ngOnInit() {
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

    const request$ = this.isEditMode
      ? this.workerService.updateWorker(this.workerForm.value.id, this.workerForm.value)
      : this.workerService.addWorker(this.workerForm.value);

    request$.subscribe(() => this.router.navigate(['/workers']));
  }

  goback(): void {
    this.router.navigate(['/workers']);
  }
}
