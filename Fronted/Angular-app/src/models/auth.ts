export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface GoogleLoginRequestDto {
  googleToken: string;
}

export interface LoginResponseDto {
  token: string;
  tokenType: string;
  expiresIn: number;
  worker: WorkerInfoDto;
}

export interface WorkerInfoDto {
  id: number;
  employeeid: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  roleName: string;
  roleId: number;        // ← מוסיפים את זה
  password: string;
  firmid: number;
  isActive: boolean;
  companyIds?: number[]; // 🆕 אופציונלי - רשימת מזהי חברות שהעובד קשור אליהן
  hireDate?: Date;
}
