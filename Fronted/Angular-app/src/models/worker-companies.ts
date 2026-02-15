export interface CompanyDto {
  id: number;
  name: string;
  taxId: string;
  address: string;
  phone: string;
  notes: string;
  firmId: number;
  email: string;
  isactive: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}

export type WorkerCompanies = CompanyDto[];