import { describe, it, expect } from 'vitest';
import {
  deltaPercent,
  deltaClass,
  deltaArrow,
  granularityLabelKey,
  barHeightPercent,
  seriesMax,
} from '@/lib/analytics';

describe('deltaPercent', () => {
  it('computes a positive delta', () => {
    expect(deltaPercent(150, 100)).toBe(50);
  });
  it('computes a negative delta', () => {
    expect(deltaPercent(80, 100)).toBe(-20);
  });
  it('guards a zero prior (no divide-by-zero)', () => {
    expect(deltaPercent(100, 0)).toBe(0);
  });
  it('rounds to one decimal place', () => {
    expect(deltaPercent(133, 100)).toBe(33);
    expect(deltaPercent(1, 3)).toBe(-66.7);
  });
});

describe('deltaClass', () => {
  it('green when up', () => {
    expect(deltaClass(12)).toBe('text-green-600');
  });
  it('red when down', () => {
    expect(deltaClass(-5)).toBe('text-red-600');
  });
  it('neutral when flat', () => {
    expect(deltaClass(0)).toBe('text-gray-500');
  });
});

describe('deltaArrow', () => {
  it('up arrow when positive', () => {
    expect(deltaArrow(3)).toBe('▲');
  });
  it('down arrow when negative', () => {
    expect(deltaArrow(-3)).toBe('▼');
  });
  it('dash when flat', () => {
    expect(deltaArrow(0)).toBe('–');
  });
});

describe('granularityLabelKey', () => {
  it('passes through known values', () => {
    expect(granularityLabelKey('QUARTERLY')).toBe('QUARTERLY');
    expect(granularityLabelKey('YEARLY')).toBe('YEARLY');
  });
  it('falls back to MONTHLY for unknown values', () => {
    expect(granularityLabelKey('WEEKLY')).toBe('MONTHLY');
    expect(granularityLabelKey('')).toBe('MONTHLY');
  });
});

describe('barHeightPercent', () => {
  it('scales relative to max', () => {
    expect(barHeightPercent(50, 100)).toBe(50);
    expect(barHeightPercent(100, 100)).toBe(100);
  });
  it('guards a non-positive max', () => {
    expect(barHeightPercent(10, 0)).toBe(0);
  });
  it('never returns negative', () => {
    expect(barHeightPercent(-10, 100)).toBe(0);
  });
});

describe('seriesMax', () => {
  it('returns the largest revenue or expense across points', () => {
    const max = seriesMax([
      { period: '2025-01', revenue: 100, expense: 300, profit: -200 },
      { period: '2025-02', revenue: 500, expense: 200, profit: 300 },
    ]);
    expect(max).toBe(500);
  });
  it('returns at least 1 for an empty series', () => {
    expect(seriesMax([])).toBe(1);
  });
});
