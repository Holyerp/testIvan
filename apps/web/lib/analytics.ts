// Pure analytics helpers (US-020). Isolated from the page component so the delta /
// granularity / chart-scaling logic can be unit-tested without rendering. The page wires
// these into the analytics dashboard (charts, comparison cards, granularity toggle).

/** The granularity wire values (SCREAMING_SNAKE_CASE), aligned with the backend enum. */
export const GRANULARITIES = ['MONTHLY', 'QUARTERLY', 'YEARLY'] as const;
export type Granularity = (typeof GRANULARITIES)[number];

/**
 * Percentage delta of current vs prior, guarding a zero prior (returns 0 rather than
 * dividing by zero or reporting an infinite change). Mirrors the backend computation so
 * the UI reconciles with the API's `comparison.*DeltaPercent`. Rounded to 1 decimal place.
 * Pure — unit-tested.
 */
export function deltaPercent(current: number, prior: number): number {
  if (prior === 0) return 0;
  return Math.round(((current - prior) / prior) * 1000) / 10;
}

/**
 * Tailwind text color classes for a delta value: green when up (>0), red when down (<0),
 * neutral gray when flat (0). Used by the period-comparison cards. Pure — unit-tested.
 */
export function deltaClass(delta: number): string {
  if (delta > 0) return 'text-green-600';
  if (delta < 0) return 'text-red-600';
  return 'text-gray-500';
}

/**
 * The arrow glyph for a delta direction (up / down / flat). Pure — unit-tested.
 */
export function deltaArrow(delta: number): string {
  if (delta > 0) return '▲';
  if (delta < 0) return '▼';
  return '–';
}

/**
 * i18n key for a granularity wire value, resolved under `analytics.granularity.*`. Unknown
 * values fall back to MONTHLY so an unexpected value still renders a documented label.
 * Pure — unit-tested.
 */
export function granularityLabelKey(granularity: string): Granularity {
  return (GRANULARITIES as readonly string[]).includes(granularity)
    ? (granularity as Granularity)
    : 'MONTHLY';
}

/**
 * Bar height as a percentage (0–100) of the max value across the series, for the CSS bar
 * chart. Guards a non-positive max (returns 0 so an empty / all-zero series renders flat
 * bars rather than NaN heights). Pure — unit-tested.
 */
export function barHeightPercent(value: number, max: number): number {
  if (max <= 0) return 0;
  return Math.max(0, (value / max) * 100);
}

/** A revenue/expense point as returned by the API. */
export interface RevenueExpensePoint {
  period: string;
  revenue: number;
  expense: number;
  profit: number;
}

/**
 * The max value to scale the revenue/expense bars against: the largest revenue or expense
 * across all points (at least 1 so the chart never divides by zero). Pure — unit-tested.
 */
export function seriesMax(points: RevenueExpensePoint[]): number {
  return Math.max(1, ...points.map((p) => Math.max(p.revenue, p.expense)));
}
