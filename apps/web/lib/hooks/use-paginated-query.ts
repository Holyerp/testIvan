'use client';

import { useEffect, useMemo, useState } from 'react';
import { useAuthStore } from '@/lib/stores/auth-store';
import { totalPages } from '@/lib/format';
import { buildListQueryString } from '@/lib/query';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

export interface UsePaginatedQueryOptions {
  path: string; // e.g. '/api/v1/sales/invoices'
  pageSize?: number; // default 20
  initialSortBy?: string;
  initialSortDir?: 'asc' | 'desc';
  extraParams?: Record<string, string>; // tab/status filters etc.
}

export interface UsePaginatedQueryResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  isLoading: boolean;
  error: string | null;
  sortBy?: string;
  sortDir: 'asc' | 'desc';
  setPage: (p: number) => void;
  setSort: (key: string) => void; // toggles dir if same key
  setParam: (key: string, value: string) => void; // resets page to 1
}

interface PagedData<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

/**
 * Encapsulates the list-fetch pattern: holds page/sort/filter state, builds the
 * query string, fetches the canonical envelope `{ success, data: { items, total,
 * page, pageSize } }` with a Bearer token from the auth store. Matches the plain
 * useState/useEffect approach used across Phase 1 (no extra dependency added).
 */
export function usePaginatedQuery<T>(
  opts: UsePaginatedQueryOptions
): UsePaginatedQueryResult<T> {
  const accessToken = useAuthStore((s) => s.accessToken);
  const pageSize = opts.pageSize ?? 20;

  const [page, setPageState] = useState(1);
  const [sortBy, setSortBy] = useState<string | undefined>(opts.initialSortBy);
  const [sortDir, setSortDir] = useState<'asc' | 'desc'>(opts.initialSortDir ?? 'asc');
  const [params, setParams] = useState<Record<string, string>>(opts.extraParams ?? {});

  const [result, setResult] = useState<PagedData<T> | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Serialize params so the effect re-runs on any filter change.
  const paramsKey = useMemo(() => JSON.stringify(params), [params]);

  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    setError(null);

    const qs = buildListQueryString({ page, pageSize, sortBy, sortDir, params });
    const url = `${API_BASE_URL}${opts.path}?${qs}`;

    fetch(url, { headers: { Authorization: `Bearer ${accessToken}` } })
      .then(async (res) => {
        const json = await res.json();
        if (!res.ok || !json.success) {
          throw new Error(json.code ?? 'INTERNAL_ERROR');
        }
        return json.data as PagedData<T>;
      })
      .then((data) => {
        if (active) {
          setResult(data);
          setIsLoading(false);
        }
      })
      .catch((err: Error) => {
        if (active) {
          setError(err.message || 'INTERNAL_ERROR');
          setIsLoading(false);
        }
      });

    return () => {
      active = false;
    };
    // paramsKey stands in for the params object identity.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [accessToken, opts.path, page, pageSize, sortBy, sortDir, paramsKey]);

  const setPage = (p: number) => setPageState(p);

  const setSort = (key: string) => {
    if (sortBy === key) {
      setSortDir((d) => (d === 'asc' ? 'desc' : 'asc'));
    } else {
      setSortBy(key);
      setSortDir('asc');
    }
    setPageState(1);
  };

  const setParam = (key: string, value: string) => {
    setParams((prev) => ({ ...prev, [key]: value }));
    setPageState(1);
  };

  return {
    items: result?.items ?? [],
    total: result?.total ?? 0,
    page,
    pageSize,
    totalPages: totalPages(result?.total ?? 0, pageSize),
    isLoading,
    error,
    sortBy,
    sortDir,
    setPage,
    setSort,
    setParam,
  };
}
