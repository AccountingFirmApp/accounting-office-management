import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { TaskDetail } from '../models/auth';

@Injectable({
  providedIn: 'root'
})
export class CompantTaskService {
  private apiUrl = 'https://localhost:7118/api/CompanyTask';
  constructor(private http: HttpClient, private authService: AuthService) { }


getTaskById(id: number): Observable<TaskDetail> {
  return this.http.get<TaskDetail>(`${this.apiUrl}/${id}`);
}

// סימון פריט כהושלם
completeItem(itemId: number, workerId: number, notes?: string): Observable<any> {
  return this.http.patch(`${this.apiUrl}/checklist-item/${itemId}/complete`, {
    workerId,
    notes
  });
}

// ביטול סימון
uncompleteItem(itemId: number): Observable<any> {
  return this.http.patch(`${this.apiUrl}/checklist-item/${itemId}/uncomplete`, {});
}
// בתוך CompanyService
getTaskDetails(taskId: number): Observable<any> {
  return this.http.get(`${this.apiUrl}/${taskId}`);
}

// בתוך ה-Service (למשל company.service.ts)

toggleChecklistItem(itemId: number, isCompleted: boolean, workerId: number): Observable<any> {
  // כתובת הבסיס ללא המילה tasks
  
  // בניית הפעולה (complete או uncomplete)
  const action = isCompleted ? 'uncomplete' : 'complete';
  
  // בניית ה-URL המדויק
  const url = `${this.apiUrl}/checklist-item/${itemId}/${action}`;
  
  console.log('Sending request to:', url); // זה יעזור לך לראות ב-Console שהכתובת תוקנה

  if (!isCompleted) {
    // סימון כבוצע - שולחים Body עם WorkerId
    return this.http.patch(url, { workerId: workerId });
  } else {
    // ביטול סימון - שולחים Body ריק (או בלי Body, תלוי במימוש)
    return this.http.patch(url, {});
  }
}
}



