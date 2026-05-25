import { describe, it, expect } from 'vitest';
import {
  notificationSeverityClass,
  unreadCount,
  type Notification,
} from '@/lib/notifications';

const overdue: Notification = {
  id: 'OVERDUE_INVOICE:inv007',
  type: 'OVERDUE_INVOICE',
  title: 'SI-007',
  message: 'Invoice SI-007 is overdue.',
  severity: 'WARNING',
  link: '/sales/invoices/inv007',
};

const lowStock: Notification = {
  id: 'LOW_STOCK:itm002',
  type: 'LOW_STOCK',
  title: 'Čelična šipka 12mm',
  message: 'Low on stock.',
  severity: 'WARNING',
  link: '/items/itm002',
};

describe('notificationSeverityClass', () => {
  it('maps INFO to the green accent', () => {
    expect(notificationSeverityClass('INFO')).toBe('bg-pine-green');
  });

  it('maps WARNING to amber', () => {
    expect(notificationSeverityClass('WARNING')).toBe('bg-amber-500');
  });

  it('maps CRITICAL to red', () => {
    expect(notificationSeverityClass('CRITICAL')).toBe('bg-red-500');
  });

  it('falls back to neutral gray for an unknown severity', () => {
    expect(notificationSeverityClass('SOMETHING_NEW')).toBe('bg-gray-400');
    expect(notificationSeverityClass('')).toBe('bg-gray-400');
  });
});

describe('unreadCount', () => {
  it('counts every notification', () => {
    expect(unreadCount([overdue, lowStock])).toBe(2);
  });

  it('returns 0 for an empty list', () => {
    expect(unreadCount([])).toBe(0);
  });

  it('returns 0 for null/undefined', () => {
    expect(unreadCount(null)).toBe(0);
    expect(unreadCount(undefined)).toBe(0);
  });

  it('counts a single notification', () => {
    expect(unreadCount([overdue])).toBe(1);
  });
});
