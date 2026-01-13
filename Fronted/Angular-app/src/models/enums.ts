/**
 * אמצעי תשלום
 * ⚠️ חשוב! הערכים חייבים להתאים בדיוק למה שהשרת מחזיר
 */
export enum PaymentMethod {
  Credit = 'Credit',
  Transfer = 'Transfer',
  Check = 'Check',
  Online = 'Online',
  Cash = 'Cash'
}

/**
 * סטטוסים של דיווחים
 * ⚠️ חשוב! הערכים חייבים להתאים בדיוק למה שהשרת מחזיר
 */
export enum ReportStatus {
  Pending = 'Pending',
  Reported = 'Reported',
  Paid = 'Paid',
  Approved = 'Approved',
  NotRequired = 'NotRequired'
}

/**
 * פונקציות עזר לתרגום
 */
export class ReportHelpers {
  
  /**
   * תרגום סטטוס לעברית
   */
  static getStatusLabel(status: string): string {
    const labels: Record<string, string> = {
      'Pending': 'ממתין',
      'Reported': 'דווח',
      'Paid': 'שולם',
      'Approved': 'אושר',
      'NotRequired': 'לא נדרש'
    };
    return labels[status] || status;
  }

  /**
   * תרגום אמצעי תשלום לעברית
   */
  static getPaymentMethodLabel(method: string): string {
    const labels: Record<string, string> = {
      'Credit': 'כרטיס אשראי',
      'Transfer': 'העברה בנקאית',
      'Check': 'צ\'ק',
      'Online': 'תשלום מקוון',
      'Cash': 'מזומן'
    };
    return labels[method] || method;
  }

  /**
   * קבלת CSS class לפי סטטוס
   */
  static getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'Pending': 'pending',
      'Reported': 'reported',
      'Paid': 'paid',
      'Approved': 'approved',
      'NotRequired': 'not-required'
    };
    return statusMap[status] || '';
  }
}