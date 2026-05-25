'use client';

import { useCallback, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd, sortByQuantityAsc, belowMinimumCardClass } from '@/lib/format';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

// Category / location options mirror the known mock values (same set as the item list).
const CATEGORY_OPTIONS = ['GRAĐEVINA', 'ALATI', 'ELEKTRO', 'FARBE'];
const LOCATION_OPTIONS = ['MAGACIN-1', 'MAGACIN-2'];

interface InventorySummary {
  totalItems: number;
  totalStockValue: number;
  itemsBelowMinimum: number;
}

interface InventoryLocation {
  location: string;
  itemCount: number;
  totalQuantity: number;
  totalValue: number;
}

interface LowStockItem {
  id: string;
  number: string;
  description: string;
  quantityOnHand: number;
  minimumStock: number;
  location: string;
}

function KpiCard({
  label,
  value,
  valueClassName = 'text-pine-navy',
}: {
  label: string;
  value: string;
  valueClassName?: string;
}) {
  return (
    <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100">
      <p className="text-sm font-medium text-gray-500">{label}</p>
      <p className={`mt-2 text-3xl font-semibold font-sans ${valueClassName}`}>{value}</p>
    </div>
  );
}

function KpiSkeleton() {
  return (
    <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100 animate-pulse">
      <div className="h-4 w-24 bg-gray-200 rounded" />
      <div className="mt-3 h-8 w-32 bg-gray-200 rounded" />
    </div>
  );
}

