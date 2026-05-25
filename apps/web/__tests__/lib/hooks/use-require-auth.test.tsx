import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook } from '@testing-library/react';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import type { AuthUser } from '@/lib/stores/auth-store';

const pushMock = vi.fn();
vi.mock('next/navigation', () => ({
  useRouter: () => ({ push: pushMock }),
}));

vi.mock('@/lib/stores/auth-store', () => ({
  useAuthStore: vi.fn(),
}));

import { useAuthStore } from '@/lib/stores/auth-store';

const initFromSession = vi.fn();

function setStore(state: { isAuthenticated: boolean; user: AuthUser | null }) {
  vi.mocked(useAuthStore).mockReturnValue({
    isAuthenticated: state.isAuthenticated,
    user: state.user,
    initFromSession,
  } as unknown as ReturnType<typeof useAuthStore>);
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
