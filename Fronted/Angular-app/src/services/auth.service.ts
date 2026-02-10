// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { LoginRequestDto, LoginResponseDto, GoogleLoginRequestDto } from '../models/auth';
// import { environment } from '../environments/environment';
// import jwtDecode from 'jwt-decode';

// interface JwtPayload {
//   role?: string;
//   roles?: string[];
// }


// @Injectable({
//   providedIn: 'root'
// })
// export class AuthService {
//   private apiUrl = `${environment.apiUrl}/auth`;

//   constructor(private http: HttpClient) { }

//   login(request: LoginRequestDto): Observable<LoginResponseDto> {
//     return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, request);
//   }

//   googleLogin(request: GoogleLoginRequestDto): Observable<LoginResponseDto> {
//     return this.http.post<LoginResponseDto>(`${this.apiUrl}/login-google`, request);
//   }

//   saveToken(token: string): void {
//     localStorage.setItem('authToken', token);
//   }

//   getToken(): string | null {
//     return localStorage.getItem('authToken');
//   }

//   saveWorkerInfo(worker: any): void {
//     localStorage.setItem('workerInfo', JSON.stringify(worker));
//   }

//   getWorkerInfo(): any {
//     const workerInfo = localStorage.getItem('workerInfo');
//     return workerInfo ? JSON.parse(workerInfo) : null;
//   }

//   logout(): void {
//     localStorage.removeItem('authToken');
//     localStorage.removeItem('workerInfo');
//   }

//   isAuthenticated(): boolean {
//     return !!this.getToken();
//   }
// }
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequestDto, LoginResponseDto, GoogleLoginRequestDto } from '../models/auth';
import { environment } from '../environments/environment';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  role?: string;
  roles?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
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

  // =========================
  // Role / Admin Helpers
  // =========================
  // getUserRole(): string | null {
  //   const token = this.getToken();
  //   if (!token) return null;

  //   try {
  //     const decoded = jwtDecode<JwtPayload>(token);

  //     if (decoded.role) {
  //       return decoded.role;
  //     }

  //     if (decoded.roles && decoded.roles.length > 0) {
  //       return decoded.roles[0];
  //     }

  //     return null;
  //   } catch {
  //     return null;
  //   }
  // }
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
