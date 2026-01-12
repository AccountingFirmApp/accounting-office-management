import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequestDto, LoginResponseDto } from '../models/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7118/api/auth';

  constructor(private http: HttpClient) { }

  login(request: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, request);
  }

  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  saveWorkerInfo(worker: any): void {
    localStorage.setItem('workerInfo', JSON.stringify(worker));
  }

  getWorkerInfo(): any {
    const workerInfo = localStorage.getItem('workerInfo');
    return workerInfo ? JSON.parse(workerInfo) : null;
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('workerInfo');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
