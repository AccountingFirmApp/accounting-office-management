
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyDto, CreateCompanyCommand, UpdateCompanyCommand } from '../models/Company';
import { TaskcompanyDto } from '../models/taskcompany';
import { AuthService } from './auth.service';
@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'https://localhost:7118/api/companies';
  constructor(private http: HttpClient, private authService: AuthService) { }
  getAllCompanies(isActive: boolean = true): Observable<CompanyDto[]> {
    const token = this.authService.getToken();
  
    return this.http.get<CompanyDto[]>(`${this.apiUrl}?isActive=${isActive}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  getCompanyById(id: number): Observable<CompanyDto> {
    return this.http.get<CompanyDto>(`${this.apiUrl}/${id}`);
  }
createCompany(command: CreateCompanyCommand): Observable<CompanyDto> {
    const token = this.authService.getToken();
    return this.http.post<CompanyDto>(this.apiUrl, command, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  updateCompany(id: number, command: UpdateCompanyCommand): Observable<CompanyDto> {
    const token = this.authService.getToken();
    return this.http.put<CompanyDto>(`${this.apiUrl}/${id}`, command, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  deleteCompany(id: number): Observable<void> {
    const token = this.authService.getToken();
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  getTasksByCompanyId(companyId: number): Observable<TaskcompanyDto[]> {
    const url = `${this.apiUrl}/${companyId}/tasks`;
    return this.http.get<TaskcompanyDto[]>(url);
  }

  updateTaskStatus(companyId: number, taskId: number, status: string): Observable<any> {
    const url = `${this.apiUrl}/${companyId}/tasks/${taskId}/status`;
    return this.http.patch(url, { status });
  }
  addCompany(command: CreateCompanyCommand): Observable<CompanyDto> {
    const token = this.authService.getToken();
  
    return this.http.post<CompanyDto>(this.apiUrl, command, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  
}