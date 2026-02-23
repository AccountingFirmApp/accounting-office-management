export interface CompanyReportConfigDto {
  id: number;
  companyId: number;
  reportTypeId: number;
  frequencyId: number;
  dayOfMonth?: number;
  isActive?: boolean;
  createdAt: Date;
  updatedAt: Date;
  year: number;

  companyName?: string;
  reportTypeName?: string;
  reportTypeShortCode?: string;
  frequencyName?: string;
}

export interface CreateCompanyReportConfigDto {
  companyId: number;
  reportTypeId: number;
  frequencyId: number;
  dayOfMonth?: number;
  year: number;
}

export interface UpdateCompanyReportConfigDto extends CreateCompanyReportConfigDto {
  id: number;
}