export default function InventoryPage() {
  const t = useTranslations('inventory');
  const router = useRouter();
  // WAREHOUSE module guard: ACCOUNTING is redirected to /403; ADMIN/MANAGER/WAREHOUSE pass.
  const { user } = useRequireModule('warehouse');
  const accessToken = useAuthStore((s) => s.accessToken);

  const [summary, setSummary] = useState<InventorySummary | null>(null);
  const [locations, setLocations] = useState<InventoryLocation[]>([]);
  const [lowStock, setLowStock] = useState<LowStockItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  const [location, setLocation] = useState('');
  const [category, setCategory] = useState('');

  const fetchJson = useCallback(
    async <T,>(path: string): Promise<T> => {
      const res = await fetch(`${API_BASE_URL}${path}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      const json = await res.json();
      if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
      return json.data as T;
    },
    [accessToken]
  );

  // Summary refetches whenever the location/category filter changes.
  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    const params = new URLSearchParams();
    if (location) params.set('location', location);
    if (category) params.set('category', category);
    const qs = params.toString();

    fetchJson<InventorySummary>(`/api/v1/inventory/summary${qs ? `?${qs}` : ''}`)
      .then((data) => {
        if (active) setSummary(data);
      })
      .catch(() => {
        if (active) setHasError(true);
      });

    return () => {
      active = false;
    };
  }, [accessToken, location, category, fetchJson]);

  // Stock-by-location and low-stock load once (unfiltered).
  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    setHasError(false);

    Promise.all([
      fetchJson<InventoryLocation[]>('/api/v1/inventory/locations'),
      fetchJson<LowStockItem[]>('/api/v1/inventory/low-stock'),
    ])
      .then(([locationData, lowStockData]) => {
        if (active) {
          setLocations(locationData);
          setLowStock(sortByQuantityAsc(lowStockData));
          setIsLoading(false);
        }
      })
      .catch(() => {
        if (active) {
          setHasError(true);
          setIsLoading(false);
        }
      });

    return () => {
      active = false;
    };
  }, [accessToken, fetchJson]);

  if (!user) return null;

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      {hasError && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {t('loadError')}
        </div>
      )}

      {/* Filters (applied to the summary) */}
      <div className="mt-6 flex flex-wrap gap-4">
        <label className="flex flex-col text-sm">
          <span className="mb-1 font-medium text-gray-600">{t('filterLocation')}</span>
          <select
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            className="rounded-lg border border-gray-200 px-3 py-2 text-sm"
          >
            <option value="">{t('filterAll')}</option>
            {LOCATION_OPTIONS.map((l) => (
              <option key={l} value={l}>
                {l}
              </option>
            ))}
          </select>
        </label>
        <label className="flex flex-col text-sm">
          <span className="mb-1 font-medium text-gray-600">{t('filterCategory')}</span>
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            className="rounded-lg border border-gray-200 px-3 py-2 text-sm"
          >
            <option value="">{t('filterAll')}</option>
            {CATEGORY_OPTIONS.map((c) => (
              <option key={c} value={c}>
                {c}
              </option>
            ))}
          </select>
        </label>
      </div>

      {/* Summary cards */}
      <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-3">
        {isLoading || !summary ? (
          <>
            <KpiSkeleton />
            <KpiSkeleton />
            <KpiSkeleton />
          </>
        ) : (
          <>
            <KpiCard label={t('totalItems')} value={String(summary.totalItems)} />
            <KpiCard label={t('totalStockValue')} value={formatRsd(summary.totalStockValue)} />
            <KpiCard
              label={t('itemsBelowMinimum')}
              value={String(summary.itemsBelowMinimum)}
              valueClassName={belowMinimumCardClass(summary.itemsBelowMinimum)}
            />
          </>
        )}
      </div>

      {/* Stock by location */}
      <div className="mt-8 rounded-xl bg-white p-6 shadow-sm border border-gray-100">
        <h2 className="text-sm font-medium text-gray-500">{t('stockByLocation')}</h2>
        <div className="mt-4 overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b border-gray-100 text-left text-gray-500">
                <th className="py-2 font-medium">{t('location')}</th>
                <th className="py-2 font-medium text-right">{t('itemCount')}</th>
                <th className="py-2 font-medium text-right">{t('totalQuantity')}</th>
                <th className="py-2 font-medium text-right">{t('totalValue')}</th>
              </tr>
            </thead>
            <tbody>
              {locations.map((l) => (
                <tr key={l.location} className="border-b border-gray-50">
                  <td className="py-2 font-medium text-pine-navy">{l.location}</td>
                  <td className="py-2 text-right">{l.itemCount}</td>
                  <td className="py-2 text-right">{l.totalQuantity}</td>
                  <td className="py-2 text-right">{formatRsd(l.totalValue)}</td>
                </tr>
              ))}
              {!isLoading && locations.length === 0 && (
                <tr>
                  <td colSpan={4} className="py-4 text-center text-gray-400">
                    {t('noResults')}
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Low-stock items */}
      <div className="mt-8 rounded-xl bg-white p-6 shadow-sm border border-gray-100">
        <h2 className="text-sm font-medium text-gray-500">{t('lowStockItems')}</h2>
        <div className="mt-4 overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b border-gray-100 text-left text-gray-500">
                <th className="py-2 font-medium">{t('number')}</th>
                <th className="py-2 font-medium">{t('description')}</th>
                <th className="py-2 font-medium text-right">{t('quantityOnHand')}</th>
                <th className="py-2 font-medium text-right">{t('minimum')}</th>
                <th className="py-2 font-medium">{t('location')}</th>
              </tr>
            </thead>
            <tbody>
              {lowStock.map((i) => (
                <tr
                  key={i.id}
                  className="cursor-pointer border-b border-gray-50 bg-amber-50 hover:bg-amber-100"
                  onClick={() => router.push(`/items/${i.id}`)}
                >
                  <td className="py-2 font-medium text-pine-navy">{i.number}</td>
                  <td className="py-2">{i.description}</td>
                  <td className="py-2 text-right font-medium text-amber-700">{i.quantityOnHand}</td>
                  <td className="py-2 text-right">{i.minimumStock}</td>
                  <td className="py-2">{i.location}</td>
                </tr>
              ))}
              {!isLoading && lowStock.length === 0 && (
                <tr>
                  <td colSpan={5} className="py-4 text-center text-gray-400">
                    {t('noResults')}
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
