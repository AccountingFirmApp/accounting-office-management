// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { Router } from '@angular/router';  // ← הוסף
// import { CompanyService } from '../../services/company';
// import { CompanyDto } from '../../models/Company';

// @Component({
//   selector: 'app-company-list',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './company-list.component.html',
//   styleUrls: ['./company-list.component.css']
// })
// export class CompanyListComponent implements OnInit {
//   companies: CompanyDto[] = [];
//   loading = false;
//   error: string | null = null;

//   constructor(
//     private companyService: CompanyService,
//     private router: Router  // ← הוסף
//   ) { }

//   ngOnInit(): void {
//     this.loadCompanies();
//   }

//   loadCompanies(): void {
//     this.loading = true;
//     this.error = null;
    
//     this.companyService.getAllCompanies().subscribe({
//       next: (data) => {
//         this.companies = data;
//         this.loading = false;
//       },
//       error: (err) => {
//         this.error = 'שגיאה בטעינת החברות';
//         this.loading = false;
//         console.error(err);
//       }
//     });
//   }

//   createCompany(): void {
//     this.router.navigate(['/Companies/create']);
//   }

//   editCompany(id: number): void {
//     this.router.navigate(['/Companies', id, 'edit']);
//   }

//   viewCompanyTasks(id: number): void {
//     this.router.navigate(['/Companies', id, 'tasks']);
//   }

//   deleteCompany(id: number): void {
//     if (confirm('האם אתה בטוח שברצונך למחוק חברה זו?')) {
//       this.companyService.deleteCompany(id).subscribe({
//         next: () => {
//           this.loadCompanies();
//         },
//         error: (err) => {
//           alert('שגיאה במחיקת החברה');
//           console.error(err);
//         }
//       });
//     }
//   }
// }
// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { Router } from '@angular/router';
// import { CompanyService } from '../../services/company';
// import { CompanyDto } from '../../models/Company';
// import { TaskDto } from '../../models/task';
// import { HttpErrorResponse } from '@angular/common/http';  // ← צריך

// @Component({
//   selector: 'app-company-list',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './company-list.component.html',
//   styleUrls: ['./company-list.component.css']
// })
// export class CompanyListComponent implements OnInit {
//   companies: CompanyDto[] = [];
//   loading = false;
//   error: string | null = null;

//   // נתונים של משימות
//   selectedCompanyTasks: TaskDto[] = [];
//   tasksLoading = false;
//   tasksError: string | null = null;
//   selectedCompanyName: string | null = null;

//   constructor(
//     private companyService: CompanyService,
//     private router: Router
//   ) { }

//   ngOnInit(): void {
//     this.loadCompanies();
//   }

//   loadCompanies(): void {
//     this.loading = true;
//     this.error = null;

//     this.companyService.getAllCompanies().subscribe({
//       next: (data) => {
//         this.companies = data;
//         this.loading = false;
//       },
//       error: (err: HttpErrorResponse) => {  // ← טיפוס ברור
//         this.error = 'שגיאה בטעינת החברות';
//         this.loading = false;
//         console.error(err);
//       }
//     });
//   }

//   createCompany(): void {
//     this.router.navigate(['/Companies/create']);
//   }

//   editCompany(id: number): void {
//     this.router.navigate(['/Companies', id, 'edit']);
//   }

//   viewCompanyTasks(companyId: number, companyName: string): void {
//     this.selectedCompanyTasks = [];
//     this.tasksLoading = true;
//     this.tasksError = null;
//     this.selectedCompanyName = companyName;

//     this.companyService.getTasksByCompanyId(companyId).subscribe({
//         next: (tasks: TaskDto[]) => {
//           this.selectedCompanyTasks = tasks;
//           this.tasksLoading = false;
//         },
//         error: (err: HttpErrorResponse) => {
//           this.tasksError = 'שגיאה בטעינת המשימות';
//           this.tasksLoading = false;
//           console.error(err);
//         }
//       });
//     }      

