import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountingFirmService {

  // כתובת ה-API – להתאים לשרת שלך
  private apiUrl = 'https://localhost:7118/api/accountingFirm';
  constructor(private http: HttpClient) {}

  // הבאת כל המשרדים
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // (אופציונלי) הבאת משרד לפי id
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}

// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class AccountingFirmService {

//   private apiUrl = 'https://localhost:5001/api/accountingFirm';

//   constructor(private http: HttpClient) {}

//   // טעינה חלקית (pagination)
//   getPage(page: number, pageSize: number): Observable<any[]> {
//     return this.http.get<any[]>(
//       `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
//     );
//   }

//   // אופציונלי – חיפוש
//   search(term: string): Observable<any[]> {
//     return this.http.get<any[]>(
//       `${this.apiUrl}/search?term=${term}`
//     );
//   }
// }