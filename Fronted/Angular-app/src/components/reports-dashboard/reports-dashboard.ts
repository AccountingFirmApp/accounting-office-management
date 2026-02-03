// // import { Component, OnInit } from '@angular/core';
// // import { Router, RouterOutlet } from '@angular/router';
// // import { ReportService } from '../../services/report';
// // import { CommonModule } from '@angular/common';
// // import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
// // import { AuthService } from '../../services/auth.service';

// // @Component({
// //   standalone: true,
// //   selector: 'app-reports-dashboard',
// //   templateUrl: './reports-dashboard.html',
// //   styleUrls: ['./reports-dashboard.css'],
// //   imports: [RouterOutlet, CommonModule, BackButtonComponent]
// // })
// // export class ReportsDashboardComponent implements OnInit {
  
// //   stats = {
// //     pending: 0,
// //     overdue: 0,
// //     reported: 0,
// //     paid: 0
// //   };

// //   constructor(
// //     private router: Router,
// //     private reportService: ReportService,
// //     public auth: AuthService
// //   ) {}

// //   ngOnInit() {
// //     this.loadStatistics();
// //   }

// //   loadStatistics() {
// //     // נטען סטטיסטיקות מה-API
// //     this.reportService.getAll().subscribe({
// //       next: (reports) => {
// //         this.stats.pending = reports.filter(r => r.status === 'Pending').length;
// //         this.stats.reported = reports.filter(r => r.status === 'Reported').length;
// //         this.stats.paid = reports.filter(r => r.status === 'Paid').length;
        
// //         // חישוב דוחות באיחור
// //         const today = new Date();
// //         this.stats.overdue = reports.filter(r => {
// //           return (r.status === 'Pending' || r.status === 'Reported') &&
// //                  new Date(r.period) < today;
// //         }).length;
// //       },
// //       error: (err) => {
// //         console.error('Error loading stats:', err);
// //         // לא נציג alert כדי לא להפריע למשתמש
// //       }
// //     });
// //   }

// //   goBack() {
// //     this.router.navigate(['/']);
// //   }

// //   createNewReport() {
// //     this.router.navigate(['/reports/new']);
// //   }
// // }




// import { Component, OnInit } from '@angular/core';
// import { Router, RouterOutlet, ActivatedRoute } from '@angular/router'; // ← הוסף ActivatedRoute
// import { ReportService } from '../../services/report';
// import { CommonModule } from '@angular/common';
// import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
// import { AuthService } from '../../services/auth.service';

// @Component({
//   standalone: true,
//   selector: 'app-reports-dashboard',
//   templateUrl: './reports-dashboard.html',
//   styleUrls: ['./reports-dashboard.css'],
//   imports: [RouterOutlet, CommonModule, BackButtonComponent]
// })
// export class ReportsDashboardComponent implements OnInit {
  
//   stats = {
//     pending: 0,
//     overdue: 0,
//     reported: 0,
//     paid: 0
//   };

//   // ← הוסף משתנה לשמירת companyId
//   filterByCompanyId: number | null = null;
//   companyName: string = '';

//   constructor(
//     private router: Router,
//     private route: ActivatedRoute, // ← הוסף
//     private reportService: ReportService,
//     public auth: AuthService
//   ) {}

//   ngOnInit() {
//     // ← קרא את companyId מה-query params
//     this.route.queryParams.subscribe(params => {
//       this.filterByCompanyId = params['companyId'] ? +params['companyId'] : null;
//       this.loadStatistics();
//     });
//   }

//   loadStatistics() {
//     this.reportService.getAll().subscribe({
//       next: (reports) => {
//         // ← פילטר לפי companyId אם קיים
//         let filteredReports = reports;
        
//         if (this.filterByCompanyId) {
//           filteredReports = reports.filter(r => r.companyId === this.filterByCompanyId);
          
//           // שמור את שם החברה להצגה
//           if (filteredReports.length > 0) {
//             this.companyName = filteredReports[0].companyName;
//           }
//         }

//         // חשב סטטיסטיקות על הדוחות המפולטרים
//         this.stats.pending = filteredReports.filter(r => r.status === 'Pending').length;
//         this.stats.reported = filteredReports.filter(r => r.status === 'Reported').length;
//         this.stats.paid = filteredReports.filter(r => r.status === 'Paid').length;
        
//         // חישוב דוחות באיחור
//         const today = new Date();
//         this.stats.overdue = filteredReports.filter(r => {
//           return (r.status === 'Pending' || r.status === 'Reported') &&
//                  new Date(r.period) < today;
//         }).length;
//       },
//       error: (err) => {
//         console.error('Error loading stats:', err);
//       }
//     });
//   }

//   goBack() {
//     this.router.navigate(['/']);
//   }

//   createNewReport() {
//     this.router.navigate(['/reports/new']);
//   }
// }

import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet, ActivatedRoute } from '@angular/router';
import { ReportService } from '../../services/report';
import { CommonModule } from '@angular/common';
import { BackButtonComponent } from '../../app/components/shared/back-button/back-button.component';
import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-reports-dashboard',
  templateUrl: './reports-dashboard.html',
  styleUrls: ['./reports-dashboard.css'],
  imports: [RouterOutlet, CommonModule, BackButtonComponent]
})
export class ReportsDashboardComponent implements OnInit {
  
  stats = {
    pending: 0,
    overdue: 0,
    reported: 0,
    paid: 0
  };

  filterByCompanyId: number | null = null;
  companyName: string = '';
  isAdminMode: boolean = false; // 🆕 הוסף את זה

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private reportService: ReportService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    // 🔥 קרא גם את adminMode וגם את companyId
    this.route.queryParams.subscribe(params => {
      this.filterByCompanyId = params['companyId'] ? +params['companyId'] : null;
      this.isAdminMode = params['adminMode'] === 'true'; // 🆕 הוסף את זה
      
      console.log('📊 Dashboard params:', {
        companyId: this.filterByCompanyId,
        adminMode: this.isAdminMode
      });
      
      this.loadStatistics();
    });
  }

  loadStatistics() {
    // 🔥 העבר את isAdminMode ל-getAll
    this.reportService.getAll(this.isAdminMode).subscribe({
      next: (reports) => {
        console.log('📊 Dashboard קיבל:', reports.length, 'דוחות');
        
        // פילטר לפי companyId אם קיים
        let filteredReports = reports;
        
        if (this.filterByCompanyId) {
          filteredReports = reports.filter(r => r.companyId === this.filterByCompanyId);
          console.log('📊 אחרי פילטור לפי חברה:', filteredReports.length);
          
          // שמור את שם החברה להצגה
          if (filteredReports.length > 0) {
            this.companyName = filteredReports[0].companyName;
          }
        }

        // חשב סטטיסטיקות על הדוחות המפולטרים
        this.stats.pending = filteredReports.filter(r => r.status === 'Pending').length;
        this.stats.reported = filteredReports.filter(r => r.status === 'Reported').length;
        this.stats.paid = filteredReports.filter(r => r.status === 'Paid').length;
        
        // חישוב דוחות באיחור
        const today = new Date();
        this.stats.overdue = filteredReports.filter(r => {
          return (r.status === 'Pending' || r.status === 'Reported') &&
                 new Date(r.period) < today;
        }).length;
        
        console.log('📊 סטטיסטיקות:', this.stats);
      },
      error: (err) => {
        console.error('❌ Error loading stats:', err);
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }

  createNewReport() {
    this.router.navigate(['/reports/new']);
  }
}