import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  ReportInstance,
  ReportInstanceDetail,
  CreateReportInstance,
  UpdateReportInstance,
  UpdateReportStatus,
  UpdateReportPayment,
  UpcomingReport
} from '../models/report-instance';
import { environment } from '../environments/environment';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private endpoint = '/reports';

  constructor(private api: ApiService) {}

 

  getAll(isAdminMode: boolean = false): Observable<ReportInstanceDetail[]> {
  const params = new HttpParams().set('isAdminMode', isAdminMode.toString());

  return this.api.get<ReportInstanceDetail[]>(
    `${this.endpoint}/all`,
    { params }
  );
}
 
  getById(id: number): Observable<ReportInstanceDetail> {
    return this.api.get<ReportInstanceDetail>(`${this.endpoint}/${id}`);
  }

  // ========== חיפושים לפי חברה ==========

  
  getByCompanyId(
    companyId: number,
    status?: string,
    fromPeriod?: Date,
    toPeriod?: Date
  ): Observable<ReportInstanceDetail[]> {
    let url = `${this.endpoint}/company/${companyId}`;
    const params: string[] = [];

    if (status) params.push(`status=${status}`);
    if (fromPeriod) params.push(`fromPeriod=${fromPeriod.toISOString()}`);
    if (toPeriod) params.push(`toPeriod=${toPeriod.toISOString()}`);

    if (params.length > 0) {
      url += `?${params.join('&')}`;
    }

    return this.api.get<ReportInstanceDetail[]>(url);
  }


  getByConfigId(configId: number): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/config/${configId}`);
  }

  // ========== חיפושים לפי סטטוס ==========

  
  getByStatus(status: string): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/status/${status}`);
  }

 
  getPending(): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/pending`);
  }

  // ========== חיפושים לפי תקופה ==========

  getByPeriod(year: number, month: number): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(
      `${this.endpoint}/period?year=${year}&month=${month}`
    );
  }

  
  getByDateRange(startDate: Date, endDate: Date): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(
      `${this.endpoint}/daterange?startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`
    );
  }

  // ========== דיווחים חשובים ומיוחדים ==========

 
  getUpcoming(companyId?: number, daysAhead: number = 30): Observable<UpcomingReport[]> {
    let url = `${this.endpoint}/upcoming?daysAhead=${daysAhead}`;
    if (companyId) {
      url += `&companyId=${companyId}`;
    }
    return this.api.get<UpcomingReport[]>(url);
  }

  
  getOverdue(): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/overdue`);
  }

 
  getReportsDueInDays(days: number = 7): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/due?days=${days}`);
  }

  // ========== כתיבה - יצירה ועדכון ==========

 
  create(report: CreateReportInstance): Observable<ReportInstance> {
    return this.api.post<ReportInstance>(this.endpoint, report);
  }

 
  update(id: number, report: UpdateReportInstance): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/${id}`, report);
  }

  
  updateStatus(dto: UpdateReportStatus): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/status`, dto);
  }

 
  updatePayment(dto: UpdateReportPayment): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/payment`, dto);
  }

  // ========== מחיקה ==========

 
  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  // ========== פונקציות עזר נוספות ==========

 
  getCurrentMonthReports(): Observable<ReportInstanceDetail[]> {
    const now = new Date();
    return this.getByPeriod(now.getFullYear(), now.getMonth() + 1);
  }

  
  getCurrentYearReports(): Observable<ReportInstanceDetail[]> {
    const now = new Date();
    const startDate = new Date(now.getFullYear(), 0, 1);
    const endDate = new Date(now.getFullYear(), 11, 31);
    return this.getByDateRange(startDate, endDate);
  }

  
  getNextWeekReports(): Observable<ReportInstanceDetail[]> {
    return this.getReportsDueInDays(7);
  }

  /**
   * קבלת דיווחים לחודש הקרוב
   */
  getNextMonthReports(): Observable<ReportInstanceDetail[]> {
    return this.getReportsDueInDays(30);
  }

  // ========== מתודות חדשות לטופס ==========

  
  getCompanies(): Observable<any[]> {
    return this.api.get<any[]>('/companies');
  }

  
  getReportTypes(): Observable<any[]> {
    return this.api.get<any[]>('/report-types');
  }

 
  getConfigs(): Observable<any[]> {
    return this.api.get<any[]>('/company-report-configs');
  }
}