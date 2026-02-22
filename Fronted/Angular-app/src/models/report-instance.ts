
/**
 * דיווח בסיסי - תואם ל-ReportInstanceDto בשרת
 */
export interface ReportInstance {
  id: number;
  configId: number;
  period: Date;
  amount?: number;
  status: string; 
  reportedDate?: Date;
  paidDate?: Date;
  comments?: string;
  createdAt?: Date;
  updatedAt?: Date;

}


export interface ReportInstanceDetail {
  id: number;
  configId: number;
  companyId: number;
  companyName: string;
  companyTaxId: string;
  reportTypeId: number;
  reportTypeName: string;
  reportTypeShortCode: string;
    frequencyName: string;
  dayOfMonth?: number;
    period: Date;
  amount?: number;
  status: string;
  paymentMethod?: string;
  receiptDate?: Date;
  reportedDate?: Date;
  paidDate?: Date;
  comments?: string;
    createdAt?: Date;
  updatedAt?: Date;
    periodFormatted?: string;
  daysOverdue?: number;
  workerNames: string[];
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