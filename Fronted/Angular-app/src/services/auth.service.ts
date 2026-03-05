
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequestDto, LoginResponseDto, GoogleLoginRequestDto } from '../models/auth';
import { envConfig } from '../app/app.config.env.example';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  role?: string;
  roles?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${envConfig.apiUrl}/auth`;
  private readonly TOKEN_KEY = 'authToken';

  constructor(private http: HttpClient) { }

  // =========================
  // Auth API Calls
  // =========================
  login(request: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          this.saveToken(response.token); // 🔥 שמירה אוטומטית
          this.saveWorkerInfo(response.worker);
        })
      );
  }

  /**
   * התחברות Google
   */
  googleLogin(request: GoogleLoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login-google`, request)
      .pipe(
        tap(response => {
          this.saveToken(response.token); // 🔥 שמירה אוטומטית
          this.saveWorkerInfo(response.worker); 
        })
      );
  }

  // =========================
  // Token Management
  // =========================
  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  /**
   * קבלת טוקן
   */
  getToken(): string | null {
    
    return localStorage.getItem('authToken');
  }

  // =========================
  // Worker Info Storage
  // =========================
  saveWorkerInfo(worker: any): void {
    localStorage.setItem('workerInfo', JSON.stringify(worker));
  }

  getWorkerInfo(): any {
    const workerInfo = localStorage.getItem('workerInfo');
    return workerInfo ? JSON.parse(workerInfo) : null;
  }

  // =========================
  // Logout
  // =========================
  
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem('workerInfo'); // ⭐ הוסף את זה!
  }
  // =========================
  // Authentication Helpers
  // =========================
  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
  
    try {
      const decoded = jwtDecode<any>(token);
  
      // בדיקה לפי השדה של Microsoft Identity
      const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      return role ?? null;
  
    } catch {
      return null;
    }
  }
  
  isAdmin(): boolean {

    return this.getUserRole() === 'Admin';
    
  }
}
