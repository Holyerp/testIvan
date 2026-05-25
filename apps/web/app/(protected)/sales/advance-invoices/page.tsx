'use client';

import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd, statusBadgeClass } from '@/lib/format';

interface SalesAdvanceInvoiceListItem {
  id: string;
  number: string;
  customerName: string;
  postingDate: string;
  amount: number;
  status: string; // OPEN | PARTIAL | PAID
}

const LIST_PATH = '/api/v1/sales/advance-invoices';

export default function SalesAdvanceInvoicesPage() {
  const t = useTranslations('salesAdvance');
  const tSales = useTranslations('sales');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const router = useRouter();

  const query = usePaginatedQuery<SalesAdvanceInvoiceListItem>({
    path: LIST_PATH,
    pageSize: 20,
    initialSortBy: 'date',
    initialSortDir: 'desc',
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: '' },
    {
      type: 'select',
      key: 'status',
      label: tSales('filterStatus'),
      value: '',
      options: [
        { value: '', label: tSales('filterAll') },
        { value: 'Open', label: tSales('status.OPEN') },
        { value: 'Partially Paid', label: tSales('status.PARTIAL') },
        { value: 'Paid', label: tSales('status.PAID') },
      ],
    },
    { type: 'dateRange', key: 'date', label: tSales('date'), from: '', to: '' },
  ];

  const handleFilterChange = (key: string, value: unknown) => {
    if (key === 'date' && typeof value === 'object' && value !== null) {
      const range = value as { from?: string; to?: string };
      query.setParam('fromDate', range.from ?? '');
      query.setParam('toDate', range.to ?? '');
      return;
    }
    query.setParam(key, typeof value === 'string' ? value : '');
  };

  const columns: Column<SalesAdvanceInvoiceListItem>[] = [
    { key: 'number', header: tSales('number'), render: (i) => <span className="font-medium text-pine-navy">{i.number}</span> },
    { key: 'customer', header: tSales('customer'), render: (i) => i.customerName },
    { key: 'date', header: tSales('date'), sortable: true, render: (i) => i.postingDate },
    { key: 'amount', header: tSales('amount'), align: 'right', sortable: true, render: (i) => formatRsd(i.amount) },
    {
      key: 'status',
      header: tSales('statusLabel'),
      render: (i) => (
        <span className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-medium ${statusBadgeClass(i.status)}`}>
          {tSales(`status.${i.status}`)}
        </span>
      ),
    },
  ];

  return (
    <div className="p-8 font-sans">
      <div className="flex flex-wrap items-center gap-3">
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        {/* Advance tag clearly distinguishes these from regular sales invoices. */}
        <span className="inline-block rounded-full bg-purple-100 px-2.5 py-0.5 text-xs font-medium text-purple-700">
          {t('advanceTag')}
        </span>
      </div>
      <p className="mt-1 text-gray-500">{t('subtitle')}</p>

      <div className="mt-6">
        <FilterPanel fields={filterFields} onChange={handleFilterChange} />
      </div>

      <div className="mt-6">
        <EntityTable<SalesAdvanceInvoiceListItem>
          columns={columns}
          rows={query.items}
          rowKey={(i) => i.id}
          isLoading={query.isLoading}
          error={query.error}
          emptyMessage={t('noResults')}
          sortBy={query.sortBy}
          sortDir={query.sortDir}
          onSort={query.setSort}
          page={query.page}
          totalPages={query.totalPages}
          onPageChange={query.setPage}
          onRowClick={(i) => router.push(`/sales/advance-invoices/${i.id}`)}
          labels={{
            previous: tSales('previous'),
            next: tSales('next'),
            pageOf: tSales('pageOf', { page: '{page}', total: '{total}' }),
            loadError: t('loadError'),
          }}
        />
      </div>
    </div>
  );
}
