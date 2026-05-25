'use client';

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd } from '@/lib/format';

interface CustomerListItem {
  id: string;
  number: string;
  displayName: string;
  city: string;
  balance: number;
  balanceDue: number;
}

export default function CustomersPage() {
  const t = useTranslations('customers');
  const router = useRouter();
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);

  const query = usePaginatedQuery<CustomerListItem>({
    path: '/api/v1/customers',
    pageSize: 20,
    initialSortBy: 'displayName',
    initialSortDir: 'asc',
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: '' },
  ];

  const columns: Column<CustomerListItem>[] = [
    {
      key: 'number',
      header: t('number'),
      sortable: true,
      render: (c) => (
        <Link href={`/customers/${c.id}`} className="font-medium text-pine-navy hover:text-pine-green hover:underline">
          {c.number}
        </Link>
      ),
    },
    {
      key: 'displayName',
      header: t('name'),
      sortable: true,
      render: (c) => (
        <Link href={`/customers/${c.id}`} className="hover:text-pine-green hover:underline">
          {c.displayName}
        </Link>
      ),
    },
    { key: 'city', header: t('city'), render: (c) => c.city },
    { key: 'balance', header: t('balance'), align: 'right', render: (c) => formatRsd(c.balance) },
    { key: 'balanceDue', header: t('balanceDue'), align: 'right', render: (c) => formatRsd(c.balanceDue) },
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
        <EntityTable<CustomerListItem>
          columns={columns}
          rows={query.items}
          rowKey={(c) => c.id}
          isLoading={query.isLoading}
          error={query.error}
          emptyMessage={t('noResults')}
          sortBy={query.sortBy}
          sortDir={query.sortDir}
          onSort={query.setSort}
          page={query.page}
          totalPages={query.totalPages}
          onPageChange={query.setPage}
          onRowClick={(c) => router.push(`/customers/${c.id}`)}
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
