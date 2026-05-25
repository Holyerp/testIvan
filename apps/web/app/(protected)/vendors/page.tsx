'use client';

import Link from 'next/link';
import { useRouter, useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd } from '@/lib/format';

interface VendorListItem {
  id: string;
  number: string;
  displayName: string;
  city: string;
  balance: number;
  phone: string;
}

export default function VendorsPage() {
  const t = useTranslations('vendors');
  const router = useRouter();
  const searchParams = useSearchParams();
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);

  // Seed the search filter from the URL so the universal-search "View all" link
  // (e.g. /vendors?search=Supplier) lands on a prefiltered list.
  const initialSearch = searchParams.get('search') ?? '';

  const query = usePaginatedQuery<VendorListItem>({
    path: '/api/v1/vendors',
    pageSize: 20,
    initialSortBy: 'displayName',
    initialSortDir: 'asc',
    extraParams: initialSearch ? { search: initialSearch } : undefined,
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: initialSearch },
  ];

  const columns: Column<VendorListItem>[] = [
    {
      key: 'number',
      header: t('number'),
      render: (v) => (
        <Link href={`/vendors/${v.id}`} className="font-medium text-pine-navy hover:text-pine-green hover:underline">
          {v.number}
        </Link>
      ),
    },
    {
      key: 'displayName',
      header: t('name'),
      sortable: true,
      render: (v) => (
        <Link href={`/vendors/${v.id}`} className="hover:text-pine-green hover:underline">
          {v.displayName}
        </Link>
      ),
    },
    { key: 'city', header: t('city'), render: (v) => v.city },
    { key: 'balance', header: t('balance'), sortable: true, align: 'right', render: (v) => formatRsd(v.balance) },
    { key: 'phone', header: t('phone'), render: (v) => v.phone },
  ];

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      <div className="mt-6">
        <FilterPanel
          fields={filterFields}
          onChange={(key, value) => query.setParam(key, typeof value === 'string' ? value : '')}
        />
      </div>

      <div className="mt-6">
        <EntityTable<VendorListItem>
          columns={columns}
          rows={query.items}
          rowKey={(v) => v.id}
          isLoading={query.isLoading}
          error={query.error}
          emptyMessage={t('noResults')}
          sortBy={query.sortBy}
          sortDir={query.sortDir}
          onSort={query.setSort}
          page={query.page}
          totalPages={query.totalPages}
          onPageChange={query.setPage}
          onRowClick={(v) => router.push(`/vendors/${v.id}`)}
          labels={{
            previous: t('previous'),
            next: t('next'),
            pageOf: t('pageOf', { page: '{page}', total: '{total}' }),
            loadError: t('loadError'),
          }}
        />
      </div>
    </div>
  );
}
