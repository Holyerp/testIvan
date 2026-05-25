import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { renderHook, act, waitFor } from '@testing-library/react';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { useAuthStore } from '@/lib/stores/auth-store';

interface Row {
  id: string;
}

function authenticate() {
  useAuthStore.setState({
    accessToken: 'tok',
    expiresAt: '2999-01-01T00:00:00Z',
    user: { id: '1', username: 'admin', role: 'ADMIN' },
    isAuthenticated: true,
  });
}

function mockOk(data: unknown) {
  const fetchMock = vi.fn().mockResolvedValue({
    ok: true,
    json: async () => ({ success: true, data }),
  } as Response);
  vi.stubGlobal('fetch', fetchMock);
  return fetchMock;
}

describe('usePaginatedQuery', () => {
  beforeEach(() => {
    authenticate();
  });

  afterEach(() => {
    vi.unstubAllGlobals();
    vi.clearAllMocks();
  });

  it('does not fetch when there is no access token', () => {
    useAuthStore.setState({ accessToken: null, isAuthenticated: false });
    const fetchMock = vi.fn();
    vi.stubGlobal('fetch', fetchMock);

    renderHook(() => usePaginatedQuery<Row>({ path: '/api/v1/things' }));

    expect(fetchMock).not.toHaveBeenCalled();
  });

  it('fetches the list with a Bearer token and exposes items/total', async () => {
    const fetchMock = mockOk({ items: [{ id: 'a' }], total: 1, page: 1, pageSize: 20 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things' })
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.items).toEqual([{ id: 'a' }]);
    expect(result.current.total).toBe(1);
    expect(result.current.error).toBeNull();

    const [, init] = fetchMock.mock.calls[0];
    expect(init.headers.Authorization).toBe('Bearer tok');
  });

  it('computes totalPages from total and pageSize', async () => {
    mockOk({ items: [], total: 45, page: 1, pageSize: 20 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things', pageSize: 20 })
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));
    expect(result.current.totalPages).toBe(3);
  });

  it('sets error from the envelope code on a failed response', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        json: async () => ({ success: false, code: 'FORBIDDEN' }),
      } as Response)
    );

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things' })
    );

    await waitFor(() => expect(result.current.isLoading).toBe(false));
    expect(result.current.error).toBe('FORBIDDEN');
  });

  it('setPage updates the page', async () => {
    mockOk({ items: [], total: 100, page: 1, pageSize: 20 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things' })
    );
    await waitFor(() => expect(result.current.isLoading).toBe(false));

    act(() => result.current.setPage(3));
    expect(result.current.page).toBe(3);
  });

  it('setSort sets the key/dir and toggles direction on the same key, resetting page', async () => {
    mockOk({ items: [], total: 0, page: 1, pageSize: 20 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things' })
    );
    await waitFor(() => expect(result.current.isLoading).toBe(false));

    act(() => result.current.setPage(2));
    act(() => result.current.setSort('name'));
    expect(result.current.sortBy).toBe('name');
    expect(result.current.sortDir).toBe('asc');
    expect(result.current.page).toBe(1);

    act(() => result.current.setSort('name'));
    expect(result.current.sortDir).toBe('desc');

    act(() => result.current.setSort('name'));
    expect(result.current.sortDir).toBe('asc');
  });

  it('setParam stores the filter and resets page to 1', async () => {
    mockOk({ items: [], total: 0, page: 1, pageSize: 20 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({ path: '/api/v1/things' })
    );
    await waitFor(() => expect(result.current.isLoading).toBe(false));

    act(() => result.current.setPage(4));
    act(() => result.current.setParam('status', 'OPEN'));
    expect(result.current.page).toBe(1);
  });

  it('honors initialSortBy/initialSortDir and extraParams', async () => {
    const fetchMock = mockOk({ items: [], total: 0, page: 1, pageSize: 10 });

    const { result } = renderHook(() =>
      usePaginatedQuery<Row>({
        path: '/api/v1/things',
        pageSize: 10,
        initialSortBy: 'createdAt',
        initialSortDir: 'desc',
        extraParams: { tab: 'OPEN' },
      })
    );
    await waitFor(() => expect(result.current.isLoading).toBe(false));

    expect(result.current.sortBy).toBe('createdAt');
    expect(result.current.sortDir).toBe('desc');
    const [url] = fetchMock.mock.calls[0];
    expect(String(url)).toContain('tab=OPEN');
  });
});
