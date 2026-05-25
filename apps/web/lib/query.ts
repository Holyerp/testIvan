export interface ListQueryParams {
  page: number;
  pageSize: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
  // Arbitrary filter params (search, status, etc.). Empty/undefined values are omitted.
  params?: Record<string, string | undefined>;
}

/**
 * Build the query string for a paginated list request.
 *
 * Always emits `page` and `pageSize`; emits `sortBy`/`sortDir` only when both are set;
 * emits each filter param only when it has a non-empty value. Values are URL-encoded.
 * Pure function — extracted so it can be unit-tested without mocking fetch.
 */
export function buildListQueryString(opts: ListQueryParams): string {
  const parts: string[] = [
    `page=${opts.page}`,
    `pageSize=${opts.pageSize}`,
  ];

  if (opts.sortBy) {
    parts.push(`sortBy=${encodeURIComponent(opts.sortBy)}`);
    parts.push(`sortDir=${opts.sortDir === 'desc' ? 'desc' : 'asc'}`);
  }

  for (const [key, value] of Object.entries(opts.params ?? {})) {
    if (value !== undefined && value !== '') {
      parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(value)}`);
    }
  }

  return parts.join('&');
}
