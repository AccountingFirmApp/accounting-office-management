/**
 * Report Type DTO
 * מייצג סוג דיווח במערכת
 */
export interface ReportTypeDto {
  id: number;
  name: string;
  description?: string;
  shortCode?: string;
}

/**
 * Frequency DTO
 * מייצג תדירות דיווח
 */
export interface FrequencyDto {
  id: number;
  name: string;
  description?: string;
}
