'use client';

import React from 'react';

export interface Column<T> {
  key: string;
  header: string; // already-translated label
  render: (row: T) => React.ReactNode;
  sortable?: boolean;
  align?: 'left' | 'right';
}

export interface EntityTableProps<T> {
  columns: Column<T>[];
  rows: T[];
  rowKey: (row: T) => string;
  isLoading?: boolean;
  error?: string | null;
  emptyMessage: string;
  // sorting (controlled)
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
  onSort?: (key: string) => void;
  // pagination (controlled)
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  onRowClick?: (row: T) => void;
  // Optional per-row class hook (e.g. overdue highlight). Returns extra classes for the <tr>.
  rowClassName?: (row: T) => string;
  // i18n labels for pagination/empty/error
  labels: { previous: string; next: string; pageOf: string; loadError: string };
}

/**
 * Interpolate `{page}`/`{total}` placeholders in a label (e.g. "Page {page} of {total}").
 */
function formatPageOf(template: string, page: number, total: number): string {
  return template
    .replace('{page}', String(page))
    .replace('{total}', String(total));
}

function SkeletonRows({ columns }: { columns: number }) {
  return (
    <>
      {Array.from({ length: 5 }).map((_, i) => (
        <tr key={i} className="border-b border-gray-100 animate-pulse">
          {Array.from({ length: columns }).map((__, j) => (
            <td key={j} className="px-4 py-3">
              <div className="h-4 w-full max-w-[8rem] rounded bg-gray-200" />
            </td>
          ))}
        </tr>
      ))}
    </>
  );
}

/**
 * Generic, presentational, sortable, paginated table. No data fetching inside —
 * sorting and pagination are fully controlled by the parent. Shared by all
 * Phase 2 list screens (and the customers list) to avoid reinventing tables.
 */
export function EntityTable<T>({
  columns,
  rows,
  rowKey,
  isLoading = false,
  error = null,
  emptyMessage,
  sortBy,
  sortDir,
  onSort,
  page,
  totalPages,
  onPageChange,
  onRowClick,
  rowClassName,
  labels,
}: EntityTableProps<T>) {
  const showEmpty = !isLoading && !error && rows.length === 0;

  const sortIndicator = (key: string) =>
    sortBy === key ? (sortDir === 'asc' ? ' ▲' : ' ▼') : '';

  return (
    <div>
      {error ? (
        <div className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {labels.loadError}
        </div>
      ) : (
        <div className="overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
          <table className="w-full text-left text-sm">
            <thead>
              <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
                {columns.map((col) => (
                  <th
                    key={col.key}
                    className={`px-4 py-3 font-medium ${col.align === 'right' ? 'text-right' : ''}`}
                  >
                    {col.sortable && onSort ? (
                      <button
                        type="button"
                        onClick={() => onSort(col.key)}
                        className="font-medium hover:text-pine-navy"
                      >
                        {col.header}
                        {sortIndicator(col.key)}
                      </button>
                    ) : (
                      col.header
                    )}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {isLoading ? (
                <SkeletonRows columns={columns.length} />
              ) : showEmpty ? (
                <tr>
                  <td
                    colSpan={columns.length}
                    className="px-4 py-10 text-center text-gray-400"
                  >
                    {emptyMessage}
                  </td>
                </tr>
              ) : (
                rows.map((row) => (
                  <tr
                    key={rowKey(row)}
                    onClick={onRowClick ? () => onRowClick(row) : undefined}
                    className={`border-b border-gray-50 last:border-0 hover:bg-gray-50 ${
                      onRowClick ? 'cursor-pointer' : ''
                    } ${rowClassName ? rowClassName(row) : ''}`}
                  >
                    {columns.map((col) => (
                      <td
                        key={col.key}
                        className={`px-4 py-3 text-gray-700 ${
                          col.align === 'right' ? 'text-right tabular-nums' : ''
                        }`}
                      >
                        {col.render(row)}
                      </td>
                    ))}
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      )}

      <div className="mt-4 flex items-center justify-between text-sm">
        <span className="text-gray-500">
          {formatPageOf(labels.pageOf, page, totalPages)}
        </span>
        <div className="flex gap-2">
          <button
            type="button"
            onClick={() => onPageChange(Math.max(1, page - 1))}
            disabled={page <= 1 || isLoading}
            className="rounded-lg border border-gray-200 px-3 py-1.5 font-medium text-gray-600 disabled:cursor-not-allowed disabled:opacity-40 hover:bg-gray-50"
          >
            {labels.previous}
          </button>
          <button
            type="button"
            onClick={() => onPageChange(Math.min(totalPages, page + 1))}
            disabled={page >= totalPages || isLoading}
            className="rounded-lg border border-gray-200 px-3 py-1.5 font-medium text-gray-600 disabled:cursor-not-allowed disabled:opacity-40 hover:bg-gray-50"
          >
            {labels.next}
          </button>
        </div>
      </div>
    </div>
  );
}
