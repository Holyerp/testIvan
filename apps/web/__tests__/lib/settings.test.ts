import { describe, it, expect } from 'vitest';
import {
  connectionModeBadgeClass,
  connectionModeKey,
  formatEntityType,
  formatSyncTimestamp,
} from '@/lib/settings';

describe('connectionModeBadgeClass', () => {
  it('returns amber classes for mock mode', () => {
    expect(connectionModeBadgeClass(true)).toContain('amber');
  });
  it('returns green classes for live mode', () => {
    expect(connectionModeBadgeClass(false)).toContain('pine-green');
  });
});

describe('connectionModeKey', () => {
  it('maps mock mode to the mock key', () => {
    expect(connectionModeKey(true)).toBe('mock');
  });
  it('maps live mode to the live key', () => {
    expect(connectionModeKey(false)).toBe('live');
  });
});

describe('formatEntityType', () => {
  it('splits camelCase into words and capitalizes', () => {
    expect(formatEntityType('salesInvoices')).toBe('Sales Invoices');
  });
  it('capitalizes a single lowercase word', () => {
    expect(formatEntityType('customers')).toBe('Customers');
  });
  it('returns empty string for empty input', () => {
    expect(formatEntityType('')).toBe('');
  });
});

describe('formatSyncTimestamp', () => {
  it('returns the never label for null', () => {
    expect(formatSyncTimestamp(null, 'Never')).toBe('Never');
  });
  it('returns the never label for an unparseable value', () => {
    expect(formatSyncTimestamp('not-a-date', 'Never')).toBe('Never');
  });
  it('formats a valid ISO timestamp into a non-never string', () => {
    const result = formatSyncTimestamp('2026-05-25T10:00:00.000Z', 'Never');
    expect(result).not.toBe('Never');
    expect(result.length).toBeGreaterThan(0);
  });
});
