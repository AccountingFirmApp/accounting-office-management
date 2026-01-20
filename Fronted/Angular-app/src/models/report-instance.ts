// // import { PaymentMethod, ReportStatus } from './enums';

// // export interface ReportInstance {
// //   id: number;
// //   configId: number;
// //   period: Date;
// //   amount?: number;
// //   status: ReportStatus;
// //   paymentMethod?: PaymentMethod;
// //   receiptDate?: Date;
// //   reportedDate?: Date;
// //   paidDate?: Date;
// //   comments: string;
// //   createdAt?: Date;
// //   updatedAt?: Date;
// // }

// // export interface ReportInstanceDetail extends ReportInstance {
// //   companyId: number;
// //   companyName: string;
// //   companyTaxId: string;
// //   reportTypeId: number;
// //   reportTypeName: string;
// //   reportTypeShortCode: string;
// //   frequencyName: string;
// //   dayOfMonth?: number;
// //   periodFormatted: string;
// //   daysOverdue?: number;
// //   isOverdue: boolean;
// //   daysUntilDue?: number;
// // }

// // export interface CreateReportInstance {
// //   configId: number;
// //   period: Date;
// //   amount?: number;
// //   paymentMethod?: PaymentMethod;
// //   receiptDate?: Date;
// //   comments: string;
// // }

// // export interface UpdateReportInstance {
// //   id: number;
// //   amount?: number;
// //   status: ReportStatus;
// //   paymentMethod?: PaymentMethod;
// //   receiptDate?: Date;
// //   reportedDate?: Date;
// //   paidDate?: Date;
// //   comments: string;
// // }


// import { PaymentMethod, ReportStatus } from './enums';

// /**
//  * דיווח בסיסי
//  */
// export interface ReportInstance {
//   id: number;
//   configId: number;
//   period: Date;
//   amount?: number;
//   status: ReportStatus;
//   paymentMethod?: PaymentMethod;
//   receiptDate?: Date;
//   reportedDate?: Date;
//   paidDate?: Date;
//   comments?: string;
//   createdAt: Date;
//   updatedAt?: Date;
// }

// /**
//  * דיווח מפורט עם כל הפרטים (Include של Config, Company וכו')
//  */
// export interface ReportInstanceDetail extends ReportInstance {
//   config: {
//     id: number;
//     companyId: number;
//     reportTypeId: number;
//     frequencyId: number;
//       daysOverdue?: number;

//     company: {
//       id: number;
//       name: string;
//       taxId?: string;
//     };
//     reportType: {
//       id: number;
//       name: string;
//       description?: string;
//     };
//     frequency: {
//       id: number;
//       name: string;
//       description?: string;
//     };
//   };


// // export interface ReportInstanceDetail extends ReportInstance {
// //   companyId: number;
// //   companyName: string;
// //   companyTaxId: string;
// //   reportTypeId: number;
// //   reportTypeName: string;
// //   reportTypeShortCode: string;
// //   frequencyName: string;
// //   dayOfMonth?: number;
// //   periodFormatted: string;
// //   daysOverdue?: number;
// //   isOverdue: boolean;
// //   daysUntilDue?: number;

// }


// /**
//  * דיווח קרוב/באיחור (לטבלת upcoming/overdue)
//  */
// export interface UpcomingReport {
//   id: number;
//   configId: number;
//   companyName: string;
//   reportTypeName: string;
//   period: Date;
//   status: ReportStatus;
//   daysUntilDue: number;
//   isOverdue: boolean;
// }

// /**
//  * יצירת דיווח חדש
//  */
// export interface CreateReportInstance {
//   configId: number;
//   period: Date;
//   amount?: number;
//   paymentMethod?: string;
//   receiptDate?: Date;
//   comments?: string;
// }

// /**
//  * עדכון מלא של דיווח
//  */
// export interface UpdateReportInstance {
//   id: number;
//   amount?: number;
//   status?: ReportStatus;
//   paymentMethod?: string;
//   receiptDate?: Date;
//   reportedDate?: Date;
//   paidDate?: Date;
//   comments?: string;
// }

