
// import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { ActivatedRoute, Router } from '@angular/router';
// import { FormsModule } from '@angular/forms';
// import { CompanyService } from '../../services/company';
// import { TaskDto } from '../../models/task';
// import { CompanyDto } from '../../models/Company';

// @Component({
//   selector: 'app-company-tasks',
//   standalone: true,
//   imports: [CommonModule, FormsModule],
//   templateUrl: './company-tasks.html',
//   styleUrls: ['./company-tasks.css']
// })
// export class CompanyTasksComponent implements OnInit {
//   companyId!: number;
//   company: CompanyDto | null = null;
//   tasks: TaskDto[] = [];
//   loading = false;
//   error: string | null = null;

//   availableStatuses = ['Pending', 'InProgress', 'Completed', 'Cancelled', 'OnHold'];

//   constructor(
//     private route: ActivatedRoute,
//     private router: Router,
//     private companyService: CompanyService,
//     private cdr: ChangeDetectorRef  // ← הוסף את זה
//   ) { 
//   }

//   ngOnInit(): void {
//     this.route.params.subscribe(params => {
//       this.companyId = +params['id'];
//       this.loadCompanyInfo();
//       this.loadTasks();
//     });
//   }

//   loadCompanyInfo(): void {
//     this.companyService.getCompanyById(this.companyId).subscribe({
//       next: (data) => {
//         this.company = data;
//         this.cdr.detectChanges(); // ← הוסף את זה
//       },
//       error: (err) => {
//         console.error('❌ שגיאה בטעינת פרטי החברה:', err);
//       }
//     });
//   }

//   loadTasks(): void {
//     this.loading = true;
//     this.error = null;
//     this.cdr.detectChanges(); // ← הוסף את זה
    
//     this.companyService.getTasksByCompanyId(this.companyId).subscribe({
//       next: (data) => {
//         this.tasks = data;
//         this.loading = false;
//         this.cdr.detectChanges(); // ← הוסף את זה
//       },
//       error: (err) => {
//         console.error('❌ שגיאה בטעינת המשימות:', err);
//         console.error('❌ פרטי השגיאה:', err.error);
//         this.error = `שגיאה בטעינת המשימות: ${err.message}`;
//         this.loading = false;
//         this.cdr.detectChanges(); // ← הוסף את זה
//       }
//     });
//   }

//   onStatusChange(task: TaskDto, newStatus: string): void {
//     task.status = newStatus;
//   }

//   goBack(): void {
//     this.router.navigate(['/companies']);
//   }

//   getStatusColor(status: string): string {
//     switch(status) {
//       case 'Completed': return '#4CAF50';
//       case 'InProgress': return '#2196F3';
//       case 'Pending': return '#FF9800';
//       case 'Cancelled': return '#f44336';
//       case 'OnHold': return '#9E9E9E';
//       default: return '#757575';
//     }
//   }

//   getStatusText(status: string): string {
//     switch(status) {
//       case 'Completed': return 'הושלמה';
//       case 'InProgress': return 'בתהליך';
//       case 'Pending': return 'ממתינה';
//       case 'Cancelled': return 'בוטלה';
//       case 'OnHold': return 'מושהית';
//       default: return status;
//     }
//   }
// }
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule ,Location} from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company';
import { TaskcompanyDto } from '../../models/taskcompany';
import { CompanyDto } from '../../models/Company';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';

@Component({
  selector: 'app-company-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, BackButtonComponent],
  templateUrl: './company-tasks.html',
  styleUrls: ['./company-tasks.css']
})
export class CompanyTasksComponent implements OnInit {
[x: string]: any;
  companyId!: number;
  company: CompanyDto | null = null;
  tasks: TaskcompanyDto[] = [];
  loading = false;
  error: string | null = null;
  updatingTaskId: number | null = null;  // ← הוסף - למעקב על המשימה שמתעדכנת
  statusToNumber: { [key: string]: number } = {
    'Pending': 0,
    'InProgress': 1,
    'Done': 2,
    'Paid': 3,
    'NotRequired': 4
  };

  availableStatuses = ['Pending', 'InProgress', 'Done', 'Paid', 'NotRequired'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private companyService: CompanyService,
    private cdr: ChangeDetectorRef,
    private location:Location

  ) { 
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.companyId = +params['id'];
      this.loadCompanyInfo();
      this.loadTasks();
    });
  }

  loadCompanyInfo(): void {
    this.companyService.getCompanyById(this.companyId).subscribe({
      next: (data) => {
        this.company = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת פרטי החברה:', err);
      }
    });
  }

  loadTasks(): void {
    this.loading = true;
    this.error = null;
    this.cdr.detectChanges();
    
    this.companyService.getTasksByCompanyId(this.companyId).subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת המשימות:', err);
        this.error = `שגיאה בטעינת המשימות: ${err.message}`;
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // ← הפונקציה המעודכנת!
  onStatusChange(task: TaskcompanyDto, newStatus: string): void {
    
    const oldStatus = task.status;  // שמור את הסטטוס הישן למקרה של שגיאה
    this.updatingTaskId = task.id;  // סמן שהמשימה הזו מתעדכנת
    
    // עדכן מיידית בממשק (Optimistic Update)
    task.status = newStatus;
    this.cdr.detectChanges();
    
    // שלח לשרת
    this.companyService.updateTaskStatus(this.companyId, task.id, newStatus).subscribe({
      next: (response) => {
        this.updatingTaskId = null;
        this.cdr.detectChanges();
        
        // הצג הודעת הצלחה (אופציונלי)
        // alert('הסטטוס עודכן בהצלחה');
      },
      error: (err) => {
        console.error('❌ שגיאה בעדכון סטטוס:', err);
        
        // החזר את הסטטוס הישן
        task.status = oldStatus;
        this.updatingTaskId = null;
        this.cdr.detectChanges();
        
        alert('שגיאה בעדכון הסטטוס: ' + (err.error?.message || err.message));
      }
    });
  }

  

  getStatusColor(status: string): string {
    switch(status) {
      case 'Done': return '#4CAF50';          // ← שינוי
      case 'InProgress': return '#2196F3';
      case 'Pending': return '#FF9800';
      case 'Paid': return '#9C27B0';          // ← חדש
      case 'NotRequired': return '#9E9E9E';   // ← חדש
      default: return '#757575';
    }
  }
  
  getStatusText(status: string): string {
    switch(status) {
      case 'Done': return 'הושלמה';          // ← שינוי
      case 'InProgress': return 'בתהליך';
      case 'Pending': return 'ממתינה';
      case 'Paid': return 'שולם';            // ← חדש
      case 'NotRequired': return 'לא נדרש';  // ← חדש
      default: return status;
    }
  }

  // פונקציה חדשה - בדוק אם המשימה הזו מתעדכנת
  isUpdating(taskId: number): boolean {
    return this.updatingTaskId === taskId;
  }
  goBack(): void {
    this.router.navigate(['/companies']);
  }
  // goHome(): void {
  //   this.location.back();
  // }

}