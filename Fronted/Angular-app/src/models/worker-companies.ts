export interface CompanyDto {
  id: number;
  name: string;
  taxId: string;
  address: string;
  phone: string;
  notes: string;
  firmId: number;
  email: string;
  isActive: boolean;
}

export type WorkerCompanies = CompanyDto[];