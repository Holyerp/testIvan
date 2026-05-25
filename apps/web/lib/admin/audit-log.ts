// Audit-log view domain types + pure helpers (US-023), extracted so they can be unit-tested
// without rendering the page or mocking fetch. Mirrors the backend wire shape
// (AuditLogEntryDto). `category` is a SCREAMING_SNAKE_CASE cross-layer enum the UI maps to
// i18n labels under `auditLog.category.*`.

export type AuditCategory = 'LOGIN' | 'VIEW' | 'EXPORT' | 'ADMIN';

export const AUDIT_CATEGORIES: AuditCategory[] = ['LOGIN', 'VIEW', 'EXPORT', 'ADMIN'];

export interface AuditLogEntry {
  id: string;
  timestamp: string;
  username: string | null;
  action: string;
  category: AuditCategory;
  entityType: string | null;
  entityId: string | null;
  details: string | null;
}

/**
 * Whether a string is one of the four known audit categories. Pure — unit-tested. Used to
 * coerce an unknown wire value to a safe display fallback.
 */
export function isAuditCategory(value: string): value is AuditCategory {
  return (AUDIT_CATEGORIES as string[]).includes(value);
}

/**
 * The i18n key suffix for a category badge label, keyed by the SCREAMING_SNAKE wire value.
 * Unknown values fall back to ADMIN so an unexpected wire value still renders a documented
 * label rather than a raw code. Pure — unit-tested.
 */
export function categoryLabelKey(category: string): AuditCategory {
  return isAuditCategory(category) ? category : 'ADMIN';
}

/**
 * Tailwind classes for the category badge, keyed by the SCREAMING_SNAKE wire value:
 * LOGIN (blue), VIEW (gray), EXPORT (green), ADMIN (amber). Unknown values default to gray.
 * Pure — unit-tested.
 */
export function categoryBadgeClass(category: string): string {
  switch (category) {
    case 'LOGIN':
      return 'bg-blue-100 text-blue-700';
    case 'EXPORT':
      return 'bg-green-100 text-green-700';
    case 'ADMIN':
      return 'bg-amber-100 text-amber-700';
    case 'VIEW':
    default:
      return 'bg-gray-100 text-gray-600';
  }
}

/**
 * Format an ISO timestamp for display, returning a placeholder for empty/invalid values.
 * Pure (locale formatting is deterministic for a given runtime locale). Unit-tested for the
 * empty/invalid branches.
 */
export function formatAuditTimestamp(iso: string | null | undefined, emptyLabel: string): string {
  if (!iso) return emptyLabel;
  const d = new Date(iso);
  return Number.isNaN(d.getTime()) ? emptyLabel : d.toLocaleString();
}
