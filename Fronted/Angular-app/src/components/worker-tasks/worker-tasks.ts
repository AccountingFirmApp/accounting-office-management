import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; 
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { WorkerTask } from '../../models/auth';
import { WorkerService } from '../../services/worker';
import { CompanyService } from '../../services/company';
import { CompantTaskService } from '../../services/compant-task.service';

@Component({
  selector: 'app-worker-tasks',
  templateUrl: './worker-tasks.html',
  styleUrls: ['./worker-tasks.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class WorkerTasksComponent implements OnInit {
  private readonly STATUS = {
    PENDING: 1,
    IN_PROGRESS: 2,
    DONE: 3,
    PAID: 4,
    NOT_REQUIRED: 5
  };

  tasks: WorkerTask[] = [];
  filteredTasks: WorkerTask[] = [];

  loading = false;
  workerId: number | null = null;
  workerName = '';

  searchTerm = '';
  selectedStatus = 'all';
  
  selectedTaskDetails: any = null;
  showChecklistModal = false;

  taskStats = {
    total: 0,
    pending: 0,
    inProgress: 0,
    done: 0,
    paid: 0,
    notRequired: 0,
    overdue: 0
  };

  constructor(
    private workerService: WorkerService,
    private companyService: CompanyService,
    private companyTaskService: CompantTaskService,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit(): void {
    const currentWorker = this.workerService.getCurrentWorker();
    if (!currentWorker) {
      console.error('❌ אין עובד מחובר');
      return;
    }
    this.workerId = currentWorker.id;
    this.workerName = `${currentWorker.firstName} ${currentWorker.lastName}`;
    this.loadTasks();
  }

  loadTasks(): void {
    if (!this.workerId) return;
    this.loading = true;
    this.workerService.getWorkerTasks(this.workerId).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.applyFilters();
        this.calculateStats();
        this.loading = false;
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת משימות', err);
        this.loading = false;
      }
    });
  }



  openTaskDetails(taskId: number): void {

    this.loading = true;
    
    this.companyTaskService.getTaskDetails(taskId).subscribe({
      next: (data) => {

        this.selectedTaskDetails = data;
        this.showChecklistModal = true;
        this.loading = false;
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת הצ\'קליסט:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  toggleChecklistItem(item: any): void {
    if (!this.workerId) return;

    this.companyTaskService.toggleChecklistItem(item.id, item.isCompleted, this.workerId).subscribe({
      next: () => {
        item.isCompleted = !item.isCompleted;
        this.updateProgressLocally();
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('❌ שגיאה בעדכון פריט:', err);
      }
    });
  }

  private updateProgressLocally(): void {
    if (!this.selectedTaskDetails) return;
  
    const items = this.selectedTaskDetails.checklistItems;
    const total = items.length;
    const completed = items.filter((i: any) => i.isCompleted).length;
  
    this.selectedTaskDetails.checklistProgress.completed = completed;
  
    const taskInList = this.tasks.find(t => t.id === this.selectedTaskDetails.id);
    if (!taskInList) return;
  
    let newStatus: number;
  
    if (completed === total && total > 0) {
      newStatus = this.STATUS.DONE;
    } else if (completed > 0 && completed < total) {
      newStatus = this.STATUS.IN_PROGRESS;
    } else {
      newStatus = this.STATUS.PENDING;
    }
  
    if (taskInList.status !== newStatus) {
      this.updateTaskStatus(taskInList, newStatus);
    
    }
  }


  
  closeModal(): void {
    this.showChecklistModal = false;
    this.selectedTaskDetails = null;
    this.loadTasks(); 
    this.cdr.detectChanges();
  }


  applyFilters(): void {
    this.filteredTasks = this.tasks.filter(task => {
      const matchesStatus = this.selectedStatus === 'all' || task.status.toString() === this.selectedStatus;
      const matchesSearch = !this.searchTerm || 
        task.companyName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        task.taskTypeName.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchesStatus && matchesSearch;
    });
  }

  onStatusFilterChange(status: string): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.applyFilters();
  }
  updateTaskStatus(task: WorkerTask, newStatus: number): void {
    this.companyService.updateTaskStatus(task.companyid, task.id, newStatus.toString()).subscribe({
      next: () => {
        task.status = newStatus; 
        this.calculateStats();  
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error('❌ שגיאה בעדכון סטטוס:', err)
    });
  }
  calculateStats(): void {
    const today = new Date();
    this.taskStats = {
      total: this.tasks.length,
      pending: this.tasks.filter(t => t.status === this.STATUS.PENDING).length,
      inProgress: this.tasks.filter(t => t.status === this.STATUS.IN_PROGRESS).length,
      done: this.tasks.filter(t => t.status === this.STATUS.DONE).length,
      paid: this.tasks.filter(t => t.status === this.STATUS.PAID).length,
      notRequired: this.tasks.filter(t => t.status === this.STATUS.NOT_REQUIRED).length,
      overdue: this.tasks.filter(t => t.duedate && new Date(t.duedate) < today && t.status !== this.STATUS.DONE).length
    };
  }

  getStatusClass(status: number): string {
    const map: { [key: number]: string } = {
      1: 'status-pending', 2: 'status-in-progress', 3: 'status-done', 4: 'status-paid', 5: 'status-not-required'
    };
    return map[status] || '';
  }

  getTaskPriority(task: WorkerTask): string {
    if (!task.duedate) return '';
    const dueDate = new Date(task.duedate);
    const diffDays = Math.ceil((dueDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
    if (diffDays < 0) return 'overdue';
    if (diffDays <= 3) return 'urgent';
    return '';
  }
}