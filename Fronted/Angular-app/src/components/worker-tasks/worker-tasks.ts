
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { WorkerTask } from '../../models/auth';
import { WorkerService } from '../../services/worker';
import { CompanyService } from '../../services/company';

@Component({
  selector: 'app-worker-tasks',
  templateUrl: './worker-tasks.html',
  styleUrls: ['./worker-tasks.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class WorkerTasksComponent implements OnInit {
  // 👇 הוסיפי את זה בהתחלה
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
    private companyService: CompanyService
  ) {}

  ngOnInit(): void {
    const currentWorker = this.workerService.getCurrentWorker();

    if (!currentWorker) {
      console.error('❌ אין עובד מחובר');
      return;
    }

    this.workerId = currentWorker.id;
    this.workerName = `${currentWorker.firstname} ${currentWorker.lastname}`;

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
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת משימות', err);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.filteredTasks = this.tasks.filter(task => {
      const matchesStatus =
        this.selectedStatus === 'all' || task.status.toString() === this.selectedStatus;

      const matchesSearch =
        !this.searchTerm ||
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
    this.companyService
      .updateTaskStatus(task.companyid, task.id, newStatus.toString())
      .subscribe({
        next: () => {
          task.status = newStatus;
          this.applyFilters();
          this.calculateStats();
          console.log('✅ סטטוס עודכן:', task.id, newStatus);
        },
        error: (err) => {
          console.error('❌ שגיאה בעדכון סטטוס', err);
          alert('שגיאה בעדכון הסטטוס');
        }
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
      overdue: this.tasks.filter(t =>
        t.duedate &&
        new Date(t.duedate) < today &&
        t.status !== this.STATUS.DONE
      ).length
    };
  
    console.log('📊 תוצאות סטטיסטיקה:', this.taskStats);
  }

  getStatusText(status: number): string {
    const map: { [key: number]: string } = {
      1: 'ממתין',
      2: 'בתהליך',
      3: 'הושלם',
      4: 'שולם',
      5: 'לא נדרש'
    };
    return map[status] || 'לא ידוע';
  }
  getStatusClass(status: number): string {
    const map: { [key: number]: string } = {
      1: 'status-pending',
      2: 'status-in-progress',
      3: 'status-done',
      4: 'status-paid',
      5: 'status-not-required'
    };
    return map[status] || '';
  }

  getTaskPriority(task: WorkerTask): string {
    if (!task.duedate) return '';

    const dueDate = new Date(task.duedate);
    const today = new Date();
    const diffDays = Math.ceil(
      (dueDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24)
    );

    if (diffDays < 0) return 'overdue';
    if (diffDays <= 3) return 'urgent';
    if (diffDays <= 7) return 'soon';
    return '';
  }
}
