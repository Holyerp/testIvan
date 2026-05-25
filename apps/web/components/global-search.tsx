'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useAuthStore } from '@/lib/stores/auth-store';
import {
  flattenHits,
  hitHref,
  nextIndex,
  SEARCH_GROUPS,
  type SearchHit,
  type SearchResults,
} from '@/lib/search';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';
const MIN_CHARS = 2;
const DEBOUNCE_MS = 300;
const LIMIT = 5;

// Where each group's "View all" link points (the existing list screen, query prefilled).
const VIEW_ALL_PATH: Record<string, string> = {
  customers: '/customers',
  vendors: '/vendors',
  salesInvoices: '/sales/invoices',
  purchaseInvoices: '/purchase/invoices',
};

export function GlobalSearch() {
  const t = useTranslations('search');
  const router = useRouter();
  const accessToken = useAuthStore((s) => s.accessToken);

  const [query, setQuery] = useState('');
  const [results, setResults] = useState<SearchResults | null>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [highlight, setHighlight] = useState(-1);

  const inputRef = useRef<HTMLInputElement | null>(null);
  const containerRef = useRef<HTMLDivElement | null>(null);

  const flat = useMemo(() => flattenHits(results), [results]);
  const trimmed = query.trim();
  const hasResults = flat.length > 0;
  const showEmpty = trimmed.length >= MIN_CHARS && !isLoading && !hasResults;

  // Cmd/Ctrl+K focuses the input from anywhere.
  useEffect(() => {
    function onKeyDown(e: KeyboardEvent) {
      if ((e.metaKey || e.ctrlKey) && e.key.toLowerCase() === 'k') {
        e.preventDefault();
        inputRef.current?.focus();
      }
    }
    window.addEventListener('keydown', onKeyDown);
    return () => window.removeEventListener('keydown', onKeyDown);
  }, []);

  // Click outside closes the panel.
  useEffect(() => {
    function onClick(e: MouseEvent) {
      if (containerRef.current && !containerRef.current.contains(e.target as Node)) {
        setIsOpen(false);
      }
    }
    document.addEventListener('mousedown', onClick);
    return () => document.removeEventListener('mousedown', onClick);
  }, []);

  // Debounced fetch. Below the threshold, clear results and show nothing.
  useEffect(() => {
    if (trimmed.length < MIN_CHARS) {
      setResults(null);
      setIsLoading(false);
      return;
    }
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    const handle = setTimeout(() => {
      const url = `${API_BASE_URL}/api/v1/search?q=${encodeURIComponent(trimmed)}&limit=${LIMIT}`;
      fetch(url, { headers: { Authorization: `Bearer ${accessToken}` } })
        .then(async (res) => {
          const json = await res.json();
          if (!res.ok || !json.success) throw new Error(json.code ?? 'INTERNAL_ERROR');
          return json.data as SearchResults;
        })
        .then((data) => {
          if (!active) return;
          setResults(data);
          setHighlight(-1);
          setIsLoading(false);
          setIsOpen(true);
        })
        .catch(() => {
          if (!active) return;
          setResults(null);
          setIsLoading(false);
          setIsOpen(true);
        });
    }, DEBOUNCE_MS);

    return () => {
      active = false;
      clearTimeout(handle);
    };
  }, [trimmed, accessToken]);

  const close = useCallback(() => {
    setIsOpen(false);
    inputRef.current?.blur();
  }, []);

  const navigateToHit = useCallback(
    (hit: SearchHit) => {
      close();
      setQuery('');
      setResults(null);
      router.push(hitHref(hit));
    },
    [close, router]
  );

  function onInputKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === 'Escape') {
      e.preventDefault();
      close();
      return;
    }
    if (!isOpen || flat.length === 0) return;

    if (e.key === 'ArrowDown') {
      e.preventDefault();
      setHighlight((i) => nextIndex(i, flat.length, 1));
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      setHighlight((i) => nextIndex(i, flat.length, -1));
    } else if (e.key === 'Enter') {
      if (highlight >= 0 && highlight < flat.length) {
        e.preventDefault();
        navigateToHit(flat[highlight]);
      }
    }
  }

  // Stable per-hit index into the flattened list, so the highlight maps across groups.
  let runningIndex = -1;

  return (
    <div ref={containerRef} className="relative w-full max-w-md">
      <input
        ref={inputRef}
        type="search"
        role="combobox"
        aria-expanded={isOpen}
        aria-controls="global-search-listbox"
        aria-label={t('placeholder')}
        placeholder={t('placeholder')}
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        onFocus={() => {
          if (trimmed.length >= MIN_CHARS) setIsOpen(true);
        }}
        onKeyDown={onInputKeyDown}
        className="w-full rounded-lg border border-gray-200 bg-white px-3 py-2 text-sm text-pine-navy placeholder-gray-400 focus:border-pine-green focus:outline-none focus:ring-1 focus:ring-pine-green"
      />

      {isOpen && trimmed.length >= MIN_CHARS && (
        <div
          id="global-search-listbox"
          role="listbox"
          className="absolute z-50 mt-2 max-h-[70vh] w-full overflow-auto rounded-lg border border-gray-200 bg-white shadow-lg"
        >
          {isLoading && (
            <div className="px-4 py-3 text-sm text-gray-500">{t('loading')}</div>
          )}

          {!isLoading && showEmpty && (
            <div className="px-4 py-3 text-sm text-gray-500">{t('noResults')}</div>
          )}

          {!isLoading &&
            hasResults &&
            SEARCH_GROUPS.map((group) => {
              const hits = results?.[group.key] ?? [];
              if (hits.length === 0) return null;

              return (
                <div key={group.key} className="border-b border-gray-100 last:border-b-0">
                  <div className="flex items-center justify-between px-4 pt-3 pb-1">
                    <span className="text-xs font-semibold uppercase tracking-wide text-gray-400">
                      {t(`group.${group.type}`)}
                    </span>
                    <Link
                      href={`${VIEW_ALL_PATH[group.key]}?search=${encodeURIComponent(trimmed)}`}
                      onClick={close}
                      className="text-xs font-medium text-pine-green hover:underline"
                    >
                      {t('viewAll')}
                    </Link>
                  </div>
                  <ul>
                    {hits.map((hit) => {
                      runningIndex += 1;
                      const index = runningIndex;
                      const selected = index === highlight;
                      return (
                        <li key={`${hit.type}-${hit.id}`} role="option" aria-selected={selected}>
                          <button
                            type="button"
                            onClick={() => navigateToHit(hit)}
                            onMouseEnter={() => setHighlight(index)}
                            className={`flex w-full flex-col items-start px-4 py-2 text-left text-sm ${
                              selected ? 'bg-pine-green/10' : 'hover:bg-gray-50'
                            }`}
                          >
                            <span className="font-medium text-pine-navy">{hit.title}</span>
                            {hit.subtitle && (
                              <span className="text-xs text-gray-500">{hit.subtitle}</span>
                            )}
                          </button>
                        </li>
                      );
                    })}
                  </ul>
                </div>
              );
            })}
        </div>
      )}
    </div>
  );
}
