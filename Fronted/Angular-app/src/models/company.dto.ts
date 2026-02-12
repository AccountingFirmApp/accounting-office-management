/**
 * Company DTO
 * מייצג חברה במערכת
 */
export interface CompanyDto {
  id: number;
  name: string;
  taxId: string;
  address?: string;
  phone?: string;
  email?: string;
  IsActive?: boolean;
  notes?: string;
}
