'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd, isOverdue, statusBadgeClass } from '@/lib/format';

interface PurchaseInvoiceListItem {
  id: string;
  number: string;
  vendorName: string;
  postingDate: string;
  dueDate: string;
  amount: number;
  status: string; // OPEN | PARTIAL | PAID
}

type Tab = 'open' | 'posted' | 'creditMemos';

const TAB_PATHS: Record<Tab, string> = {
  open: '/api/v1/purchase/invoices',
  posted: '/api/v1/purchase/posted-invoices',
  creditMemos: '/api/v1/purchase/credit-memos',
};

const TABS: Tab[] = ['open', 'posted', 'creditMemos'];

const TAB_LABEL_KEY: Record<Tab, string> = {
  open: 'tabOpen',
  posted: 'tabPosted',
  creditMemos: 'tabCreditMemos',
};

export default function PurchaseInvoicesPage() {
  const t = useTranslations('purchase');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const [tab, setTab] = useState<Tab>('open');

  if (!user) return null;

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      <div className="mt-6 flex gap-1 border-b border-gray-200">
        {TABS.map((tabKey) => (
          <button
            key={tabKey}
            type="button"
            onClick={() => setTab(tabKey)}
            className={`px-4 py-2 text-sm font-medium transition-colors ${
              tab === tabKey
                ? 'border-b-2 border-pine-green text-pine-navy'
                : 'text-gray-500 hover:text-pine-navy'
            }`}
          >
            {t(TAB_LABEL_KEY[tabKey])}
          </button>
        ))}
      </div>

      {/* Remount on tab change so page/sort/filter state resets per collection. */}
      <PurchaseInvoiceTable key={tab} path={TAB_PATHS[tab]} />
    </div>
  );
}

function PurchaseInvoiceTable({ path }: { path: string }) {
  const t = useTranslations('purchase');
  const router = useRouter();
  const today = new Date().toISOString().slice(0, 10);

  const query = usePaginatedQuery<PurchaseInvoiceListItem>({
    path,
    pageSize: 20,
    initialSortBy: 'date',
    initialSortDir: 'desc',
  });

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: '' },
    {
      type: 'select',
      key: 'status',
      label: t('filterStatus'),
      value: '',
      options: [
        { value: '', label: t('filterAll') },
        { value: 'Open', label: t('status.OPEN') },
        { value: 'Partially Paid', label: t('status.PARTIAL') },
        { value: 'Paid', label: t('status.PAID') },
      ],
    },
    { type: 'dateRange', key: 'date', label: t('date'), from: '', to: '' },
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

  const columns: Column<PurchaseInvoiceListItem>[] = [
    { key: 'number', header: t('number'), render: (i) => <span className="font-medium text-pine-navy">{i.number}</span> },
    { key: 'vendor', header: t('vendor'), render: (i) => i.vendorName },
    { key: 'date', header: t('date'), sortable: true, render: (i) => i.postingDate },
    { key: 'dueDate', header: t('dueDate'), sortable: true, render: (i) => i.dueDate },
    { key: 'amount', header: t('amount'), align: 'right', sortable: true, render: (i) => formatRsd(i.amount) },
    {
      key: 'status',
      header: t('statusLabel'),
      render: (i) => (
        <span className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-medium ${statusBadgeClass(i.status)}`}>
          {t(`status.${i.status}`)}
        </span>
      ),
    },
  ];

  return (
    <>
      <div className="mt-6">
        <FilterPanel fields={filterFields} onChange={handleFilterChange} />
      </div>

      <div className="mt-6">
        <EntityTable<PurchaseInvoiceListItem>
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
          onRowClick={(i) => router.push(`/purchase/invoices/${i.id}`)}
          rowClassName={(i) => (isOverdue(i.status, i.dueDate, today) ? 'bg-amber-50' : '')}
          labels={{
            previous: t('previous'),
            next: t('next'),
            pageOf: t('pageOf', { page: '{page}', total: '{total}' }),
            loadError: t('loadError'),
          }}
        />
      </div>
    </>
  );
}
