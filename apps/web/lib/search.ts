// Universal-search domain types + pure helpers, extracted so they can be unit-tested
// without rendering the component or mocking fetch. Mirrors the backend wire shape
// (SearchResultsDto / SearchHitDto) — `type` is the SCREAMING_SNAKE_CASE cross-layer enum.

export type SearchHitType =
  | 'CUSTOMER'
  | 'VENDOR'
  | 'SALES_INVOICE'
  | 'PURCHASE_INVOICE';

export interface SearchHit {
  id: string;
  type: SearchHitType;
  title: string;
  subtitle: string;
}

export interface SearchResults {
  customers: SearchHit[];
  vendors: SearchHit[];
  salesInvoices: SearchHit[];
  purchaseInvoices: SearchHit[];
}

// The group display order — also the order keyboard navigation walks through.
export const SEARCH_GROUPS: { key: keyof SearchResults; type: SearchHitType }[] = [
  { key: 'customers', type: 'CUSTOMER' },
  { key: 'vendors', type: 'VENDOR' },
  { key: 'salesInvoices', type: 'SALES_INVOICE' },
  { key: 'purchaseInvoices', type: 'PURCHASE_INVOICE' },
];

const EMPTY_RESULTS: SearchResults = {
  customers: [],
  vendors: [],
  salesInvoices: [],
  purchaseInvoices: [],
};

/**
 * Flatten the grouped results into a single ordered list (customers, vendors, sales
 * invoices, purchase invoices) for keyboard navigation. Pure — no side effects.
 */
export function flattenHits(results: SearchResults | null | undefined): SearchHit[] {
  const r = results ?? EMPTY_RESULTS;
  return SEARCH_GROUPS.flatMap((g) => r[g.key] ?? []);
}

/**
 * Map a hit to its detail route. The id is URL-encoded so ids with reserved characters
 * still produce a valid href.
 */
export function hitHref(hit: SearchHit): string {
  const id = encodeURIComponent(hit.id);
  switch (hit.type) {
    case 'CUSTOMER':
      return `/customers/${id}`;
    case 'VENDOR':
      return `/vendors/${id}`;
    case 'SALES_INVOICE':
      return `/sales/invoices/${id}`;
    case 'PURCHASE_INVOICE':
      return `/purchase/invoices/${id}`;
    default:
      return '/';
  }
}

/**
 * Wrap-around index movement for arrow-key navigation over `total` items.
 * Down past the end wraps to 0; up past 0 wraps to the last item. Returns 0 when
 * there are no items. `current` of -1 (nothing highlighted) + delta 1 → 0 (first).
 */
export function nextIndex(current: number, total: number, delta: number): number {
  if (total <= 0) return 0;
  // Normalize and wrap into [0, total).
  return (((current + delta) % total) + total) % total;
}
