import { describe, it, expect } from 'vitest';
import { formatRsd, totalPages } from '@/lib/format';

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
