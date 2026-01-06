export interface WorkerCompanies {
  worker: {
    id: number;
    fullName: string;
  };
  companies: Array<{
    assignmentId: number;
    companyId: number;
    companyName: string;
    companyTaxId: string;
    isActive: boolean;
    assignedAt: Date;
  }>;
}