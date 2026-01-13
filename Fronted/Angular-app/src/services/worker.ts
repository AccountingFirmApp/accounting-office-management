import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkerCompanies } from '../models/worker-companies';
import { WorkerInfoDto } from '../models/auth';

@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  private apiUrl = 'https://localhost:7118/api/workers';
  currentWorker!:WorkerInfoDto;
  constructor(private http: HttpClient) { }

  // מחזיר מערך של CompanyDto
  getWorkerCompanies(workerId: number): Observable<WorkerCompanies> {
    return this.http.get<WorkerCompanies>(`${this.apiUrl}/${workerId}/companies`);
  }
}