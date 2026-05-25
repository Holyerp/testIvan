'use client';

import { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { useAuthStore } from '@/lib/stores/auth-store';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

interface InvoiceTrendPoint {
  month: string;
  count: number;
  totalAmount: number;
}

interface DashboardKpis {
  totalCustomers: number;
  openInvoicesAmount: number;
  overdueInvoicesAmount: number;
  totalVendors: number;
  invoiceTrend: InvoiceTrendPoint[];
  isMockMode: boolean;
}

const rsdFormatter = new Intl.NumberFormat('sr-RS', {
  style: 'currency',
  currency: 'RSD',
  maximumFractionDigits: 0,
});

function KpiCard({
  label,
  value,
  subtitle,
}: {
  label: string;
  value: string;
  subtitle?: string;
}) {
  return (
    <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100">
      <p className="text-sm font-medium text-gray-500">{label}</p>
      <p className="mt-2 text-3xl font-semibold text-pine-navy font-sans">{value}</p>
      {subtitle && <p className="mt-1 text-xs text-gray-400">{subtitle}</p>}
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

export default function DashboardPage() {
  const t = useTranslations('dashboard');
  const { user } = useRequireAuth();
  const accessToken = useAuthStore((s) => s.accessToken);

  const [kpis, setKpis] = useState<DashboardKpis | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    setHasError(false);

    fetch(`${API_BASE_URL}/api/v1/dashboard/kpis`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    })
      .then(async (res) => {
        const json = await res.json();
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as DashboardKpis;
      })
      .then((data) => {
        if (active) {
          setKpis(data);
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
  }, [accessToken]);

  if (!user) return null;

  const maxTrendCount = kpis
    ? Math.max(1, ...kpis.invoiceTrend.map((p) => p.count))
    : 1;

  return (
    <div className="p-8 font-sans">
      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
          <p className="mt-1 text-gray-500">{t('subtitle')}</p>
        </div>
        {kpis && (
          <span
            className={`inline-flex items-center gap-2 rounded-full px-3 py-1 text-xs font-medium ${
              kpis.isMockMode
                ? 'bg-amber-100 text-amber-700'
                : 'bg-pine-green-pale text-pine-green-dark'
            }`}
          >
            <span
              className={`h-2 w-2 rounded-full ${
                kpis.isMockMode ? 'bg-amber-500' : 'bg-pine-green'
              }`}
            />
            {kpis.isMockMode ? t('mockMode') : t('connected')}
          </span>
        )}
      </div>

      {hasError && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {t('loadError')}
        </div>
      )}

      <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
        {isLoading ? (
          <>
            <KpiSkeleton />
            <KpiSkeleton />
            <KpiSkeleton />
            <KpiSkeleton />
          </>
        ) : kpis ? (
          <>
            <KpiCard label={t('totalCustomers')} value={String(kpis.totalCustomers)} />
            <KpiCard
              label={t('openInvoices')}
              value={rsdFormatter.format(kpis.openInvoicesAmount)}
              subtitle={t('openInvoicesSubtitle')}
            />
            <KpiCard
              label={t('overdueInvoices')}
              value={rsdFormatter.format(kpis.overdueInvoicesAmount)}
              subtitle={t('overdueSubtitle')}
            />
            <KpiCard label={t('totalVendors')} value={String(kpis.totalVendors)} />
          </>
        ) : null}
      </div>

      {kpis && !hasError && (
        <div className="mt-8 rounded-xl bg-white p-6 shadow-sm border border-gray-100">
          <h2 className="text-sm font-medium text-gray-500">{t('invoiceTrend')}</h2>
          <div className="mt-6 flex items-end gap-4 h-48">
            {kpis.invoiceTrend.map((point) => (
              <div key={point.month} className="flex flex-1 flex-col items-center gap-2">
                <div className="flex w-full flex-1 items-end">
                  <div
                    className="w-full rounded-t bg-pine-navy-mid transition-all"
                    style={{ height: `${(point.count / maxTrendCount) * 100}%` }}
                    role="img"
                    aria-label={`${point.month}: ${point.count}`}
                  />
                </div>
                <span className="text-xs font-medium text-gray-600">{point.count}</span>
                <span className="text-[10px] text-gray-400">{point.month}</span>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
