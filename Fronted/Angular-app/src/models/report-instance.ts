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
export interface CreateReportInstance {
  companyId: number;    
  reportTypeId: number;   
  frequencyId?: number;  
  period: Date;
  amount?: number;
  paymentMethod?: string;
  receiptDate?: Date;
  comments?: string;
}


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


export interface UpdateReportStatus {
  id: number;
  status: string;
}

export interface UpdateReportPayment {
  id: number;
  amount: number;
  paymentMethod: string;
  paidDate: Date;
}