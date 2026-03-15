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
import { RECURRENCE_OPTIONS } from '../../models/enums';
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
  recurrenceOptions = RECURRENCE_OPTIONS;
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
        error: (err) =>
           console.error('שגיאה בטעינת עובדים', err)
      });

    }
  }
  // save() {
  
  //   const selectedWorker = this.workers.find(w => w.id === this.data.assignedWorkerId);
  //   // עדכון השדות שהמטריצה משתמשת בהם להצגה
  //   this.data.firstName = selectedWorker.firstName; // וודאי שאלו השמות בתוך אובייקט ה-worker
  //   this.data.lastName = selectedWorker.lastName;
    
  //   // ליתר ביטחון, אם יש מקומות שמשתמשים בשם המלא
  //   this.data.workerName = selectedWorker.fullName;

  //   this.data.workerName = selectedWorker ? selectedWorker.fullName : 'לא שובץ';
  //   this.data.isActive = true;
  
  //   this.dialogRef.close(this.data);
  // }
  // save() {
  //   console.log('assignedWorkerId:', this.data.assignedWorkerId);
  // console.log('workers list:', this.workers);
  //   // 1. מציאת האובייקט המלא של העובד שנבחר מהרשימה
  //   const selectedWorker = this.workers.find(w => w.id == this.data.assignedWorkerId);    
  //   if (selectedWorker) {
  //     // 2. עדכון השדות שהמטריצה (task-matrix) מציגה ב-HTML שלה
  //     // אנחנו חייבים לעדכן את firstName ו-lastName כי המטריצה מציגה אותם ישירות
  //     this.data.firstName = selectedWorker.firstName;
  //     this.data.lastName = selectedWorker.lastName;
      
  //     // עדכון שם מלא ליתר ביטחון
  //     this.data.workerName = selectedWorker.fullName || (selectedWorker.firstName + ' ' + selectedWorker.lastName);
      
  //     // טיפול במקרה שהשדות ב-selectedWorker הם באותיות קטנות (בגלל Postgres/JSON)
  //     if (!selectedWorker.firstName && selectedWorker['firstname']) {
  //       this.data.firstName = selectedWorker['firstname'];
  //       this.data.lastName = selectedWorker['lastname'];
  //     }
  //   } else {
  //     // אם לא נבחר עובד
  //     this.data.firstName = '';
  //     this.data.lastName = '';
  //     this.data.workerName = 'לא שובץ';
  //   }
  
  //   // 3. הגדרת המשימה כפעילה וסגירת הדיאלוג עם הנתונים החדשים
  //   this.data.isActive = true;
  //   this.dialogRef.close(this.data);
  // }


  save() {
  
    const selectedWorker = this.workers.find(w => w.id == this.data.assignedWorkerId);
    
    if (selectedWorker) {
      console.log('העובד נמצא! שם:', selectedWorker.fullName);
      
      this.data.firstName = selectedWorker.fullName.split(' ')[0]; // פירוק שם פרטי
      this.data.lastName = selectedWorker.fullName.split(' ')[1] || ''; // פירוק שם משפחה
      this.data.workerName = selectedWorker.fullName; 
    } else {
      this.data.firstName = null;
      this.data.lastName = null;
      this.data.workerName = 'לא שובץ';
    }
  
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
