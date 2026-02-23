import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChecklistTemplate } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class ChecklistService {
  private apiUrl = 'https://localhost:7118/api/ChecklistTemplates/template';
  constructor(private http: HttpClient) {}

  getTemplate(taskTypeId: number) {
    return this.http.get<ChecklistTemplate>(`https://localhost:7118/api/ChecklistTemplates/template/${taskTypeId}`);
    }
  saveTemplate(template: ChecklistTemplate) {
    return this.http.post(this.apiUrl, template);
  }
  getTaskTypes() {
    return this.http.get<any[]>('https://localhost:7118/api/companies/task-types');
  }

  
}
