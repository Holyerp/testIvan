import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook } from '@testing-library/react';
import { useRequireModule } from '@/lib/hooks/use-require-module';
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
  // The hook reads render-time state via selectors: useAuthStore((s) => s.isAuthenticated).
  // Apply the selector so the return value reflects the selected slice (mirrors real Zustand).
  vi.mocked(useAuthStore).mockImplementation(
    (selector?: (s: typeof value) => unknown) =>
      (selector ? selector(value) : value) as unknown as ReturnType<typeof useAuthStore>,
  );
  (useAuthStore as unknown as { getState: ReturnType<typeof vi.fn> }).getState.mockReturnValue(
    value as unknown as ReturnType<typeof useAuthStore.getState>,
  );
}

describe('useRequireModule', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('redirects to /login when not authenticated', () => {
    setStore({ isAuthenticated: false, user: null });

    renderHook(() => useRequireModule('financial'));

    expect(initFromSession).toHaveBeenCalled();
    expect(pushMock).toHaveBeenCalledWith('/login');
  });

  it('redirects a WAREHOUSE user away from the financial module to /403', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'wh', role: 'WAREHOUSE' } });

    renderHook(() => useRequireModule('financial'));

    expect(pushMock).toHaveBeenCalledWith('/403');
  });

  it('allows a WAREHOUSE user on the warehouse module', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'wh', role: 'WAREHOUSE' } });

    const { result } = renderHook(() => useRequireModule('warehouse'));

    expect(pushMock).not.toHaveBeenCalled();
    expect(result.current.isAuthenticated).toBe(true);
  });

  it('allows an ADMIN user on the financial module', () => {
    setStore({ isAuthenticated: true, user: { id: '1', username: 'a', role: 'ADMIN' } });

    renderHook(() => useRequireModule('financial'));

    expect(pushMock).not.toHaveBeenCalled();
  });

  it('redirects to /403 when authenticated but user is null', () => {
    setStore({ isAuthenticated: true, user: null });

    renderHook(() => useRequireModule('financial'));

    expect(pushMock).toHaveBeenCalledWith('/403');
  });
});
