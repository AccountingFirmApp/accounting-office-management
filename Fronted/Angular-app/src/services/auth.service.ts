// services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequestDto, LoginResponseDto, GoogleLoginRequestDto } from '../models/auth';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  private readonly TOKEN_KEY = 'authToken';

  constructor(private http: HttpClient) { }

  /**
   * התחברות רגילה
   */
  login(request: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          this.saveToken(response.token); // 🔥 שמירה אוטומטית
          console.log('✅ Token saved:', response.token.substring(0, 20) + '...');
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
          console.log('✅ Token saved:', response.token.substring(0, 20) + '...');
        })
      );
  }

  /**
   * שמירת טוקן
   */
  private saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  /**
   * קבלת טוקן
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  /**
   * התנתקות
   */
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  /**
   * בדיקה האם מחובר
   */
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}