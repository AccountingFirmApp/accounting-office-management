// src/app/components/workers-list/workers-list.component.ts
import { Component, OnInit } from '@angular/core';
import { WorkerService } from '../../services/worker';
import { WorkerInfoDto } from '../../models/auth';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { WorkerDto } from '../../models/worker';

@Component({
  selector: 'app-workers-list',
   standalone: true,
    imports: [CommonModule, BackButtonComponent],
  templateUrl: './workers-list-component.component.html',
  styleUrls: ['workers-list-component.component.css']
})
export class WorkersListComponent implements OnInit {

  workers: WorkerInfoDto[] = [];
  isLoading = false;
  errorMessage = '';
  selectedWorker: WorkerInfoDto | null = null;

  constructor(private WorkerService: WorkerService,   private router: Router,
    private route: ActivatedRoute,

  ) {}

  ngOnInit(): void {
    this.loadWorkers();
    
  }

  loadWorkers(): void {
    this.isLoading = true;

    this.WorkerService.getallWorkers().subscribe({
      next: (data) => {
        this.workers = data;
        this.isLoading = false;
      },
      error: (err) => {
        // console.error('❌ שגיאה בטעינת עובדים', err);
        this.errorMessage = 'אירעה שגיאה בטעינת העובדים';
        this.isLoading = false;
      }
    });
  }
  goHome(): void {
    this.router.navigate(['/home']);
  }
  openCreate() {
    this.router.navigate(['/workers/create']);
  }
  
  openEdit(worker: any) {
    this.router.navigate(['/workers', worker.id, 'edit']);
  }
  
  // viewWorker(worker: any) {
  //   // אופציונלי – אם יש דף צפייה
  //   this.router.navigate(['/workers', worker.id]);
  // }
  viewWorker(worker: WorkerInfoDto): void {
    this.selectedWorker= worker;
    // console.log('Selected worker:', worker);
   
  }

  // ⭐ סגירת המודל
  closeModal(): void {
    this.selectedWorker = null;
  }
  // deleteWorker(employeeId: number) {
  //   // שואל את המשתמש לפני מחיקה
  //   const confirmed = window.confirm('האם אתה בטוח שברצונך למחוק את העובד?');
  
  //   if (!confirmed) {
  //     return; // אם המשתמש ביטל, לא מבצעים מחיקה
  //   }
  
  //   // אם המשתמש אישר, מבצעים מחיקה
  //   this.WorkerService.deleteWorker(employeeId).subscribe({ 
  //     next: () => {
  //       this.loadWorkers(); // טען מחדש את רשימת העובדים לאחר המחיקה
  //     },
  //     error: (err) => {
  //       console.error('❌ שגיאה במחיקת עובד', err);
  //       this.errorMessage = 'אירעה שגיאה במחיקת העובד';
  //     }
  //   });
  // }
}  