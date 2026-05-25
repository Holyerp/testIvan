import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { NotificationBell } from '@/components/notification-bell';
import type { Notification } from '@/lib/notifications';

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

const notifications: Notification[] = [
  {
    id: 'OVERDUE_INVOICE:inv007',
    type: 'OVERDUE_INVOICE',
    title: 'SI-007',
    message: 'Invoice SI-007 is 10 day(s) overdue.',
    severity: 'WARNING',
    link: '/sales/invoices/inv007',
  },
  {
    id: 'LOW_STOCK:itm002',
    type: 'LOW_STOCK',
    title: 'Steel bar 12mm',
    message: 'Low on stock (30 / min 100).',
    severity: 'WARNING',
    link: '/items/itm002',
  },
];

function mockFetchOk(data: Notification[]) {
  const fetchMock = vi.fn().mockResolvedValue({
    ok: true,
    json: async () => ({ success: true, data }),
  } as Response);
  vi.stubGlobal('fetch', fetchMock);
  return fetchMock;
}

describe('NotificationBell', () => {
  beforeEach(() => {
    setToken('tok');
  });

  afterEach(() => {
    vi.unstubAllGlobals();
    vi.clearAllMocks();
  });

  it('renders the bell button and fetches with a Bearer token', async () => {
    const fetchMock = mockFetchOk(notifications);
    render(<NotificationBell />);

    expect(screen.getByRole('button', { name: 'label' })).toBeDefined();

    await waitFor(() => expect(screen.getByTestId('notification-badge')).toBeDefined());
    const [, init] = fetchMock.mock.calls[0];
    expect(init.headers.Authorization).toBe('Bearer tok');
  });

  it('shows the unread-count badge and opens the dropdown with items', async () => {
    mockFetchOk(notifications);
    render(<NotificationBell />);

    await waitFor(() =>
      expect(screen.getByTestId('notification-badge').textContent).toBe('2')
    );

    fireEvent.click(screen.getByRole('button', { name: 'label' }));

    expect(screen.getByText('SI-007')).toBeDefined();
    expect(screen.getByText('Steel bar 12mm')).toBeDefined();
  });

  it('navigates to the notification link when an item is clicked', async () => {
    mockFetchOk(notifications);
    render(<NotificationBell />);

    await waitFor(() => expect(screen.getByTestId('notification-badge')).toBeDefined());
    fireEvent.click(screen.getByRole('button', { name: 'label' }));
    fireEvent.click(screen.getByText('SI-007'));

    expect(pushMock).toHaveBeenCalledWith('/sales/invoices/inv007');
  });

  it('shows the empty state and no badge when there are no notifications', async () => {
    mockFetchOk([]);
    render(<NotificationBell />);

    // Let the fetch resolve, then open the dropdown.
    await waitFor(() => expect(screen.getByRole('button', { name: 'label' })).toBeDefined());
    fireEvent.click(screen.getByRole('button', { name: 'label' }));

    await waitFor(() => expect(screen.getByText('empty')).toBeDefined());
    expect(screen.queryByTestId('notification-badge')).toBeNull();
  });
});
