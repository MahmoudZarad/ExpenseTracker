export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthUser {
  userId: number;
  name: string;
  email: string;
}

export interface AuthResponse {
  isSuccess: boolean;
  statusCode: number;
  value: {
    userId: number;
    name: string;
    email: string;
    token: string;
  };
}
