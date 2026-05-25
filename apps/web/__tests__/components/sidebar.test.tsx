import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { Sidebar } from '@/components/sidebar';
import type { AuthUser } from '@/lib/stores/auth-store';

vi.mock('next/navigation', () => ({
  usePathname: () => '/dashboard',
}));

vi.mock('@/lib/stores/auth-store', () => ({
  useAuthStore: vi.fn(),
}));

import { useAuthStore } from '@/lib/stores/auth-store';

const clearAuth = vi.fn();

function setUser(user: AuthUser | null) {
  vi.mocked(useAuthStore).mockImplementation((selector: (s: unknown) => unknown) =>
    selector({ user, clearAuth })
  );
}

describe('Sidebar', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('shows all nav items for an ADMIN', () => {
    setUser({ id: '1', username: 'admin', role: 'ADMIN' });
    render(<Sidebar />);

    expect(screen.getByText('Dashboard')).toBeDefined();
    expect(screen.getByText('Customers')).toBeDefined();
    expect(screen.getByText('Vendors')).toBeDefined();
    expect(screen.getByText('Sales Invoices')).toBeDefined();
    expect(screen.getByText('Purchase Invoices')).toBeDefined();
    expect(screen.getByText('Items')).toBeDefined();
  });

  it('shows Dashboard and Items (warehouse) but no financial items for a WAREHOUSE user', () => {
    setUser({ id: '2', username: 'wh', role: 'WAREHOUSE' });
    render(<Sidebar />);

    expect(screen.getByText('Dashboard')).toBeDefined();
    expect(screen.getByText('Items')).toBeDefined();
    expect(screen.queryByText('Customers')).toBeNull();
    expect(screen.queryByText('Vendors')).toBeNull();
    expect(screen.queryByText('Sales Invoices')).toBeNull();
  });

  it('renders the user role and username when a user is present', () => {
    setUser({ id: '3', username: 'manager1', role: 'MANAGER' });
    render(<Sidebar />);

    expect(screen.getByText('MANAGER')).toBeDefined();
    expect(screen.getByText('manager1')).toBeDefined();
  });

  it('shows every item (no role filter) when there is no user', () => {
    setUser(null);
    render(<Sidebar />);

    expect(screen.getByText('Dashboard')).toBeDefined();
    expect(screen.getByText('Customers')).toBeDefined();
  });

  it('calls clearAuth when Logout is clicked', () => {
    // jsdom does not implement navigation; stub the href setter so the click handler
    // can run without emitting a "Not implemented" navigation error.
    const originalLocation = window.location;
    Object.defineProperty(window, 'location', {
      configurable: true,
      writable: true,
      value: { ...originalLocation, href: '' },
    });

    setUser({ id: '1', username: 'admin', role: 'ADMIN' });
    render(<Sidebar />);

    fireEvent.click(screen.getByText('Logout'));
    expect(clearAuth).toHaveBeenCalledTimes(1);

    Object.defineProperty(window, 'location', {
      configurable: true,
      writable: true,
      value: originalLocation,
    });
  });
});
