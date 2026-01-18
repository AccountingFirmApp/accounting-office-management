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
import { Observable } from 'rxjs';
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

  constructor(private http: HttpClient) { }

  // =========================
  // Auth API Calls
  // =========================
  login(request: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, request);
  }

  googleLogin(request: GoogleLoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login-google`, request);
  }

  // =========================
  // Token Management
  // =========================
  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    console.log('🔑 קבלת טוקן מה-localStorage1',localStorage.getItem('authToken')); 
    
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
    localStorage.removeItem('authToken');
    localStorage.removeItem('workerInfo');
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
  //   console.log('🔐 קבלת תפקיד משתמש מהטוקן:2', token);

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
    console.log('🔐 בדיקת Admin:3', this.getUserRole());

    return this.getUserRole() === 'Admin';
    
  }
}
