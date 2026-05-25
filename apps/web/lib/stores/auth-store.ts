import { create } from 'zustand';

export interface AuthUser {
  id: string;
  username: string;
  role: string;
}

interface AuthState {
  accessToken: string | null;
  expiresAt: string | null;
  user: AuthUser | null;
  isAuthenticated: boolean;
  setAuth: (token: string, expiresAt: string, user: AuthUser) => void;
  clearAuth: () => void;
  initFromSession: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  accessToken: null,
  expiresAt: null,
  user: null,
  isAuthenticated: false,

  setAuth: (accessToken, expiresAt, user) =>
    set({ accessToken, expiresAt, user, isAuthenticated: true }),

  clearAuth: () => {
    if (typeof window !== 'undefined') {
      sessionStorage.removeItem('access_token');
      sessionStorage.removeItem('token_expires_at');
      sessionStorage.removeItem('user');
    }
    set({ accessToken: null, expiresAt: null, user: null, isAuthenticated: false });
  },

  initFromSession: () => {
    if (typeof window === 'undefined') return;
    const token = sessionStorage.getItem('access_token');
    const expiresAt = sessionStorage.getItem('token_expires_at');
    const userJson = sessionStorage.getItem('user');
    if (token && expiresAt && userJson) {
      const expiresDate = new Date(expiresAt);
      if (expiresDate > new Date()) {
        try {
          const user = JSON.parse(userJson) as AuthUser;
          set({ accessToken: token, expiresAt, user, isAuthenticated: true });
        } catch {
          sessionStorage.removeItem('access_token');
          sessionStorage.removeItem('token_expires_at');
          sessionStorage.removeItem('user');
        }
      } else {
        sessionStorage.removeItem('access_token');
        sessionStorage.removeItem('token_expires_at');
        sessionStorage.removeItem('user');
      }
    }
  },
}));
