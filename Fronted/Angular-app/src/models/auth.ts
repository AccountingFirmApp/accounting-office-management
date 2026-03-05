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
  roleId: number; 
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
export interface ChecklistItem {
  id?: number;
  title: string;
  description?: string;
  isCompleted?: boolean;
  orderIndex: number;
}

export interface ChecklistProgress {
  total: number;
  completed: number;
}


export interface ChecklistTemplate {
  taskTypeId: number;
  items: ChecklistItem[];
}
export interface TaskDetail {
  id: number;
  companyName: string;
  taskTypeName: string;
  status: string;
  checklistItems: ChecklistItem[];
  checklistProgress: ChecklistProgress;
}

export interface CompanyTaskConfigDto {
  id: number;
  companyId: number;
  companyName: string;
  taskTypeId: number;
  taskTypeName: string;
  configurationId?: number; 
  assignedWorkerId?: number;
  workerName: string;
  frequency: number;
  dueDay: number;
  isActive: boolean;
}