//   deleteCompany(id: number): void {
//     if (confirm('האם אתה בטוח שברצונך למחוק חברה זו?')) {
//       this.companyService.deleteCompany(id).subscribe({
//         next: () => {
//           this.loadCompanies();
//         },
//         error: (err: HttpErrorResponse) => {  // ← טיפוס ברור
//           alert('שגיאה במחיקת החברה');
//           console.error(err);
//         }
//       });
//     }
//   }
// }
// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { Router } from '@angular/router';
// import { CompanyService } from '../../services/company';
// import { CompanyDto } from '../../models/Company';

// @Component({
//   selector: 'app-company-list',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './company-list.component.html',
//   styleUrls: ['./company-list.component.css']
// })
// export class CompanyListComponent implements OnInit {
//   companies: CompanyDto[] = [];
//   loading = false;
//   error: string | null = null;

//   constructor(
//     private companyService: CompanyService,
//     private router: Router
//   ) { }

//   ngOnInit(): void {
//     this.loadCompanies();
//   }

//   loadCompanies(): void {
//     this.loading = true;
//     this.error = null;
    
//     this.companyService.getAllCompanies().subscribe({
//       next: (data) => {
//         this.companies = data;
//         this.loading = false;
//       },
//       error: (err) => {
//         this.error = 'שגיאה בטעינת החברות';
//         this.loading = false;
//         console.error(err);
//       }
//     });
//   }

//   // ← זה מנווט למשימות
//   viewCompanyTasks(id: number): void {
//     this.router.navigate(['/companies', id, 'tasks']);
//   }

//   // ← זה מנווט לעריכה
//   editCompany(id: number): void {
//     this.router.navigate(['/companies', id, 'edit']);
//   }

//   deleteCompany(id: number): void {
//     if (confirm('האם אתה בטוח שברצונך למחוק חברה זו?')) {
//       this.companyService.deleteCompany(id).subscribe({
//         next: () => {
//           this.loadCompanies();
//         },
//         error: (err) => {
//           alert('שגיאה במחיקת החברה');
//           console.error(err);
//         }
//       });
//     }
//   }
// }
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule ,Location} from '@angular/common';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company';
import { CompanyDto } from '../../models/Company';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  companies: CompanyDto[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private companyService: CompanyService,
    private router: Router,
    private cdr: ChangeDetectorRef , // ← הוסף את זה
    private location:Location,
    public auth: AuthService

  ) { 
    console.log('✅ CompanyListComponent נוצר');
  }

  ngOnInit(): void {
    console.log('✅ ngOnInit נקרא');
    this.loadCompanies();
  }

  loadCompanies(): void {
    console.log('🔄 מתחיל לטעון חברות...');
    this.loading = true;
    this.error = null;
    this.cdr.detectChanges(); // ← הוסף את זה
    
    this.companyService.getAllCompanies().subscribe({
      next: (data) => {
        console.log('✅ התקבלו חברות:', data);
        console.log('✅ מספר חברות:', data.length);
        this.companies = data;
        this.loading = false;
        this.cdr.detectChanges(); // ← הוסף את זה
        console.log('✅ loading =', this.loading);
        console.log('✅ companies.length =', this.companies.length);
      },
      error: (err) => {
        console.error('❌ שגיאה בטעינת חברות:', err);
        this.error = 'שגיאה בטעינת החברות';
        this.loading = false;
        this.cdr.detectChanges(); // ← הוסף את זה
      }
    });
  }

  viewCompanyTasks(id: number): void {
    console.log('🔄 מנווט למשימות של חברה:', id);
    this.router.navigate(['/companies', id, 'tasks']);
  }

  editCompany(id: number): void {
    console.log('🔄 מנווט לעריכת חברה:', id);
    this.router.navigate(['/companies', id, 'edit']);
  }

  deleteCompany(id: number): void {
    if (confirm('האם אתה בטוח שברצונך למחוק חברה זו?')) {
      this.companyService.deleteCompany(id).subscribe({
        next: () => {
          console.log('✅ חברה נמחקה בהצלחה');
          this.loadCompanies();
        },
        error: (err) => {
          console.error('❌ שגיאה במחיקת החברה:', err);
          alert('שגיאה במחיקת החברה');
        }
      });
    }
  }
addCompany(): void {
    this.router.navigate(['/companies/create']);
  }
  goHome(): void {
    this.router.navigate(['/home']);
  }
}