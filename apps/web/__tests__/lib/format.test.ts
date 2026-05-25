import { describe, it, expect } from 'vitest';
import {
  formatRsd,
  totalPages,
  invoiceStatusKey,
  isOverdue,
  statusBadgeClass,
  creditMemoStatusBadgeClass,
  creditDocumentTypeBadgeClass,
  computeInvoiceTotals,
  lowStockRowClass,
  ledgerTypeKey,
  formatSignedQuantity,
  sortByQuantityAsc,
  belowMinimumCardClass,
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

describe('computeInvoiceTotals', () => {
  it('sums line totals into subtotal and applies VAT per line', () => {
    const result = computeInvoiceTotals([
      { vatPercent: 20, lineTotal: 2000 },
      { vatPercent: 20, lineTotal: 3000 },
    ]);
    expect(result.subtotal).toBe(5000);
    expect(result.vatAmount).toBe(1000);
    expect(result.total).toBe(6000);
  });

  it('handles mixed VAT rates', () => {
    const result = computeInvoiceTotals([
      { vatPercent: 20, lineTotal: 1000 },
      { vatPercent: 10, lineTotal: 1000 },
    ]);
    expect(result.subtotal).toBe(2000);
    expect(result.vatAmount).toBe(300);
    expect(result.total).toBe(2300);
  });

  it('returns zeros for an empty line list', () => {
    const result = computeInvoiceTotals([]);
    expect(result.subtotal).toBe(0);
    expect(result.vatAmount).toBe(0);
    expect(result.total).toBe(0);
  });

  it('total always equals subtotal plus vat', () => {
    const result = computeInvoiceTotals([{ vatPercent: 18, lineTotal: 1234.5 }]);
    expect(result.total).toBeCloseTo(result.subtotal + result.vatAmount, 5);
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

describe('creditMemoStatusBadgeClass', () => {
  it('POSTED is green', () => {
    expect(creditMemoStatusBadgeClass('POSTED')).toContain('green');
  });
  it('OPEN is blue', () => {
    expect(creditMemoStatusBadgeClass('OPEN')).toContain('blue');
  });
  it('unknown defaults to blue', () => {
    expect(creditMemoStatusBadgeClass('WHATEVER')).toContain('blue');
  });
});

describe('creditDocumentTypeBadgeClass', () => {
  it('CREDIT_MEMO is green', () => {
    expect(creditDocumentTypeBadgeClass('CREDIT_MEMO')).toContain('green');
  });
  it('DEBIT_MEMO is amber', () => {
    expect(creditDocumentTypeBadgeClass('DEBIT_MEMO')).toContain('amber');
  });
  it('STORNO is red', () => {
    expect(creditDocumentTypeBadgeClass('STORNO')).toContain('red');
  });
  it('unknown defaults to gray', () => {
    expect(creditDocumentTypeBadgeClass('WHATEVER')).toContain('gray');
  });
});

describe('lowStockRowClass', () => {
  it('returns an amber tint when low stock', () => {
    expect(lowStockRowClass(true)).toContain('amber');
  });
  it('returns no extra classes when not low stock', () => {
    expect(lowStockRowClass(false)).toBe('');
  });
});

describe('ledgerTypeKey', () => {
  it('passes known wire values through', () => {
    expect(ledgerTypeKey('PURCHASE')).toBe('PURCHASE');
    expect(ledgerTypeKey('SALE')).toBe('SALE');
    expect(ledgerTypeKey('TRANSFER')).toBe('TRANSFER');
    expect(ledgerTypeKey('ADJUSTMENT')).toBe('ADJUSTMENT');
  });
  it('falls back to ADJUSTMENT for unknown values', () => {
    expect(ledgerTypeKey('WHATEVER')).toBe('ADJUSTMENT');
  });
});

describe('formatSignedQuantity', () => {
  it('prefixes positive quantities with +', () => {
    expect(formatSignedQuantity(20)).toBe('+20');
  });
  it('keeps the minus sign for negatives', () => {
    expect(formatSignedQuantity(-5)).toBe('-5');
  });
  it('renders zero without a sign', () => {
    expect(formatSignedQuantity(0)).toBe('0');
  });
});

describe('sortByQuantityAsc', () => {
  it('orders items by quantity on hand ascending', () => {
    const result = sortByQuantityAsc([
      { quantityOnHand: 30 },
      { quantityOnHand: 5 },
      { quantityOnHand: 12 },
    ]);
    expect(result.map((i) => i.quantityOnHand)).toEqual([5, 12, 30]);
  });
  it('does not mutate the input array', () => {
    const input = [{ quantityOnHand: 3 }, { quantityOnHand: 1 }];
    sortByQuantityAsc(input);
    expect(input.map((i) => i.quantityOnHand)).toEqual([3, 1]);
  });
  it('handles an empty list', () => {
    expect(sortByQuantityAsc([])).toEqual([]);
  });
});

describe('belowMinimumCardClass', () => {
  it('uses amber when there are items below minimum', () => {
    expect(belowMinimumCardClass(4)).toContain('amber');
  });
  it('uses the neutral navy color when none are below minimum', () => {
    expect(belowMinimumCardClass(0)).toContain('pine-navy');
  });
});
