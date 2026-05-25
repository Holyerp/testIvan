import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import LoginPage from '@/app/(auth)/login/page';

// next/navigation mock
const pushMock = vi.fn();
vi.mock('next/navigation', () => ({
  useRouter: () => ({ push: pushMock }),
}));

// sessionStorage mock
const sessionStorageMock = (() => {
  const store: Record<string, string> = {};
  return {
    getItem: (key: string) => store[key] ?? null,
    setItem: (key: string, value: string) => { store[key] = value; },
    removeItem: (key: string) => { delete store[key]; },
    clear: () => { Object.keys(store).forEach(k => delete store[k]); },
  };
})();
Object.defineProperty(global, 'sessionStorage', { value: sessionStorageMock });

describe('LoginPage', () => {
  beforeEach(() => {
    vi.resetAllMocks();
    sessionStorageMock.clear();
  });

  it('renders username and password fields', () => {
    render(<LoginPage />);
    expect(screen.getByLabelText('Korisničko ime')).toBeDefined();
    expect(screen.getByLabelText('Lozinka')).toBeDefined();
    expect(screen.getByRole('button', { name: 'Prijavi se' })).toBeDefined();
  });

  it('shows validation errors when submitting empty form', async () => {
    render(<LoginPage />);
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(screen.getByText('Korisničko ime je obavezno')).toBeDefined();
    });
    await waitFor(() => {
      expect(screen.getByText('Lozinka je obavezna')).toBeDefined();
    });
  });

  it('shows loading state during submission', async () => {
    global.fetch = vi.fn().mockReturnValue(new Promise(() => {})); // never resolves

    render(<LoginPage />);
    await userEvent.type(screen.getByLabelText('Korisničko ime'), 'admin');
    await userEvent.type(screen.getByLabelText('Lozinka'), 'Admin123!');
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Prijavljivanje...' })).toBeDefined();
    });
  });

  it('redirects to dashboard on successful login', async () => {
    global.fetch = vi.fn().mockResolvedValueOnce({
      ok: true,
      json: async () => ({
        success: true,
        data: {
          accessToken: 'test-token',
          expiresAt: '2026-06-01T00:00:00Z',
          user: { id: '1', username: 'admin', role: 'ADMIN' },
        },
      }),
    } as Response);

    render(<LoginPage />);
    await userEvent.type(screen.getByLabelText('Korisničko ime'), 'admin');
    await userEvent.type(screen.getByLabelText('Lozinka'), 'Admin123!');
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(pushMock).toHaveBeenCalledWith('/dashboard');
    });
    expect(sessionStorageMock.getItem('access_token')).toBe('test-token');
    expect(sessionStorageMock.getItem('user')).toContain('admin');
  });

  it('shows AUTH_INVALID_CREDENTIALS error on 401', async () => {
    global.fetch = vi.fn().mockResolvedValueOnce({
      ok: false,
      status: 401,
      json: async () => ({ success: false, error: 'Invalid credentials', code: 'AUTH_INVALID_CREDENTIALS' }),
    } as Response);

    render(<LoginPage />);
    await userEvent.type(screen.getByLabelText('Korisničko ime'), 'admin');
    await userEvent.type(screen.getByLabelText('Lozinka'), 'wrong');
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(screen.getByRole('alert')).toBeDefined();
      expect(screen.getByText('Pogrešno korisničko ime ili lozinka.')).toBeDefined();
    });
    expect(pushMock).not.toHaveBeenCalled();
  });

  it('shows AUTH_RATE_LIMITED error on 429', async () => {
    global.fetch = vi.fn().mockResolvedValueOnce({
      ok: false,
      status: 429,
      json: async () => ({ success: false, error: 'Too many attempts', code: 'AUTH_RATE_LIMITED' }),
    } as Response);

    render(<LoginPage />);
    await userEvent.type(screen.getByLabelText('Korisničko ime'), 'admin');
    await userEvent.type(screen.getByLabelText('Lozinka'), 'pass');
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(screen.getByText('Previše neuspješnih pokušaja. Pokušajte za 15 minuta.')).toBeDefined();
    });
  });

  it('shows INTERNAL_ERROR on network failure', async () => {
    global.fetch = vi.fn().mockRejectedValueOnce(new Error('Network error'));

    render(<LoginPage />);
    await userEvent.type(screen.getByLabelText('Korisničko ime'), 'admin');
    await userEvent.type(screen.getByLabelText('Lozinka'), 'Admin123!');
    fireEvent.click(screen.getByRole('button', { name: 'Prijavi se' }));

    await waitFor(() => {
      expect(screen.getByText('Greška servera. Pokušajte ponovo.')).toBeDefined();
    });
  });
});
