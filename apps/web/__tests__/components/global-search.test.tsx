import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { GlobalSearch } from '@/components/global-search';
import type { SearchResults } from '@/lib/search';

const pushMock = vi.fn();
vi.mock('next/navigation', () => ({
  useRouter: () => ({ push: pushMock }),
}));

// Translation mock — returns the key so assertions are deterministic.
vi.mock('next-intl', () => ({
  useTranslations: () => (key: string) => key,
}));

vi.mock('@/lib/stores/auth-store', () => ({
  useAuthStore: vi.fn(),
}));

import { useAuthStore } from '@/lib/stores/auth-store';

function setToken(token: string | null) {
  vi.mocked(useAuthStore).mockImplementation((selector: (s: unknown) => unknown) =>
    selector({ accessToken: token })
  );
}

const results: SearchResults = {
  customers: [{ id: 'c1', type: 'CUSTOMER', title: 'Acme Co', subtitle: 'PIB 123' }],
  vendors: [],
  salesInvoices: [{ id: 's1', type: 'SALES_INVOICE', title: 'INV-001', subtitle: '100' }],
  purchaseInvoices: [],
};

function mockFetchOk(data: SearchResults) {
  const fetchMock = vi.fn().mockResolvedValue({
    ok: true,
    json: async () => ({ success: true, data }),
  } as Response);
  vi.stubGlobal('fetch', fetchMock);
  return fetchMock;
}

function typeQuery(value: string) {
  fireEvent.change(screen.getByRole('combobox'), { target: { value } });
}

describe('GlobalSearch', () => {
  beforeEach(() => {
    setToken('tok');
  });

  afterEach(() => {
    vi.unstubAllGlobals();
    vi.clearAllMocks();
  });

  it('does not fetch when fewer than 2 characters are typed', async () => {
    const fetchMock = vi.fn();
    vi.stubGlobal('fetch', fetchMock);
    render(<GlobalSearch />);

    typeQuery('a');
    await new Promise((r) => setTimeout(r, 400));

    expect(fetchMock).not.toHaveBeenCalled();
  });

  it('fetches with a Bearer token and renders grouped results', async () => {
    const fetchMock = mockFetchOk(results);

    render(<GlobalSearch />);
    typeQuery('ac');

    await waitFor(() => expect(screen.getByText('Acme Co')).toBeDefined());
    expect(screen.getByText('INV-001')).toBeDefined();

    const [, init] = fetchMock.mock.calls[0];
    expect(init.headers.Authorization).toBe('Bearer tok');
  });

  it('shows the no-results state when all groups are empty', async () => {
    mockFetchOk({ customers: [], vendors: [], salesInvoices: [], purchaseInvoices: [] });

    render(<GlobalSearch />);
    typeQuery('zz');

    await waitFor(() => expect(screen.getByText('noResults')).toBeDefined());
  });

  it('navigates to the highlighted hit on ArrowDown + Enter', async () => {
    mockFetchOk(results);

    render(<GlobalSearch />);
    const input = screen.getByRole('combobox');
    typeQuery('ac');
    await waitFor(() => expect(screen.getByText('Acme Co')).toBeDefined());

    fireEvent.keyDown(input, { key: 'ArrowDown' });
    fireEvent.keyDown(input, { key: 'Enter' });

    expect(pushMock).toHaveBeenCalledWith('/customers/c1');
  });

  it('closes the dropdown on Escape', async () => {
    mockFetchOk(results);

    render(<GlobalSearch />);
    const input = screen.getByRole('combobox');
    typeQuery('ac');
    await waitFor(() => expect(screen.getByText('Acme Co')).toBeDefined());

    fireEvent.keyDown(input, { key: 'Escape' });

    await waitFor(() => expect(screen.queryByText('Acme Co')).toBeNull());
  });

  it('does not fetch until a token is present', async () => {
    setToken(null);
    const fetchMock = vi.fn();
    vi.stubGlobal('fetch', fetchMock);

    render(<GlobalSearch />);
    typeQuery('ac');
    await new Promise((r) => setTimeout(r, 350));

    expect(fetchMock).not.toHaveBeenCalled();
  });
});
