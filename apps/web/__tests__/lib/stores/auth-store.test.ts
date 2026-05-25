import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { useAuthStore, type AuthUser } from '@/lib/stores/auth-store';

const user: AuthUser = { id: '1', username: 'admin', role: 'ADMIN' };

function resetStore() {
  useAuthStore.setState({
    accessToken: null,
    expiresAt: null,
    user: null,
    isAuthenticated: false,
  });
}

describe('auth-store', () => {
  beforeEach(() => {
    resetStore();
    sessionStorage.clear();
    vi.useRealTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  describe('setAuth', () => {
    it('sets token, expiry, user and flips isAuthenticated', () => {
      useAuthStore.getState().setAuth('tok', '2999-01-01T00:00:00Z', user);

      const s = useAuthStore.getState();
      expect(s.accessToken).toBe('tok');
      expect(s.expiresAt).toBe('2999-01-01T00:00:00Z');
      expect(s.user).toEqual(user);
      expect(s.isAuthenticated).toBe(true);
    });
  });

  describe('clearAuth', () => {
    it('resets state and removes session keys', () => {
      sessionStorage.setItem('access_token', 'tok');
      sessionStorage.setItem('token_expires_at', '2999-01-01T00:00:00Z');
      sessionStorage.setItem('user', JSON.stringify(user));
      useAuthStore.getState().setAuth('tok', '2999-01-01T00:00:00Z', user);

      useAuthStore.getState().clearAuth();

      const s = useAuthStore.getState();
      expect(s.accessToken).toBeNull();
      expect(s.expiresAt).toBeNull();
      expect(s.user).toBeNull();
      expect(s.isAuthenticated).toBe(false);
      expect(sessionStorage.getItem('access_token')).toBeNull();
      expect(sessionStorage.getItem('token_expires_at')).toBeNull();
      expect(sessionStorage.getItem('user')).toBeNull();
    });
  });

  describe('initFromSession', () => {
    it('hydrates state when a valid non-expired session exists', () => {
      vi.useFakeTimers();
      vi.setSystemTime(new Date('2026-01-01T00:00:00Z'));
      sessionStorage.setItem('access_token', 'tok');
      sessionStorage.setItem('token_expires_at', '2026-01-02T00:00:00Z');
      sessionStorage.setItem('user', JSON.stringify(user));

      useAuthStore.getState().initFromSession();

      const s = useAuthStore.getState();
      expect(s.isAuthenticated).toBe(true);
      expect(s.accessToken).toBe('tok');
      expect(s.user).toEqual(user);
    });

    it('does nothing when no session keys are present', () => {
      useAuthStore.getState().initFromSession();

      expect(useAuthStore.getState().isAuthenticated).toBe(false);
    });

    it('clears expired session keys and leaves state unauthenticated', () => {
      vi.useFakeTimers();
      vi.setSystemTime(new Date('2026-01-03T00:00:00Z'));
      sessionStorage.setItem('access_token', 'tok');
      sessionStorage.setItem('token_expires_at', '2026-01-02T00:00:00Z');
      sessionStorage.setItem('user', JSON.stringify(user));

      useAuthStore.getState().initFromSession();

      expect(useAuthStore.getState().isAuthenticated).toBe(false);
      expect(sessionStorage.getItem('access_token')).toBeNull();
      expect(sessionStorage.getItem('token_expires_at')).toBeNull();
      expect(sessionStorage.getItem('user')).toBeNull();
    });

    it('clears keys when stored user JSON is malformed', () => {
      vi.useFakeTimers();
      vi.setSystemTime(new Date('2026-01-01T00:00:00Z'));
      sessionStorage.setItem('access_token', 'tok');
      sessionStorage.setItem('token_expires_at', '2026-01-02T00:00:00Z');
      sessionStorage.setItem('user', '{not-json');

      useAuthStore.getState().initFromSession();

      expect(useAuthStore.getState().isAuthenticated).toBe(false);
      expect(sessionStorage.getItem('access_token')).toBeNull();
      expect(sessionStorage.getItem('user')).toBeNull();
    });
  });
});
