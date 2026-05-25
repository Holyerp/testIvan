'use client';

import { useEffect, useMemo, useRef, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd, totalPages } from '@/lib/format';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';
const PAGE_SIZE = 20;

interface CustomerListItem {
  id: string;
  number: string;
  displayName: string;
  city: string;
  balance: number;
  balanceDue: number;
}

interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

type SortField = 'displayName' | 'number';
type SortDir = 'asc' | 'desc';

function SkeletonRows() {
  return (
    <>
      {Array.from({ length: 8 }).map((_, i) => (
        <tr key={i} className="border-b border-gray-100 animate-pulse">
          {Array.from({ length: 5 }).map((__, j) => (
            <td key={j} className="px-4 py-3">
              <div className="h-4 w-full max-w-[8rem] rounded bg-gray-200" />
            </td>
          ))}
        </tr>
      ))}
    </>
  );
}

export default function CustomersPage() {
  const t = useTranslations('customers');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const accessToken = useAuthStore((s) => s.accessToken);

  const [searchInput, setSearchInput] = useState('');
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const [sortBy, setSortBy] = useState<SortField>('displayName');
  const [sortDir, setSortDir] = useState<SortDir>('asc');

  const [result, setResult] = useState<PagedResult<CustomerListItem> | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  // Debounce the search input (~400ms) and reset to page 1 on change.
  const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      setSearch(searchInput);
      setPage(1);
    }, 400);
    return () => {
      if (debounceRef.current) clearTimeout(debounceRef.current);
    };
  }, [searchInput]);

  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    setHasError(false);

    const url =
      `${API_BASE_URL}/api/v1/customers?page=${page}&pageSize=${PAGE_SIZE}` +
      `&search=${encodeURIComponent(search)}&sortBy=${sortBy}&sortDir=${sortDir}`;

    fetch(url, { headers: { Authorization: `Bearer ${accessToken}` } })
      .then(async (res) => {
        const json = await res.json();
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as PagedResult<CustomerListItem>;
      })
      .then((data) => {
        if (active) {
          setResult(data);
          setIsLoading(false);
        }
      })
      .catch(() => {
        if (active) {
          setHasError(true);
          setIsLoading(false);
        }
      });

    return () => {
      active = false;
    };
  }, [accessToken, page, search, sortBy, sortDir]);

  const pageCount = useMemo(
    () => totalPages(result?.total ?? 0, PAGE_SIZE),
    [result?.total]
  );

  if (!user) return null;

  const toggleSort = (field: SortField) => {
    if (sortBy === field) {
      setSortDir((d) => (d === 'asc' ? 'desc' : 'asc'));
    } else {
      setSortBy(field);
      setSortDir('asc');
    }
    setPage(1);
  };

  const sortIndicator = (field: SortField) =>
    sortBy === field ? (sortDir === 'asc' ? ' ▲' : ' ▼') : '';

  const items = result?.items ?? [];
  const showEmpty = !isLoading && !hasError && items.length === 0;

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      <div className="mt-6">
        <input
          type="search"
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          placeholder={t('search')}
          aria-label={t('search')}
          className="w-full max-w-sm rounded-lg border border-gray-200 px-4 py-2 text-sm focus:border-pine-green focus:outline-none focus:ring-1 focus:ring-pine-green"
        />
      </div>

      {hasError && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {t('loadError')}
        </div>
      )}

      <div className="mt-6 overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
        <table className="w-full text-left text-sm">
          <thead>
            <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
              <th className="px-4 py-3">
                <button
                  type="button"
                  onClick={() => toggleSort('number')}
                  className="font-medium hover:text-pine-navy"
                >
                  {t('number')}
                  {sortIndicator('number')}
                </button>
              </th>
              <th className="px-4 py-3">
                <button
                  type="button"
                  onClick={() => toggleSort('displayName')}
                  className="font-medium hover:text-pine-navy"
                >
                  {t('name')}
                  {sortIndicator('displayName')}
                </button>
              </th>
              <th className="px-4 py-3 font-medium">{t('city')}</th>
              <th className="px-4 py-3 text-right font-medium">{t('balance')}</th>
              <th className="px-4 py-3 text-right font-medium">{t('balanceDue')}</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <SkeletonRows />
            ) : showEmpty ? (
              <tr>
                <td colSpan={5} className="px-4 py-10 text-center text-gray-400">
                  {t('noResults')}
                </td>
              </tr>
            ) : (
              items.map((c) => (
                <tr key={c.id} className="border-b border-gray-50 last:border-0 hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium text-pine-navy">{c.number}</td>
                  <td className="px-4 py-3 text-gray-700">{c.displayName}</td>
                  <td className="px-4 py-3 text-gray-500">{c.city}</td>
                  <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                    {formatRsd(c.balance)}
                  </td>
                  <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                    {formatRsd(c.balanceDue)}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <div className="mt-4 flex items-center justify-between text-sm">
        <span className="text-gray-500">
          {t('pageOf', { page, total: pageCount })}
        </span>
        <div className="flex gap-2">
          <button
            type="button"
            onClick={() => setPage((p) => Math.max(1, p - 1))}
            disabled={page <= 1 || isLoading}
            className="rounded-lg border border-gray-200 px-3 py-1.5 font-medium text-gray-600 disabled:cursor-not-allowed disabled:opacity-40 hover:bg-gray-50"
          >
            {t('previous')}
          </button>
          <button
            type="button"
            onClick={() => setPage((p) => Math.min(pageCount, p + 1))}
            disabled={page >= pageCount || isLoading}
            className="rounded-lg border border-gray-200 px-3 py-1.5 font-medium text-gray-600 disabled:cursor-not-allowed disabled:opacity-40 hover:bg-gray-50"
          >
            {t('next')}
          </button>
        </div>
      </div>
    </div>
  );
}
