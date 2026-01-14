// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { CompanyDto, CreateCompanyCommand, UpdateCompanyCommand, CompanyWithPendingReportsDto } from '../models/Company';
// import { TaskDto } from '../models/task';
// import { environment } from '../environments/environment';

// @Injectable({
//   providedIn: 'root'
// })
// export class CompanyService {
// //   getTasksByCompanyId(companyId: number) {
// //       throw new Error('Method not implemented.');
// //   }
//   private apiUrl = `${environment.apiUrl}/api/Companies`; // התאם לנתיב שלך

//   constructor(private http: HttpClient) { }

//   // קבלת כל החברות
//   getAllCompanies(): Observable<CompanyDto[]> {
//     return this.http.get<CompanyDto[]>(this.apiUrl);
//   }

//   // קבלת חברה לפי ID
//   getCompanyById(id: number): Observable<CompanyDto> {
//     return this.http.get<CompanyDto>(`${this.apiUrl}/${id}`);
//   }

//   // קבלת חברות לפי FirmId
//   getCompaniesByFirmId(firmId: number): Observable<CompanyDto[]> {
//     return this.http.get<CompanyDto[]>(`${this.apiUrl}/firm/${firmId}`);
//   }

//   // קבלת חברות עם דוחות ממתינים
//   getCompaniesByFirmIdWithReports(firmId: number): Observable<CompanyWithPendingReportsDto[]> {
//     return this.http.get<CompanyWithPendingReportsDto[]>(`${this.apiUrl}/firm/${firmId}/with-reports`);
//   }

//   // יצירת חברה חדשה
//   createCompany(command: CreateCompanyCommand): Observable<CompanyDto> {
//     return this.http.post<CompanyDto>(this.apiUrl, command);
//   }

//   // עדכון חברה
//   updateCompany(id: number, command: UpdateCompanyCommand): Observable<CompanyDto> {
//     return this.http.put<CompanyDto>(`${this.apiUrl}/${id}`, command);
//   }

//   // מחיקת חברה
//   deleteCompany(id: number): Observable<void> {
//     return this.http.delete<void>(`${this.apiUrl}/${id}`);
//   }
// //   getTasksByCompanyId(companyId: number): Observable<TaskDto[]> {
// //     return this.http.get<TaskDto[]>(`${this.apiUrl}/${companyId}/tasks`);
// //   // קבלת משימות של חברה
// //   getTasksByCompanyId(companyId: number): Observable<TaskDto[]> {
// //     return this.http.get<TaskDto[]>(`${this.apiUrl}/${companyId}/tasks`);
// //   }

// getTasksByCompanyId(companyId: number): Observable<TaskDto[]> {
//     return this.http.get<TaskDto[]>(`${this.apiUrl}/${companyId}/tasks`);
//   }
  
// }   
// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { CompanyDto, CreateCompanyCommand, UpdateCompanyCommand } from '../models/Company';
// import { TaskDto } from '../models/task'
// import { environment } from '../environments/environment';

// @Injectable({
//   providedIn: 'root'
// })
// export class CompanyService {
//   private apiUrl = `${environment.apiUrl}/api/Companies`;  // או

//   constructor(private http: HttpClient) { }

//   getAllCompanies(): Observable<CompanyDto[]> {
//     console.log('🔍 קריאה ל-API:', `${this.apiUrl}`);
//     return this.http.get<CompanyDto[]>(this.apiUrl);
//   }

//   getCompanyById(id: number): Observable<CompanyDto> {
//     console.log('🔍 קריאה ל-API:', `${this.apiUrl}/${id}`);
//     return this.http.get<CompanyDto>(`${this.apiUrl}/${id}`);
//   }

//   getCompaniesByFirmId(firmId: number): Observable<CompanyDto[]> {
//     return this.http.get<CompanyDto[]>(`${this.apiUrl}/firm/${firmId}`);
//   }

//   createCompany(command: CreateCompanyCommand): Observable<CompanyDto> {
//     return this.http.post<CompanyDto>(this.apiUrl, command);
//   }

//   updateCompany(id: number, command: UpdateCompanyCommand): Observable<CompanyDto> {
//     return this.http.put<CompanyDto>(`${this.apiUrl}/${id}`, command);
//   }

//   deleteCompany(id: number): Observable<void> {
//     return this.http.delete<void>(`${this.apiUrl}/${id}`);
//   }

//   // ← זה החשוב! התקן את ה-endpoint
//   getTasksByCompanyId(companyId: number): Observable<TaskDto[]> {
//     const url = `${this.apiUrl}/${companyId}/tasks`;
//     console.log('🔍 קריאה למשימות:', url);
//     return this.http.get<TaskDto[]>(url);
//   }
// }
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyDto, CreateCompanyCommand, UpdateCompanyCommand } from '../models/Company';
import { TaskDto } from '../models/task';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'https://localhost:7118/api/companies';

  constructor(private http: HttpClient) { }

  getAllCompanies(): Observable<CompanyDto[]> {
    console.log('🔍 קריאה ל-API:', this.apiUrl);
    return this.http.get<CompanyDto[]>(this.apiUrl);
  }

  getCompanyById(id: number): Observable<CompanyDto> {
    console.log('🔍 קריאה ל-API:', `${this.apiUrl}/${id}`);
    return this.http.get<CompanyDto>(`${this.apiUrl}/${id}`);
  }

  createCompany(command: CreateCompanyCommand): Observable<CompanyDto> {
    return this.http.post<CompanyDto>(this.apiUrl, command);
  }

  updateCompany(id: number, command: UpdateCompanyCommand): Observable<CompanyDto> {
    return this.http.put<CompanyDto>(`${this.apiUrl}/${id}`, command);
  }

  deleteCompany(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getTasksByCompanyId(companyId: number): Observable<TaskDto[]> {
    const url = `${this.apiUrl}/${companyId}/tasks`;
    console.log('🔍 קריאה למשימות:', url);
    return this.http.get<TaskDto[]>(url);
  }

  // ← הפונקציה החדשה!
  updateTaskStatus(companyId: number, taskId: number, status: string): Observable<any> {
    const url = `${this.apiUrl}/${companyId}/tasks/${taskId}/status`;
    console.log('🔄 עדכון סטטוס:', url, status);
    return this.http.patch(url, { status });
  }
}