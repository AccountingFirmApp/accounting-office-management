export interface CompanyDto {
    id: number;
    name: string;
    taxId: string;
    firmId: number;
    phone?: string;
    email?: string;
    address?: string;
    IsActive: boolean;
    createdAt?: Date;
    updatedAt?: Date;
  }
  
  export interface CreateCompanyCommand {
    name: string;
    taxId: string;
    firmId: number;
    phone?: string;
    email?: string;
    address?: string;
  }
  
  export interface UpdateCompanyCommand {
    id: number;
    name: string;
    taxId: string;
    phone?: string;
    email?: string;
    address?: string;
  }
  
  export interface CompanyWithPendingReportsDto extends CompanyDto {
    pendingReportsCount?: number;
  }