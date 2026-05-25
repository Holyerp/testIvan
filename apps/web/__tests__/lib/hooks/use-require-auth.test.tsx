import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook } from '@testing-library/react';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import type { AuthUser } from '@/lib/stores/auth-store';

const pushMock = vi.fn();
vi.mock('next/navigation', () => ({
  useRouter: () => ({ push: pushMock }),
}));

vi.mock('@/lib/stores/auth-store', () => {
  const fn = vi.fn() as unknown as ReturnType<typeof vi.fn> & { getState: ReturnType<typeof vi.fn> };
  fn.getState = vi.fn();
  return { useAuthStore: fn };
});

import { useAuthStore } from '@/lib/stores/auth-store';

const initFromSession = vi.fn();

function setStore(state: { isAuthenticated: boolean; user: AuthUser | null }) {
  const value = {
    isAuthenticated: state.isAuthenticated,
    user: state.user,
    initFromSession,
  };
  // The hook reads render-time state via useAuthStore() (for its return value)
  // and the fresh state via useAuthStore.getState() (for the redirect decision).
  vi.mocked(useAuthStore).mockReturnValue(value as unknown as ReturnType<typeof useAuthStore>);
  (useAuthStore as unknown as { getState: ReturnType<typeof vi.fn> }).getState.mockReturnValue(
    value as unknown as ReturnType<typeof useAuthStore.getState>,
  );
}

describe('useRequireAuth', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('redirects to /login when not authenticated', () => {
    setStore({ isAuthenticated: false, user: null });

    renderHook(() => useRequireAuth());

    expect(initFromSession).toHaveBeenCalled();
    expect(pushMock).toHaveBeenCalledWith('/login');
  });

  it('does not redirect when authenticated and no roles required', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'a', role: 'ADMIN' } });

    const { result } = renderHook(() => useRequireAuth());

    expect(pushMock).not.toHaveBeenCalled();
    expect(result.current.isAuthenticated).toBe(true);
  });

  it('allows access when the user has a required role', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'a', role: 'MANAGER' } });

    renderHook(() => useRequireAuth(['ADMIN', 'MANAGER']));

    expect(pushMock).not.toHaveBeenCalled();
  });

  it('redirects to /403 when the user lacks the required role', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'w', role: 'WAREHOUSE' } });

    renderHook(() => useRequireAuth(['ADMIN']));

    expect(pushMock).toHaveBeenCalledWith('/403');
  });
});
