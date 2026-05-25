'use client';

import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd, creditDocumentTypeBadgeClass, creditMemoStatusBadgeClass } from '@/lib/format';

interface CreditDocumentListItem {
  id: string;
  number: string;
  type: string; // CREDIT_MEMO | DEBIT_MEMO | STORNO
  partyName: string;
  postingDate: string;
  amount: number;
  originalInvoiceNumber: string;
  status: string; // OPEN | POSTED
}

export default function CreditDocumentsPage() {
  const t = useTranslations('creditDocuments');
  const router = useRouter();
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);

  const query = usePaginatedQuery<CreditDocumentListItem>({
    path: '/api/v1/credit-documents',
    pageSize: 20,
    initialSortBy: 'date',
    initialSortDir: 'desc',
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: '' },
    {
      type: 'select',
      key: 'type',
      label: t('filterType'),
      value: '',
      options: [
        { value: '', label: t('filterAll') },
        { value: 'CREDIT_MEMO', label: t('type.CREDIT_MEMO') },
        { value: 'DEBIT_MEMO', label: t('type.DEBIT_MEMO') },
        { value: 'STORNO', label: t('type.STORNO') },
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

  const columns: Column<CreditDocumentListItem>[] = [
    {
      key: 'number',
      header: t('number'),
      sortable: true,
      render: (i) => <span className="font-medium text-pine-navy">{i.number}</span>,
    },
    {
      key: 'type',
      header: t('typeLabel'),
      render: (i) => (
        <span
          className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-medium ${creditDocumentTypeBadgeClass(i.type)}`}
        >
          {t(`type.${i.type}`)}
        </span>
      ),
    },
    { key: 'party', header: t('party'), render: (i) => i.partyName },
    { key: 'date', header: t('date'), sortable: true, render: (i) => i.postingDate },
    {
      key: 'originalInvoice',
      header: t('originalInvoice'),
      render: (i) => <span className="text-gray-600">{i.originalInvoiceNumber || '—'}</span>,
    },
    { key: 'amount', header: t('amount'), align: 'right', sortable: true, render: (i) => formatRsd(i.amount) },
    {
      key: 'status',
      header: t('statusLabel'),
      render: (i) => (
        <span
          className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-medium ${creditMemoStatusBadgeClass(i.status)}`}
        >
          {t(`status.${i.status}`)}
        </span>
      ),
    },
  ];

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      <div className="mt-6">
        <FilterPanel fields={filterFields} onChange={handleFilterChange} />
      </div>

      <div className="mt-6">
        <EntityTable<CreditDocumentListItem>
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
          onRowClick={(i) => router.push(`/credit-documents/${i.id}`)}
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
