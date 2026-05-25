import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import { AuthProvider } from '@/components/auth-provider';

vi.mock('@/lib/stores/auth-store', () => ({
  useAuthStore: vi.fn(),
}));

import { useAuthStore } from '@/lib/stores/auth-store';

const initFromSession = vi.fn();

describe('AuthProvider', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.mocked(useAuthStore).mockImplementation((selector: (s: unknown) => unknown) =>
      selector({ initFromSession })
    );
  });

  it('renders its children', () => {
    render(
      <AuthProvider>
        <span>child content</span>
      </AuthProvider>
    );
    expect(screen.getByText('child content')).toBeDefined();
  });

  it('calls initFromSession on mount', () => {
    render(
      <AuthProvider>
        <span>x</span>
      </AuthProvider>
    );
    expect(initFromSession).toHaveBeenCalledTimes(1);
  });
});
