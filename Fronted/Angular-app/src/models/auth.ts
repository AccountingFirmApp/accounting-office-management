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
  employeeId: string;
  firstName: string;
  lastName: string;
  email: string;
  roleName: string;
  roleId: number;        // ← מוסיפים את זה
  password: string;
  firmId: number;
  isActive: boolean;
}
