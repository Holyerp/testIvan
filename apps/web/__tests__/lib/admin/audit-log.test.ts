import { describe, it, expect } from 'vitest';
import {
  isAuditCategory,
  categoryLabelKey,
  categoryBadgeClass,
  formatAuditTimestamp,
  AUDIT_CATEGORIES,
} from '@/lib/admin/audit-log';

describe('isAuditCategory', () => {
  it('accepts the four known categories', () => {
    expect(isAuditCategory('LOGIN')).toBe(true);
    expect(isAuditCategory('VIEW')).toBe(true);
    expect(isAuditCategory('EXPORT')).toBe(true);
    expect(isAuditCategory('ADMIN')).toBe(true);
  });
  it('rejects unknown values and wrong casing', () => {
    expect(isAuditCategory('SOMETHING')).toBe(false);
    expect(isAuditCategory('login')).toBe(false);
    expect(isAuditCategory('')).toBe(false);
  });
});

describe('AUDIT_CATEGORIES', () => {
  it('lists exactly the four wire values', () => {
    expect(AUDIT_CATEGORIES).toEqual(['LOGIN', 'VIEW', 'EXPORT', 'ADMIN']);
  });
});

describe('categoryLabelKey', () => {
  it('returns the category unchanged when known', () => {
    expect(categoryLabelKey('LOGIN')).toBe('LOGIN');
    expect(categoryLabelKey('EXPORT')).toBe('EXPORT');
  });
  it('falls back to ADMIN for unknown values', () => {
    expect(categoryLabelKey('MYSTERY')).toBe('ADMIN');
    expect(categoryLabelKey('')).toBe('ADMIN');
  });
});

describe('categoryBadgeClass', () => {
  it('colors login blue', () => {
    expect(categoryBadgeClass('LOGIN')).toContain('blue');
  });
  it('colors export green', () => {
    expect(categoryBadgeClass('EXPORT')).toContain('green');
  });
  it('colors admin amber', () => {
    expect(categoryBadgeClass('ADMIN')).toContain('amber');
  });
  it('colors view gray', () => {
    expect(categoryBadgeClass('VIEW')).toContain('gray');
  });
  it('defaults unknown values to gray', () => {
    expect(categoryBadgeClass('WHATEVER')).toContain('gray');
  });
});

describe('formatAuditTimestamp', () => {
  it('returns the empty label for null/undefined/empty', () => {
    expect(formatAuditTimestamp(null, '—')).toBe('—');
    expect(formatAuditTimestamp(undefined, '—')).toBe('—');
    expect(formatAuditTimestamp('', '—')).toBe('—');
  });
  it('returns the empty label for an invalid date string', () => {
    expect(formatAuditTimestamp('not-a-date', '—')).toBe('—');
  });
  it('formats a valid ISO timestamp to a non-empty string', () => {
    const out = formatAuditTimestamp('2026-05-25T10:30:00Z', '—');
    expect(out).not.toBe('—');
    expect(out.length).toBeGreaterThan(0);
  });
});
