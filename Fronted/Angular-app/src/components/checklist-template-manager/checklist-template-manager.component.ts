import { Component, OnInit } from '@angular/core';
import { ChecklistTemplate } from '../../models/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChecklistService } from '../../services/checklistTemplateService';
import { MatSelectModule, MatOption, MatLabel, MatFormField } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule, MatCard } from '@angular/material/card';
import { BackButtonComponent } from "../../app/components/shared/back-button/back-button.component";
@Component({
  selector: 'app-checklist-template-manager',
  templateUrl: './checklist-template-manager.component.html',
  styleUrls: ['./checklist-template-manager.component.css'],
  imports: [CommonModule, FormsModule, BackButtonComponent, MatOption, MatLabel, MatFormField, MatCard],
  standalone: true,

})
export class ChecklistTemplateManagerComponent implements OnInit {
  taskTypes: any[] = [];
  selectedTaskTypeId: number | null = null;
  template: any = null;
  items: any[] = [];
  loading = false;

  constructor(private checklistService: ChecklistService) {}

  ngOnInit() {
    this.loadTaskTypes();
  }

  loadTaskTypes() {
    this.checklistService.getTaskTypes().subscribe(data => this.taskTypes = data);
  }

  onTaskTypeChange() {
    if (!this.selectedTaskTypeId) return;
    this.loading = true;
    this.checklistService.getTemplate(this.selectedTaskTypeId).subscribe(res => {
      this.template = res;
      this.items = res?.items || [];
      this.loading = false;
    });
  }

  addItem() {
    this.items.push({
      title: '',
      description: '',
      order_index: this.items.length + 1,
      is_optional: false
    });
  }

  removeItem(index: number) {
    this.items.splice(index, 1);
  }

  saveTemplate() {
    if (!this.selectedTaskTypeId) {
      alert('אנא בחר סוג משימה');
      return;
    }
  
    const cleanedItems = this.items.map(item => ({
      title: item.title,
      description: item.description || '', 
      orderIndex: item.orderIndex || item.order_index || 0, 
      isOptional: item.isOptional || item.is_optional || false
    }));
  
    const data: ChecklistTemplate = {
      taskTypeId: this.selectedTaskTypeId as number,
      items: cleanedItems
    };
  
    this.checklistService.saveTemplate(data).subscribe({
      next: () => {
        alert('התבנית נשמרה בהצלחה!');
      },
      error: (err) => {
        alert('שגיאה בשמירה: ' + (err.error?.errors?.["Items[0].Description"] || 'בדקי את הקונסול'));
      }
    });
  }

  
  
}

