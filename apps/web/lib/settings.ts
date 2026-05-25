// Admin Settings domain types + pure helpers (US-022), extracted so they can be unit-tested
// without rendering the page or mocking fetch. Mirrors the backend wire shape (BcConfigDto /
// BcConnectionTestResultDto). SECURITY: the BC credentials are never part of the wire shape —
// the backend omits ClientId / ClientSecret, so there is nothing to render here.

/** Per-entity last-successful-sync row shown in the "Last Sync" table. */
export interface EntitySyncStatus {
  entityType: string;
  lastSyncedAt: string | null;
}

/** Non-sensitive BC connection config (matches the backend BcConfigDto). */
export interface BcConfig {
  tenantId: string;
  companyId: string;
  environment: string;
  baseUrl: string;
  useMock: boolean;
  lastSync: EntitySyncStatus[];
}

/** Result of an on-demand connectivity probe (matches BcConnectionTestResultDto). */
export interface BcConnectionTestResult {
  success: boolean;
  durationMs: number;
  lastSuccessfulSyncAt: string | null;
  errorCode: string | null;
}

/**
 * Tailwind classes for the connection-mode badge: amber for Mock (synthetic data), green for
 * Live (real BC). Mirrors the dashboard status badge. Pure — unit-tested.
 */
export function connectionModeBadgeClass(useMock: boolean): string {
  return useMock
    ? 'bg-amber-100 text-amber-700'
    : 'bg-pine-green-pale text-pine-green-dark';
}

/**
 * i18n key (under `settings.*`) for the connection-mode label. Pure — unit-tested.
 */
export function connectionModeKey(useMock: boolean): string {
  return useMock ? 'mock' : 'live';
}

/**
 * Format a BC entity-type wire value (e.g. "salesInvoices") for display: insert a space before
 * each capital and capitalize the first letter ("Sales Invoices"). Used as a defensive fallback
 * when no localized label exists. Pure — unit-tested.
 */
export function formatEntityType(entityType: string): string {
  if (!entityType) return '';
  const spaced = entityType.replace(/([A-Z])/g, ' $1');
  return spaced.charAt(0).toUpperCase() + spaced.slice(1).trim();
}

/**
 * Format an ISO timestamp for display, returning the supplied "never" label when the timestamp
 * is null/empty/unparseable. Pure (locale formatting aside) — unit-tested for the null path.
 */
export function formatSyncTimestamp(iso: string | null, neverLabel: string): string {
  if (!iso) return neverLabel;
  const d = new Date(iso);
  return Number.isNaN(d.getTime()) ? neverLabel : d.toLocaleString();
}
