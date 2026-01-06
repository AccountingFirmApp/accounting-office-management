import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkerCompanies } from '../models/worker-companies';

@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  private apiUrl = 'https://localhost:5001/api/workers'; // ⬅️ שנה את הפורט אם צריך

  constructor(private http: HttpClient) { }

  getWorkerCompanies(workerId: number): Observable<WorkerCompanies> {
    return this.http.get<WorkerCompanies>(`${this.apiUrl}/${workerId}/companies`);
  }
}