export type UserRole = 'ADMIN' | 'MANAGER' | 'ACCOUNTING' | 'WAREHOUSE';

export interface User {
  id: string;
  username: string;
  role: UserRole;
}

export interface AuthTokens {
  accessToken: string;
  expiresAt: string;
  user: User;
}

export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface ApiSuccessResponse<T> {
  success: true;
  data: T;
}

export interface ApiErrorResponse {
  success: false;
  error: string;
  code: string;
}
