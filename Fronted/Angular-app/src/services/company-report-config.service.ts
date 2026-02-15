import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { CompanyReportConfigDto, CreateCompanyReportConfigDto, UpdateCompanyReportConfigDto } from '../models/company-report-config';

@Injectable({ providedIn: 'root' })
export class CompanyReportConfigService {
  private endpoint = '/company-report-configs';

  constructor(private api: ApiService) {}

  getAll(): Observable<CompanyReportConfigDto[]> {
    return this.api.get<CompanyReportConfigDto[]>(this.endpoint);
  }

  getByCompanyId(companyId: number): Observable<CompanyReportConfigDto[]> {
    return this.api.get<CompanyReportConfigDto[]>(`${this.endpoint}/company/${companyId}`);
  }

  getById(id: number): Observable<CompanyReportConfigDto> {
    return this.api.get<CompanyReportConfigDto>(`${this.endpoint}/${id}`);
  }

  create(config: CreateCompanyReportConfigDto): Observable<CompanyReportConfigDto> {
    return this.api.post<CompanyReportConfigDto>(this.endpoint, config);
  }

update(id: number, dto: any): Observable<CompanyReportConfigDto> {
  return this.api.patch<CompanyReportConfigDto>(`${this.endpoint}/${id}`, dto);
}

delete(id: number): Observable<void> {
  return this.api.delete<void>(`${this.endpoint}/${id}`);
}
  getFrequencies(): Observable<any[]> {
    return new Observable(observer => {
      observer.next([
        { id: 1, name: 'חודשי' },
        { id: 2, name: 'דו חודשי' },
        { id: 3, name: 'רבעוני' },
        { id: 4, name: 'שנתי' }
      ]);
      observer.complete();
    });
  }
}
