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

completeItem(itemId: number, workerId: number, notes?: string): Observable<any> {
  return this.http.patch(`${this.apiUrl}/checklist-item/${itemId}/complete`, {
    workerId,
    notes
  });
}

uncompleteItem(itemId: number): Observable<any> {
  return this.http.patch(`${this.apiUrl}/checklist-item/${itemId}/uncomplete`, {});
}
getTaskDetails(taskId: number): Observable<any> {
  return this.http.get(`${this.apiUrl}/${taskId}`);
}



toggleChecklistItem(itemId: number, isCompleted: boolean, workerId: number): Observable<any> {

  
  
  const action = isCompleted ? 'uncomplete' : 'complete';
  
  
  const url = `${this.apiUrl}/checklist-item/${itemId}/${action}`;
  


  if (!isCompleted) {
    return this.http.patch(url, { workerId: workerId });
  } else {
    return this.http.patch(url, {});
  }
}

}



