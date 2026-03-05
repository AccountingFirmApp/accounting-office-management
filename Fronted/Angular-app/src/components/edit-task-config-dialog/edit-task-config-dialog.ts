import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { CompanyTaskConfigDto } from '../../models/auth';
import { WorkerService } from '../../services/worker';

@Component({
  selector: 'app-edit-task-config-dialog',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatDialogModule, 
    MatFormFieldModule, 
    MatSelectModule, 
    MatInputModule, 
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatSlideToggleModule
  ],
  templateUrl: './edit-task-config-dialog.html',
  styleUrls: ['./edit-task-config-dialog.css']
})


export class EditTaskConfigDialogComponent implements OnInit {
  workers: any[] = []; 

  constructor(
    private workerService: WorkerService, 
    public dialogRef: MatDialogRef<EditTaskConfigDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    // העתקה עמוקה מבטיחה ניתוק מוחלט מהמטריצה בזמן העריכה
    this.data = JSON.parse(JSON.stringify(data)); 
  }

  ngOnInit(): void {
  
    if (this.data.companyId) {
      this.workerService.getWorkersbyCompany(this.data.companyId).subscribe({
        next: (res) => {
          this.workers = res;
         
        },
        error: (err) => console.error('שגיאה בטעינת עובדים', err)
      });
    }
  }
  save() {
  
    const selectedWorker = this.workers.find(w => w.id === this.data.assignedWorkerId);
    

    this.data.workerName = selectedWorker ? selectedWorker.fullName : 'לא שובץ';
    this.data.isActive = true;
  
    this.dialogRef.close(this.data);
  }
  deleteConfig() {
    if (confirm('האם את בטוחה?')) {
      this.data.isActive = false;
      this.data.assignedWorkerId = null; 
      this.data.workerName = null;      
      this.dialogRef.close(this.data);   
    }
  }
    onNoClick(): void {
    this.dialogRef.close();
  }
}
