export interface TaskcompanyDto {
    id: number;
    companyId: number;
    taskTypeId: number;
    period: Date;
    dueDate?: Date;
    completedDate?: Date;
    assignedWorkerId?: number;
    notes?: string;
    status: string;
    createdAt: Date;
    updatedAt: Date;
    companyName?: string;
    taskTypeName?: string;
    assignedWorkerName?: string;
  }