// /**
//  * עדכון סטטוס בלבד
//  */
// export interface UpdateReportStatus {
//   id: number;
//   status: ReportStatus;
// }

// /**
//  * עדכון תשלום בלבד
//  */
// export interface UpdateReportPayment {
//   id: number;
//   amount: number;
//   paymentMethod: string;
//   paidDate: Date;
// }

// /**
//  * פילטר לחיפוש דיווחים
//  */
// export interface ReportFilter {
//   companyId?: number;
//   configId?: number;
//   status?: ReportStatus;
//   fromPeriod?: Date;
//   toPeriod?: Date;
//   isOverdue?: boolean;
// }

// /**
//  * סטטיסטיקות דיווחים (לעתיד)
//  */
// export interface ReportStatistics {
//   totalReports: number;
//   pendingReports: number;
//   overdueReports: number;
//   completedReports: number;
//   totalAmount: number;
//   averageAmount: number;
// }




import { PaymentMethod, ReportStatus } from './enums';

/**
 * דיווח בסיסי - תואם ל-ReportInstanceDto בשרת
 */
export interface ReportInstance {
  id: number;
  configId: number;
  period: Date;
  amount?: number;
  status: string;  // "Pending", "Reported", "Paid", "Approved", "NotRequired"
  paymentMethod?: string;  // "Credit", "Transfer", "Check", "Online", "Cash"
  receiptDate?: Date;
  reportedDate?: Date;
  paidDate?: Date;
  comments?: string;
  createdAt?: Date;
  updatedAt?: Date;
}

/**
 * דיווח מפורט - תואם ל-ReportInstanceDetailDto בשרת
 * ⚠️ זה מה שהשרת מחזיר מ-GetReportsByCompanyIdQueryHandler
 */
export interface ReportInstanceDetail {
  // ID fields
  id: number;
  configId: number;
  
  // Company info - ישירות בשורש (לא בתוך config.company!)
  companyId: number;
  companyName: string;
  companyTaxId: string;
  
  // Report Type info - ישירות בשורש
  reportTypeId: number;
  reportTypeName: string;
  reportTypeShortCode: string;
  
  // Frequency info - ישירות בשורש
  frequencyName: string;
  dayOfMonth?: number;
  
  // Instance data
  period: Date;
  amount?: number;
  status: string;
  paymentMethod?: string;
  receiptDate?: Date;
  reportedDate?: Date;
  paidDate?: Date;
  comments?: string;
  
  // Timestamps
  createdAt?: Date;
  updatedAt?: Date;
  
  // Calculated fields (אם השרת מחזיר אותם)
  periodFormatted?: string;
  daysOverdue?: number;
}

/**
 * דיווח קרוב/באיחור - תואם ל-UpcomingReportDto בשרת
 */
export interface UpcomingReport {
  id: number;
  companyName: string;
  reportTypeName: string;
  shortCode: string;
  period: Date;
  status: string;
  amount?: number;
  dayOfMonth?: number;
  daysOverdue: number;
}

/**
 * יצירת דיווח חדש - תואם ל-CreateReportInstanceDto בשרת
 */
export interface CreateReportInstance {
  companyId: number;      // 🆕 חובה
  reportTypeId: number;   // 🆕 חובה
  frequencyId?: number;   // 🆕 אופציונלי
  // configId: number;
  period: Date;
  amount?: number;
  paymentMethod?: string;
  receiptDate?: Date;
  comments?: string;
}

/**
 * עדכון מלא של דיווח - תואם ל-UpdateReportInstanceDto בשרת
 */
export interface UpdateReportInstance {
  id: number;
  amount?: number;
  status?: string;
  paymentMethod?: string;
  receiptDate?: Date;
  reportedDate?: Date;
  paidDate?: Date;
  comments?: string;
}

/**
 * עדכון סטטוס בלבד - תואם ל-UpdateReportStatusDto בשרת
 */
export interface UpdateReportStatus {
  id: number;
  status: string;
}

/**
 * עדכון תשלום בלבד - תואם ל-UpdateReportPaymentDto בשרת
 */
export interface UpdateReportPayment {
  id: number;
  amount: number;
  paymentMethod: string;
  paidDate: Date;
}