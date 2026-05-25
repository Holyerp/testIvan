'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useAuthStore } from '@/lib/stores/auth-store';
import {
  notificationSeverityClass,
  unreadCount,
  type Notification,
} from '@/lib/notifications';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

export function NotificationBell() {
  const t = useTranslations('notifications');
  const router = useRouter();
  const accessToken = useAuthStore((s) => s.accessToken);

  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const containerRef = useRef<HTMLDivElement | null>(null);

  // Fetch the actionable-alert list once a token is present.
  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    fetch(`${API_BASE_URL}/api/v1/notifications`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    })
      .then(async (res) => {
        const json = await res.json();
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as Notification[];
      })
      .then((data) => {
        if (!active) return;
        setNotifications(data);
        setIsLoading(false);
      })
      .catch(() => {
        if (!active) return;
        setNotifications([]);
        setIsLoading(false);
      });

    return () => {
      active = false;
    };
  }, [accessToken]);

  // Click outside closes the dropdown.
  useEffect(() => {
    function onClick(e: MouseEvent) {
      if (containerRef.current && !containerRef.current.contains(e.target as Node)) {
        setIsOpen(false);
      }
    }
    document.addEventListener('mousedown', onClick);
    return () => document.removeEventListener('mousedown', onClick);
  }, []);

  const count = unreadCount(notifications);

  const navigateTo = useCallback(
    (link: string | null | undefined) => {
      setIsOpen(false);
      if (link) router.push(link);
    },
    [router]
  );

  return (
    <div ref={containerRef} className="relative">
      <button
        type="button"
        aria-label={t('label')}
        aria-haspopup="true"
        aria-expanded={isOpen}
        onClick={() => setIsOpen((o) => !o)}
        className="relative flex h-10 w-10 items-center justify-center rounded-lg text-pine-navy hover:bg-gray-100 focus:outline-none focus:ring-1 focus:ring-pine-green"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="h-5 w-5"
          aria-hidden="true"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M14.857 17.082a23.848 23.848 0 0 0 5.454-1.31A8.967 8.967 0 0 1 18 9.75V9A6 6 0 0 0 6 9v.75a8.967 8.967 0 0 1-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 0 1-5.714 0m5.714 0a3 3 0 1 1-5.714 0"
          />
        </svg>
        {count > 0 && (
          <span
            data-testid="notification-badge"
            className="absolute -right-0.5 -top-0.5 flex h-4 min-w-[1rem] items-center justify-center rounded-full bg-red-500 px-1 text-[10px] font-semibold text-white"
          >
            {count}
          </span>
        )}
      </button>

      {isOpen && (
        <div
          role="menu"
          aria-label={t('label')}
          className="absolute right-0 z-50 mt-2 max-h-[70vh] w-80 overflow-auto rounded-lg border border-gray-200 bg-white shadow-lg"
        >
          <div className="border-b border-gray-100 px-4 py-3">
            <span className="text-xs font-semibold uppercase tracking-wide text-gray-400">
              {t('title')}
            </span>
          </div>

          {isLoading && (
            <div className="px-4 py-3 text-sm text-gray-500">{t('loading')}</div>
          )}

          {!isLoading && count === 0 && (
            <div className="px-4 py-6 text-center text-sm text-gray-500">{t('empty')}</div>
          )}

          {!isLoading && count > 0 && (
            <ul>
              {notifications.map((n) => (
                <li key={n.id} role="none" className="border-b border-gray-50 last:border-0">
                  <button
                    type="button"
                    role="menuitem"
                    onClick={() => navigateTo(n.link)}
                    className="flex w-full items-start gap-3 px-4 py-3 text-left hover:bg-gray-50"
                  >
                    <span
                      aria-hidden="true"
                      className={`mt-1.5 h-2 w-2 shrink-0 rounded-full ${notificationSeverityClass(n.severity)}`}
                    />
                    <span className="flex flex-col">
                      <span className="text-sm font-medium text-pine-navy">{n.title}</span>
                      <span className="text-xs text-gray-500">{n.message}</span>
                    </span>
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
}
