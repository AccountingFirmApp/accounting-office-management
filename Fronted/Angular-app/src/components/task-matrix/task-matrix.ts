import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { CompanyTaskConfigDto } from '../../models/auth';
import { TaskConfigsService } from '../../services/TaskConfigsService';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

// Imports של Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTableModule } from '@angular/material/table';
import { EditTaskConfigDialogComponent } from '../edit-task-config-dialog/edit-task-config-dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BackButtonComponent } from "../../app/components/shared/back-button/back-button.component";

@Component({
  selector: 'app-task-matrix',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatCheckboxModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatTooltipModule,
    BackButtonComponent
],
  templateUrl: './task-matrix.html',
  styleUrls: ['./task-matrix.css']
})
export class TaskMatrixComponent implements OnInit {
  matrixData: CompanyTaskConfigDto[] = [];
  distinctTaskTypes: string[] = [];
  distinctCompanies: string[] = [];

  constructor(
    private configService: TaskConfigsService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadMatrix();
  }

  loadMatrix(): void {
    this.configService.getTaskMatrix().subscribe({
      next: (data) => {
        this.matrixData = data;
        this.distinctTaskTypes = [...new Set(data.map(item => item.taskTypeName))].sort();
        this.distinctCompanies = [...new Set(data.map(item => item.companyName))].sort();
      },
      error: (err) => console.error('שגיאה בטעינת המטריצה:', err)
    });
  }

  getConfig(company: string, taskType: string): CompanyTaskConfigDto | undefined {
    return this.matrixData.find(m => 
      m.companyName === company && m.taskTypeName === taskType
    );
  }

  openEditDialog(config: CompanyTaskConfigDto): void {
    const dialogRef = this.dialog.open(EditTaskConfigDialogComponent, {
      width: '550px',      
      maxWidth: '95vw',   
      data: { ...config }, 
      direction: 'rtl',    
      hasBackdrop: true,   
      panelClass: 'modern-dialog-container' 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.saveConfig(result);
      }
    });
  }

  
  saveConfig(updatedConfig: CompanyTaskConfigDto) {
    this.configService.saveTaskConfig(updatedConfig).subscribe({
      next: (savedResultFromServer) => {
        const index = this.matrixData.findIndex(m => 
          m.companyId === updatedConfig.companyId && 
          m.taskTypeId === updatedConfig.taskTypeId
        );
  
        if (index !== -1) {
    
          const freshObject = {
            ...this.matrixData[index], 
            ...updatedConfig,          
            isActive: updatedConfig.isActive
          };
  
          this.matrixData[index] = freshObject;
          this.matrixData = [...this.matrixData]; 
  
  
        } else {
       
          this.loadMatrix();
        }
      },
      error: (err) => alert('שגיאה בשמירה')
    });
  }
  trackByFn(index: number, item: any) {
    return item.id || `${item.companyId}-${item.taskTypeId}`;
  }

trackByCompany(index: number, company: string): string {
  return company;
}


trackByTaskType(index: number, type: string): string {
  return type;
}
}