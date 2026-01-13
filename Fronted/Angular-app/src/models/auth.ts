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
  firmId: number;
}
