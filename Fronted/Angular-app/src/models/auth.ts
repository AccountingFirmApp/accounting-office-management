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
  firstname: string;
  lastname: string;
  email: string;
  roleName: string;
  roleId: number;        // ← מוסיפים את זה
  password: string;
  firmId: number;
  isActive: boolean;
}
export interface WorkerTask {
  id: number;
  companyid: number;
  companyName: string;
  tasktypeid: number;
  taskTypeName: string;
  period: string;
  duedate?: string;
  completeddate?: string;
  status: number;
  notes?: string;
  assignedworkerid?: number;
  assignedWorkerName?: string;
  createdat: string;
  updatedat: string;
}