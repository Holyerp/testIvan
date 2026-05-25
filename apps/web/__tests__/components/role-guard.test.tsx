import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import { RoleGuard } from '@/components/role-guard';
import type { AuthUser } from '@/lib/stores/auth-store';

// Mock the auth store module
vi.mock('@/lib/stores/auth-store', () => ({
  useAuthStore: vi.fn(),
}));

import { useAuthStore } from '@/lib/stores/auth-store';

type StoreState = { user: AuthUser | null };

function setMockUser(user: AuthUser | null) {
  vi.mocked(useAuthStore).mockImplementation(
    (selector: (s: StoreState) => unknown) => selector({ user })
  );
}

describe('RoleGuard', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders children when user has the required role', () => {
    setMockUser({ id: '1', username: 'testuser', role: 'ADMIN' });

    render(
      <RoleGuard roles={['ADMIN']}>
        <span>Protected Content</span>
      </RoleGuard>
    );

    expect(screen.getByText('Protected Content')).toBeDefined();
  });

  it('renders children when user has one of the allowed roles', () => {
    setMockUser({ id: '2', username: 'manager', role: 'MANAGER' });

    render(
      <RoleGuard roles={['ADMIN', 'MANAGER', 'ACCOUNTING']}>
        <span>Financial Content</span>
      </RoleGuard>
    );

    expect(screen.getByText('Financial Content')).toBeDefined();
  });

  it('renders fallback when user has wrong role', () => {
    setMockUser({ id: '3', username: 'warehouse', role: 'WAREHOUSE' });

    render(
      <RoleGuard roles={['ADMIN']} fallback={<span>No Access</span>}>
        <span>Protected</span>
      </RoleGuard>
    );

    expect(screen.queryByText('Protected')).toBeNull();
    expect(screen.getByText('No Access')).toBeDefined();
  });

  it('renders null fallback (default) when user has wrong role and no fallback provided', () => {
    setMockUser({ id: '3', username: 'warehouse', role: 'WAREHOUSE' });

    const { container } = render(
      <RoleGuard roles={['ADMIN']}>
        <span>Protected</span>
      </RoleGuard>
    );

    expect(screen.queryByText('Protected')).toBeNull();
    expect(container.firstChild).toBeNull();
  });

  it('renders fallback when no user is authenticated', () => {
    setMockUser(null);

    render(
      <RoleGuard roles={['ADMIN']} fallback={<span>No Access</span>}>
        <span>Protected</span>
      </RoleGuard>
    );

    expect(screen.queryByText('Protected')).toBeNull();
    expect(screen.getByText('No Access')).toBeDefined();
  });
});
