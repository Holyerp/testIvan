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
