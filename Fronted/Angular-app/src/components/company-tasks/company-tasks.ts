

import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule ,Location} from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company';
import { TaskcompanyDto } from '../../models/taskcompany';
import { CompanyDto } from '../../models/Company';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { CompantTaskService } from '../../services/compant-task.service';
import { LoadingComponent } from '../../app/components/shared/loading/loading.component';
import { ErrorMessageComponent } from '../../app/components/shared/error-message/error-message.component';

@Component({
  selector: 'app-company-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, BackButtonComponent, LoadingComponent, ErrorMessageComponent],
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
  updatingTaskId: number | null = null;  
  statusToNumber: { [key: string]: number } = {
    'Pending': 0,
    'InProgress': 1,
    'Done': 2,
    'Paid': 3,
    'NotRequired': 4
  };

  availableStatuses = ['Pending', 'InProgress', 'Done', 'Paid', 'NotRequired'];
selectedTaskDetails: any = null; // יחזיק את המשימה המורחבת כולל הצ'קליסט
showChecklistModal = false;     

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private companyService: CompanyService,
    private taskService: CompantTaskService,
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
        // console.error('❌ שגיאה בטעינת פרטי החברה:', err);
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
        // console.error('❌ שגיאה בטעינת המשימות:', err);
        this.error = `שגיאה בטעינת המשימות: ${err.message}`;
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onStatusChange(task: TaskcompanyDto, newStatus: string): void {
    
    const oldStatus = task.status;  
    this.updatingTaskId = task.id;  
    
    // עדכון מיידית בממשק 
    task.status = newStatus;
    this.cdr.detectChanges();
    
    this.companyService.updateTaskStatus(this.companyId, task.id, newStatus).subscribe({
      next: (response) => {
        this.updatingTaskId = null;
        this.cdr.detectChanges();
        
      
      },
      error: (err) => {
        // console.error('❌ שגיאה בעדכון סטטוס:', err);
        
        task.status = oldStatus;
        this.updatingTaskId = null;
        this.cdr.detectChanges();
        
      }
    });
  }

  

  getStatusColor(status: string): string {
    switch(status) {
      case 'Done': return '#4CAF50';        
      case 'InProgress': return '#2196F3';
      case 'Pending': return '#FF9800';
      case 'Paid': return '#9C27B0';          
      case 'NotRequired': return '#9E9E9E';  
      default: return '#757575';
    }
  }
  
  getStatusText(status: string): string {
    switch(status) {
      case 'Done': return 'הושלמה';         
      case 'InProgress': return 'בתהליך';
      case 'Pending': return 'ממתינה';
      case 'Paid': return 'שולם';            
      case 'NotRequired': return 'לא נדרש';  
      default: return status;
    }
  }

  isUpdating(taskId: number): boolean {
    return this.updatingTaskId === taskId;
  }
  goBack(): void {
    this.router.navigate(['/companies']);
  }
  

openTaskDetails(taskId: number): void {
  this.loading = true;
  this.taskService.getTaskDetails(taskId).subscribe({
    next: (data) => {
      this.selectedTaskDetails = data;
      this.showChecklistModal = true;
      this.loading = false;
      this.cdr.detectChanges();
    },
    error: (err) => {
      console.error('❌ שגיאה בטעינת פרטי צ\'קליסט:', err);
      this.loading = false;
    }
  });
}

toggleChecklistItem(item: any): void {
  const workerId = 1; 
  
  this.taskService.toggleChecklistItem(item.id, item.isCompleted, workerId).subscribe({
    next: () => {
      item.isCompleted = !item.isCompleted;
      this.updateProgressLocally();
      this.cdr.detectChanges();
    }
  });
}

private updateProgressLocally(): void {
  if (this.selectedTaskDetails) {
    const completed = this.selectedTaskDetails.checklistItems.filter((i: any) => i.isCompleted).length;
    this.selectedTaskDetails.checklistProgress.completed = completed;
  }
}

closeModal(): void {
  this.showChecklistModal = false;
  this.selectedTaskDetails = null;
  this.loadTasks(); 
}
}