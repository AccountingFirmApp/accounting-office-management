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

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private endpoint = '/reports';

  constructor(private api: ApiService) {}

  // ========== קריאה - פונקציות בסיסיות ==========

  /**
   * קבלת כל הדיווחים במערכת
   * GET: /api/reports/all
   */
  getAll(): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/all`);
  }

  /**
   * קבלת דיווח לפי ID
   * GET: /api/reports/{id}
   */
  getById(id: number): Observable<ReportInstanceDetail> {
    return this.api.get<ReportInstanceDetail>(`${this.endpoint}/${id}`);
  }

  // ========== חיפושים לפי חברה ==========

  /**
   * קבלת דיווחים של חברה עם אפשרות לסינון
   * GET: /api/reports/company/{companyId}?status=...&fromPeriod=...&toPeriod=...
   */
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

  /**
   * קבלת דיווחים לפי Config ID
   * GET: /api/reports/config/{configId}
   */
  getByConfigId(configId: number): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/config/${configId}`);
  }

  // ========== חיפושים לפי סטטוס ==========

  /**
   * קבלת דיווחים לפי סטטוס
   * GET: /api/reports/status/{status}
   */
  getByStatus(status: string): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/status/${status}`);
  }

  /**
   * קבלת דיווחים ממתינים (Pending)
   * GET: /api/reports/pending
   */
  getPending(): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/pending`);
  }

  // ========== חיפושים לפי תקופה ==========

  /**
   * קבלת דיווחים לפי תקופה (חודש/שנה)
   * GET: /api/reports/period?year=2024&month=1
   */
  getByPeriod(year: number, month: number): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(
      `${this.endpoint}/period?year=${year}&month=${month}`
    );
  }

  /**
   * קבלת דיווחים בטווח תאריכים
   * GET: /api/reports/daterange?startDate=...&endDate=...
   */
  getByDateRange(startDate: Date, endDate: Date): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(
      `${this.endpoint}/daterange?startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`
    );
  }

  // ========== דיווחים חשובים ומיוחדים ==========

  /**
   * קבלת דיווחים קרובים/באיחור
   * GET: /api/reports/upcoming?daysAhead=30&companyId=...
   */
  getUpcoming(companyId?: number, daysAhead: number = 30): Observable<UpcomingReport[]> {
    let url = `${this.endpoint}/upcoming?daysAhead=${daysAhead}`;
    if (companyId) {
      url += `&companyId=${companyId}`;
    }
    return this.api.get<UpcomingReport[]>(url);
  }

  /**
   * קבלת דיווחים באיחור (OVERDUE) - קריטי!
   * GET: /api/reports/overdue
   */
  getOverdue(): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/overdue`);
  }

  /**
   * קבלת דיווחים שצריך להגיש בעוד X ימים
   * GET: /api/reports/due?days=7
   * דוגמה: getReportsDueInDays(7) - דיווחים לשבוע הקרוב
   */
  getReportsDueInDays(days: number = 7): Observable<ReportInstanceDetail[]> {
    return this.api.get<ReportInstanceDetail[]>(`${this.endpoint}/due?days=${days}`);
  }

  // ========== כתיבה - יצירה ועדכון ==========

  /**
   * יצירת דיווח חדש
   * POST: /api/reports
   */
  create(report: CreateReportInstance): Observable<ReportInstance> {
    return this.api.post<ReportInstance>(this.endpoint, report);
  }

  /**
   * עדכון מלא של דיווח
   * PUT: /api/reports/{id}
   */
  update(id: number, report: UpdateReportInstance): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/${id}`, report);
  }

  /**
   * עדכון סטטוס דיווח
   * PUT: /api/reports/status
   */
  updateStatus(dto: UpdateReportStatus): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/status`, dto);
  }

  /**
   * עדכון תשלום דיווח
   * PUT: /api/reports/payment
   */
  updatePayment(dto: UpdateReportPayment): Observable<void> {
    return this.api.put<void>(`${this.endpoint}/payment`, dto);
  }

  // ========== מחיקה ==========

  /**
   * מחיקת דיווח
   * DELETE: /api/reports/{id}
   * ⚠️ שים לב: לא קיים ב-Controller הנוכחי, תצטרך להוסיף אם צריך
   */
  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  // ========== פונקציות עזר נוספות ==========

  /**
   * קבלת דיווחים של החודש הנוכחי
   */
  getCurrentMonthReports(): Observable<ReportInstanceDetail[]> {
    const now = new Date();
    return this.getByPeriod(now.getFullYear(), now.getMonth() + 1);
  }

  /**
   * קבלת דיווחים של השנה הנוכחית
   */
  getCurrentYearReports(): Observable<ReportInstanceDetail[]> {
    const now = new Date();
    const startDate = new Date(now.getFullYear(), 0, 1);
    const endDate = new Date(now.getFullYear(), 11, 31);
    return this.getByDateRange(startDate, endDate);
  }

  /**
   * קבלת דיווחים לשבוע הקרוב
   */
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

  /**
   * קבלת רשימת כל החברות
   * GET: /api/companies
   */
  getCompanies(): Observable<any[]> {
    return this.api.get<any[]>('/companies');
  }

  /**
   * קבלת רשימת כל סוגי הדיווחים
   * GET: /api/report-types
   */
  getReportTypes(): Observable<any[]> {
    return this.api.get<any[]>('/report-types');
  }

  /**
   * קבלת רשימת כל הקונפיגורציות (שילובי חברה + סוג דיווח)
   * GET: /api/company-report-configs
   */
  getConfigs(): Observable<any[]> {
    return this.api.get<any[]>('/company-report-configs');
  }
}