'use client';

import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField, type FilterChangeValue } from '@/components/filter-panel';
import { exportToExcel, type ExportColumn } from '@/lib/export';
import {
  type AuditLogEntry,
  AUDIT_CATEGORIES,
  categoryBadgeClass,
  categoryLabelKey,
  formatAuditTimestamp,
} from '@/lib/admin/audit-log';

export default function AuditLogPage() {
  const t = useTranslations('auditLog');
  // ADMIN module guard: every other role is redirected to /403.
  const { user } = useRequireModule('admin');

  const query = usePaginatedQuery<AuditLogEntry>({
    path: '/api/v1/admin/audit-log',
    pageSize: 20,
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'username', placeholder: t('filterUser'), value: '' },
    {
      type: 'select',
      key: 'category',
      label: t('filterCategory'),
      value: '',
      options: [
        { value: '', label: t('all') },
        ...AUDIT_CATEGORIES.map((c) => ({ value: c, label: t(`category.${c}`) })),
      ],
    },
    { type: 'dateRange', key: 'dateRange', label: t('dateRange'), from: '', to: '' },
  ];

  const handleFilterChange = (key: string, value: FilterChangeValue) => {
    if (key === 'dateRange' && typeof value === 'object' && 'from' in value) {
      query.setParam('fromDate', value.from ?? '');
      query.setParam('toDate', value.to ?? '');
      return;
    }
    query.setParam(key, typeof value === 'string' ? value : '');
  };

  const columns: Column<AuditLogEntry>[] = [
    {
      key: 'timestamp',
      header: t('timestamp'),
      render: (e) => formatAuditTimestamp(e.timestamp, '—'),
    },
    { key: 'user', header: t('user'), render: (e) => e.username ?? '—' },
    {
      key: 'category',
      header: t('categoryColumn'),
      render: (e) => (
        <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${categoryBadgeClass(e.category)}`}>
          {t(`category.${categoryLabelKey(e.category)}`)}
        </span>
      ),
    },
    {
      key: 'action',
      header: t('action'),
      render: (e) => <span className="font-mono text-xs text-gray-600">{e.action}</span>,
    },
    { key: 'entityType', header: t('entityType'), render: (e) => e.entityType ?? '—' },
    { key: 'entityId', header: t('entityId'), render: (e) => e.entityId ?? '—' },
  ];

  const exportColumns: ExportColumn<AuditLogEntry>[] = [
    { header: t('timestamp'), value: (e) => formatAuditTimestamp(e.timestamp, '') },
    { header: t('user'), value: (e) => e.username ?? '' },
    { header: t('action'), value: (e) => e.action },
    { header: t('categoryColumn'), value: (e) => t(`category.${categoryLabelKey(e.category)}`) },
    { header: t('entityType'), value: (e) => e.entityType ?? '' },
    { header: t('entityId'), value: (e) => e.entityId ?? '' },
  ];

  const handleExport = () => {
    exportToExcel(query.items, exportColumns, 'audit-log');
  };

  return (
    <div className="p-8 font-sans">
      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
          <p className="mt-1 text-gray-500">{t('subtitle')}</p>
        </div>
        <button
          type="button"
          onClick={handleExport}
          disabled={query.items.length === 0}
          className="rounded-lg bg-pine-navy px-4 py-2 text-sm font-medium text-white hover:bg-pine-navy-mid disabled:opacity-50"
        >
          {t('exportExcel')}
        </button>
      </div>

      <div className="mt-6">
        <FilterPanel fields={filterFields} onChange={handleFilterChange} />
      </div>

      <div className="mt-6">
        <EntityTable<AuditLogEntry>
          columns={columns}
          rows={query.items}
          rowKey={(e) => e.id}
          isLoading={query.isLoading}
          error={query.error}
          emptyMessage={t('noResults')}
          page={query.page}
          totalPages={query.totalPages}
          onPageChange={query.setPage}
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
