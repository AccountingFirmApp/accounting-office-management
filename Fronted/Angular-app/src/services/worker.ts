import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkerCompanies } from '../models/worker-companies';
import { WorkerInfoDto, WorkerTask } from '../models/auth';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  private apiUrl = 'https://localhost:7118/api/workers';
  currentWorker!: WorkerInfoDto;

  constructor(private http: HttpClient, private authService: AuthService) { }

  getWorkerCompanies(workerId: number): Observable<WorkerCompanies> {
    return this.http.get<WorkerCompanies>(`${this.apiUrl}/${workerId}/companies`);
  }

  getallWorkers(): Observable<WorkerInfoDto[]> {
    const token = this.authService.getToken();
    return this.http.get<WorkerInfoDto[]>(this.apiUrl, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  deleteWorker(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateWorker(id: number, worker: WorkerInfoDto): Observable<WorkerInfoDto> {
    return this.http.put<WorkerInfoDto>(`${this.apiUrl}/${id}`, worker);
  }

  addWorker(worker: WorkerInfoDto): Observable<WorkerInfoDto> {
    return this.http.post<WorkerInfoDto>(this.apiUrl, worker);
  }

  getWorkerById(id: number): Observable<WorkerInfoDto> {
    const token = this.authService.getToken();
    return this.http.get<WorkerInfoDto>(`${this.apiUrl}/${id}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  getInactiveWorkers(): Observable<WorkerInfoDto[]> {
    const token = this.authService.getToken();
    return this.http.get<WorkerInfoDto[]>(`${this.apiUrl}?isActive=false`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
getCurrentWorker(): WorkerInfoDto | null {
  return this.currentWorker;
}
  // ✅ הוחזרה פנימה ל-class
  getCurrentWorkerId(): number | null {
    const worker = this.currentWorker;
    return worker ? worker.id : null;
  }

  getWorkerTasks(workerId: number): Observable<WorkerTask[]> {
    return this.http.get<WorkerTask[]>(`${this.apiUrl}/${workerId}/tasks`);
  }

  getWorkersbyCompany(companyId: number): Observable<WorkerInfoDto[]> {
    return this.http.get<WorkerInfoDto[]>(`${this.apiUrl}/by-company/${companyId}`);
  }
}