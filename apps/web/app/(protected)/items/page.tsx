'use client';

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { usePaginatedQuery } from '@/lib/hooks/use-paginated-query';
import { EntityTable, type Column } from '@/components/entity-table';
import { FilterPanel, type FilterField } from '@/components/filter-panel';
import { formatRsd, lowStockRowClass } from '@/lib/format';

interface ItemListItem {
  id: string;
  number: string;
  description: string;
  category: string;
  unitOfMeasure: string;
  quantityOnHand: number;
  minimumStock: number;
  unitCost: number;
  isLowStock: boolean;
}

// Category / location options mirror the known mock values. Empty value = "All".
const CATEGORY_OPTIONS = ['GRAĐEVINA', 'ALATI', 'ELEKTRO', 'FARBE'];
const LOCATION_OPTIONS = ['MAGACIN-1', 'MAGACIN-2'];

export default function ItemsPage() {
  const t = useTranslations('items');
  const router = useRouter();
  // WAREHOUSE module guard: ACCOUNTING is redirected to /403; ADMIN/MANAGER/WAREHOUSE pass.
  const { user } = useRequireModule('warehouse');

  const query = usePaginatedQuery<ItemListItem>({
    path: '/api/v1/items',
    pageSize: 20,
    initialSortBy: 'name',
    initialSortDir: 'asc',
  });

  if (!user) return null;

  const filterFields: FilterField[] = [
    { type: 'search', key: 'search', placeholder: t('search'), value: '' },
    {
      type: 'select',
      key: 'category',
      label: t('filterCategory'),
      value: '',
      options: [
        { value: '', label: t('filterAll') },
        ...CATEGORY_OPTIONS.map((c) => ({ value: c, label: c })),
      ],
    },
    {
      type: 'select',
      key: 'location',
      label: t('filterLocation'),
      value: '',
      options: [
        { value: '', label: t('filterAll') },
        ...LOCATION_OPTIONS.map((l) => ({ value: l, label: l })),
      ],
    },
  ];

  const columns: Column<ItemListItem>[] = [
    {
      key: 'number',
      header: t('number'),
      render: (i) => (
        <Link href={`/items/${i.id}`} className="font-medium text-pine-navy hover:text-pine-green hover:underline">
          {i.number}
        </Link>
      ),
    },
    {
      key: 'name',
      header: t('description'),
      sortable: true,
      render: (i) => (
        <Link href={`/items/${i.id}`} className="hover:text-pine-green hover:underline">
          {i.description}
        </Link>
      ),
    },
    { key: 'unitOfMeasure', header: t('unit'), render: (i) => i.unitOfMeasure },
    {
      key: 'quantity',
      header: t('quantityOnHand'),
      sortable: true,
      align: 'right',
      render: (i) => (
        <span className="inline-flex items-center justify-end gap-2">
          <span>{i.quantityOnHand}</span>
          {i.isLowStock && (
            <span className="rounded-full bg-amber-100 px-2 py-0.5 text-xs font-medium text-amber-700">
              {t('lowStock')}
            </span>
          )}
        </span>
      ),
    },
    { key: 'unitCost', header: t('unitCost'), sortable: true, align: 'right', render: (i) => formatRsd(i.unitCost) },
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
        <EntityTable<ItemListItem>
          columns={columns}
          rows={query.items}
          rowKey={(i) => i.id}
          isLoading={query.isLoading}
          error={query.error}
          emptyMessage={t('noResults')}
          rowClassName={(i) => lowStockRowClass(i.isLowStock)}
          sortBy={query.sortBy}
          sortDir={query.sortDir}
          onSort={query.setSort}
          page={query.page}
          totalPages={query.totalPages}
          onPageChange={query.setPage}
          onRowClick={(i) => router.push(`/items/${i.id}`)}
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
