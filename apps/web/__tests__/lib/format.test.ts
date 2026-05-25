import { describe, it, expect } from 'vitest';
import {
  formatRsd,
  totalPages,
  invoiceStatusKey,
  isOverdue,
  statusBadgeClass,
} from '@/lib/format';

describe('formatRsd', () => {
  it('formats a number as RSD currency', () => {
    const result = formatRsd(150000);
    expect(result).toContain('150');
  });
  it('handles zero', () => {
    expect(formatRsd(0)).toContain('0');
  });
});

describe('totalPages', () => {
  it('computes ceil division', () => {
    expect(totalPages(45, 20)).toBe(3);
  });
  it('returns at least 1 for zero total', () => {
    expect(totalPages(0, 20)).toBe(1);
  });
  it('exact multiple', () => {
    expect(totalPages(40, 20)).toBe(2);
  });
});

describe('invoiceStatusKey', () => {
  it('maps Paid to statusPaid', () => {
    expect(invoiceStatusKey('Paid')).toBe('statusPaid');
  });
  it('maps Open to statusOpen', () => {
    expect(invoiceStatusKey('Open')).toBe('statusOpen');
  });
  it('defaults unknown to statusOpen', () => {
    expect(invoiceStatusKey('Whatever')).toBe('statusOpen');
  });
});

describe('isOverdue', () => {
  const today = '2026-05-25';

  it('paid invoices are never overdue', () => {
    expect(isOverdue('PAID', '2020-01-01', today)).toBe(false);
  });
  it('open invoice with past due date is overdue', () => {
    expect(isOverdue('OPEN', '2026-05-01', today)).toBe(true);
  });
  it('open invoice with future due date is not overdue', () => {
    expect(isOverdue('OPEN', '2026-06-30', today)).toBe(false);
  });
  it('partial invoice with past due date is overdue', () => {
    expect(isOverdue('PARTIAL', '2026-04-15', today)).toBe(true);
  });
  it('missing due date is not overdue', () => {
    expect(isOverdue('OPEN', '', today)).toBe(false);
  });
});

describe('statusBadgeClass', () => {
  it('PAID is green', () => {
    expect(statusBadgeClass('PAID')).toContain('green');
  });
  it('PARTIAL is amber', () => {
    expect(statusBadgeClass('PARTIAL')).toContain('amber');
  });
  it('OPEN is blue', () => {
    expect(statusBadgeClass('OPEN')).toContain('blue');
  });
  it('unknown defaults to blue', () => {
    expect(statusBadgeClass('WHATEVER')).toContain('blue');
  });
});
