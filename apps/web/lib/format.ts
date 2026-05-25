export function formatRsd(amount: number): string {
  return new Intl.NumberFormat('sr-RS', {
    style: 'currency',
    currency: 'RSD',
    maximumFractionDigits: 0,
  }).format(amount);
}

export function totalPages(total: number, pageSize: number): number {
  return Math.max(1, Math.ceil(total / pageSize));
}

export function invoiceStatusKey(status: string): string {
  return status === 'Paid' ? 'statusPaid' : 'statusOpen';
}

/**
 * An invoice is overdue when it is not fully paid and its due date is in the past.
 * `today` is injected (ISO yyyy-MM-dd) so the function stays pure and testable.
 */
export function isOverdue(status: string, dueDate: string, today: string): boolean {
  if (status === 'PAID') return false;
  if (!dueDate) return false;
  return dueDate < today;
}

/** A sales-invoice line as rendered on the detail screen. */
export interface InvoiceLine {
  vatPercent: number;
  lineTotal: number;
}

/** Computed invoice totals (subtotal excl. VAT, VAT amount, grand total). */
export interface InvoiceTotals {
  subtotal: number;
  vatAmount: number;
  total: number;
}

/**
 * Derive invoice totals from its line items (subtotal = Σ lineTotal,
 * vatAmount = Σ lineTotal × vatPercent / 100, total = subtotal + vatAmount).
 * Pure function — mirrors the backend computation so the UI can reconcile / fall back.
 */
export function computeInvoiceTotals(lines: InvoiceLine[]): InvoiceTotals {
  const subtotal = lines.reduce((sum, l) => sum + l.lineTotal, 0);
  const vatAmount = lines.reduce((sum, l) => sum + (l.lineTotal * l.vatPercent) / 100, 0);
  return { subtotal, vatAmount, total: subtotal + vatAmount };
}

/**
 * Tailwind classes for the warehouse low-stock row highlight (US-017). Items whose
 * quantity on hand is below their minimum stock get an amber tint; everything else
 * gets no extra row classes. Pure function — extracted so it can be unit-tested.
 */
export function lowStockRowClass(isLowStock: boolean): string {
  return isLowStock ? 'bg-amber-50' : '';
}

/** Tailwind classes for the status badge, keyed by the SCREAMING_SNAKE wire value. */
export function statusBadgeClass(status: string): string {
  switch (status) {
    case 'PAID':
      return 'bg-green-100 text-green-700';
    case 'PARTIAL':
      return 'bg-amber-100 text-amber-700';
    case 'OPEN':
    default:
      return 'bg-blue-100 text-blue-700';
  }
}

/**
 * Tailwind classes for the credit-memo status badge. Credit memos use the
 * OPEN | POSTED lifecycle (not the invoice OPEN/PARTIAL/PAID one): POSTED is
 * green (booked), OPEN is blue (draft). Unknown values default to blue.
 */
export function creditMemoStatusBadgeClass(status: string): string {
  switch (status) {
    case 'POSTED':
      return 'bg-green-100 text-green-700';
    case 'OPEN':
    default:
      return 'bg-blue-100 text-blue-700';
  }
}

/**
 * i18n key for an item ledger entry type (US-018), keyed by the SCREAMING_SNAKE wire
 * value (PURCHASE | SALE | ADJUSTMENT | TRANSFER). The UI resolves it under the
 * `items.ledgerType.*` namespace; unknown values fall back to ADJUSTMENT so an unexpected
 * wire value still renders a documented label rather than a raw key.
 */
export function ledgerTypeKey(type: string): string {
  switch (type) {
    case 'PURCHASE':
    case 'SALE':
    case 'TRANSFER':
      return type;
    case 'ADJUSTMENT':
    default:
      return 'ADJUSTMENT';
  }
}

/**
 * Format a signed ledger quantity for display: positive (inbound) movements get a leading
 * `+`, negatives keep their `-`, and zero renders without a sign. Pure — unit-tested.
 */
export function formatSignedQuantity(quantity: number): string {
  if (quantity > 0) return `+${quantity}`;
  return `${quantity}`;
}

/**
 * Tailwind classes for the credit-document type badge (US-016), keyed by the
 * SCREAMING_SNAKE wire value: CREDIT_MEMO (green — a credit to the party),
 * DEBIT_MEMO (amber — an additional charge), STORNO (red — a cancellation).
 * Unknown values default to gray.
 */
export function creditDocumentTypeBadgeClass(type: string): string {
  switch (type) {
    case 'CREDIT_MEMO':
      return 'bg-green-100 text-green-700';
    case 'DEBIT_MEMO':
      return 'bg-amber-100 text-amber-700';
    case 'STORNO':
      return 'bg-red-100 text-red-700';
    default:
      return 'bg-gray-100 text-gray-700';
  }
}
