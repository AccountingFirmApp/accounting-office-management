import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyTaskConfigDto } from '../models/auth';
import { environment } from '../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class TaskConfigsService {
  private apiUrl = `${environment.apiUrl}/TaskConfigurations`;

  constructor(private http: HttpClient) { }

  getTaskMatrix(): Observable<CompanyTaskConfigDto[]> {
    return this.http.get<CompanyTaskConfigDto[]>(`${this.apiUrl}/matrix`);
  }
  saveTaskConfig(config: CompanyTaskConfigDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/save`, config);
  }
}