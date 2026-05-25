// In-app notification domain types + pure helpers (T-005), extracted so they can be
// unit-tested without rendering the bell or mocking fetch. Mirrors the backend wire
// shape (NotificationDto) — `type` and `severity` are SCREAMING_SNAKE_CASE cross-layer
// enums the UI maps to i18n labels.

export type NotificationType = 'OVERDUE_INVOICE' | 'LOW_STOCK';
export type NotificationSeverity = 'INFO' | 'WARNING' | 'CRITICAL';

export interface Notification {
  id: string;
  type: NotificationType;
  title: string;
  message: string;
  severity: NotificationSeverity;
  link?: string | null;
}

// Tailwind classes per severity (dot / accent). CRITICAL is the loudest.
const SEVERITY_CLASS: Record<string, string> = {
  INFO: 'bg-pine-green',
  WARNING: 'bg-amber-500',
  CRITICAL: 'bg-red-500',
};

const SEVERITY_CLASS_FALLBACK = 'bg-gray-400';

/**
 * The Tailwind background class for a severity dot. Unknown severities fall back to a
 * neutral gray so a future backend value never renders an empty class. Pure.
 */
export function notificationSeverityClass(severity: string): string {
  return SEVERITY_CLASS[severity] ?? SEVERITY_CLASS_FALLBACK;
}

/**
 * The unread badge count for the bell. Today every fetched notification is unread (the
 * backend returns only actionable alerts and we do not persist read-state yet), so this
 * is the list length — capped is left to the UI. Pure. Null/undefined → 0.
 */
export function unreadCount(notifications: Notification[] | null | undefined): number {
  return notifications?.length ?? 0;
}
