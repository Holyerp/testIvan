'use client';

import { useCallback, useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd } from '@/lib/format';
import {
  GRANULARITIES,
  type Granularity,
  type RevenueExpensePoint,
  deltaArrow,
  deltaClass,
  seriesMax,
  barHeightPercent,
} from '@/lib/analytics';
import { exportToExcel, exportToPdf, type ExportColumn } from '@/lib/export';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

interface PeriodComparison {
  currentRevenue: number;
  priorRevenue: number;
  currentExpense: number;
  priorExpense: number;
  revenueDeltaPercent: number;
  expenseDeltaPercent: number;
}

interface RevenueExpenseSeries {
  points: RevenueExpensePoint[];
  comparison: PeriodComparison;
  granularity: Granularity;
}

interface TopCustomer {
  id: string;
  name: string;
  revenue: number;
  invoiceCount: number;
}

interface TopItem {
  id: string;
  number: string;
  description: string;
  salesVolume: number;
  salesValue: number;
}

async function fetchJson<T>(path: string, token: string): Promise<T> {
  const res = await fetch(`${API_BASE_URL}${path}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  const json = await res.json();
  if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
  return json.data as T;
}

export default function AnalyticsPage() {
  const t = useTranslations('analytics');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const accessToken = useAuthStore((s) => s.accessToken);

  const [granularity, setGranularity] = useState<Granularity>('MONTHLY');
  const [fromDate, setFromDate] = useState('');
  const [toDate, setToDate] = useState('');

  const [series, setSeries] = useState<RevenueExpenseSeries | null>(null);
  const [customers, setCustomers] = useState<TopCustomer[]>([]);
  const [items, setItems] = useState<TopItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  const dateParams = useCallback(() => {
    const parts: string[] = [];
    if (fromDate) parts.push(`fromDate=${encodeURIComponent(fromDate)}`);
    if (toDate) parts.push(`toDate=${encodeURIComponent(toDate)}`);
    return parts.length > 0 ? `&${parts.join('&')}` : '';
  }, [fromDate, toDate]);

  useEffect(() => {
    if (!accessToken) return;

    let active = true;
    setIsLoading(true);
    setHasError(false);

    const range = dateParams();
    Promise.all([
      fetchJson<RevenueExpenseSeries>(
        `/api/v1/analytics/revenue-expense?granularity=${granularity}${range}`,
        accessToken
      ),
      fetchJson<TopCustomer[]>(`/api/v1/analytics/top-customers?top=10${range}`, accessToken),
      fetchJson<TopItem[]>(`/api/v1/analytics/top-items?top=10${range}`, accessToken),
    ])
      .then(([s, c, i]) => {
        if (!active) return;
        setSeries(s);
        setCustomers(c);
        setItems(i);
        setIsLoading(false);
      })
      .catch(() => {
        if (!active) return;
        setHasError(true);
        setIsLoading(false);
      });

    return () => {
      active = false;
    };
  }, [accessToken, granularity, dateParams]);

  if (!user) return null;

  const max = series ? seriesMax(series.points) : 1;

  const seriesColumns: ExportColumn<RevenueExpensePoint>[] = [
    { header: t('period'), value: (p) => p.period },
    { header: t('revenue'), value: (p) => p.revenue },
    { header: t('expense'), value: (p) => p.expense },
    { header: t('profit'), value: (p) => p.profit },
  ];

  const handleExport = (kind: 'excel' | 'pdf') => {
    if (!series) return;
    if (kind === 'excel') {
      exportToExcel(series.points, seriesColumns, 'analytics-revenue-expense');
    } else {
      exportToPdf(series.points, seriesColumns, 'analytics-revenue-expense', t('title'));
    }
  };

  return (
    <div className="p-8 font-sans">
      <div className="flex items-start justify-between gap-4">
        <div>
          <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
          <p className="mt-1 text-gray-500">{t('subtitle')}</p>
        </div>
        <div className="flex gap-2">
          <button
            type="button"
            onClick={() => handleExport('excel')}
            disabled={!series}
            className="rounded-lg bg-pine-green px-3 py-2 text-sm font-medium text-white hover:opacity-90 disabled:opacity-40"
          >
            {t('exportExcel')}
          </button>
          <button
            type="button"
            onClick={() => handleExport('pdf')}
            disabled={!series}
            className="rounded-lg border border-pine-navy px-3 py-2 text-sm font-medium text-pine-navy hover:bg-pine-navy/5 disabled:opacity-40"
          >
            {t('exportPdf')}
          </button>
        </div>
      </div>

      {/* Controls: granularity toggle + date range picker */}
      <div className="mt-6 flex flex-wrap items-end gap-4">
        <div className="inline-flex rounded-lg border border-gray-200 bg-white p-1">
          {GRANULARITIES.map((g) => (
            <button
              key={g}
              type="button"
              onClick={() => setGranularity(g)}
              className={`rounded-md px-3 py-1.5 text-sm font-medium transition-colors ${
                granularity === g
                  ? 'bg-pine-navy text-white'
                  : 'text-gray-600 hover:bg-gray-100'
              }`}
            >
              {t(`granularity.${g}`)}
            </button>
          ))}
        </div>
        <label className="flex flex-col text-xs font-medium text-gray-500">
          {t('dateFrom')}
          <input
            type="date"
            value={fromDate}
            onChange={(e) => setFromDate(e.target.value)}
            className="mt-1 rounded-lg border border-gray-200 px-3 py-1.5 text-sm text-pine-navy"
          />
        </label>
        <label className="flex flex-col text-xs font-medium text-gray-500">
          {t('dateTo')}
          <input
            type="date"
            value={toDate}
            onChange={(e) => setToDate(e.target.value)}
            className="mt-1 rounded-lg border border-gray-200 px-3 py-1.5 text-sm text-pine-navy"
          />
        </label>
      </div>

      {hasError && (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {t('loadError')}
        </div>
      )}

      {isLoading ? (
        <div className="mt-8 grid grid-cols-1 gap-4 lg:grid-cols-2">
          <div className="h-64 animate-pulse rounded-xl bg-white shadow-sm border border-gray-100" />
          <div className="h-64 animate-pulse rounded-xl bg-white shadow-sm border border-gray-100" />
        </div>
      ) : (
        series &&
        !hasError && (
          <>
            {/* Period comparison cards */}
            <div className="mt-8 grid grid-cols-1 gap-4 sm:grid-cols-2">
              <ComparisonCard
                label={t('revenue')}
                current={series.comparison.currentRevenue}
                prior={series.comparison.priorRevenue}
                deltaPct={series.comparison.revenueDeltaPercent}
                vsPriorLabel={t('vsPrior')}
              />
              <ComparisonCard
                label={t('expense')}
                current={series.comparison.currentExpense}
                prior={series.comparison.priorExpense}
                deltaPct={series.comparison.expenseDeltaPercent}
                vsPriorLabel={t('vsPrior')}
              />
            </div>

            {/* Revenue vs Expense paired bar chart */}
            <div className="mt-8 rounded-xl bg-white p-6 shadow-sm border border-gray-100">
              <div className="flex items-center justify-between">
                <h2 className="text-sm font-medium text-gray-500">
                  {t('revenue')} / {t('expense')}
                </h2>
                <div className="flex gap-4 text-xs text-gray-500">
                  <span className="flex items-center gap-1.5">
                    <span className="h-2.5 w-2.5 rounded-sm bg-pine-green" /> {t('revenue')}
                  </span>
                  <span className="flex items-center gap-1.5">
                    <span className="h-2.5 w-2.5 rounded-sm bg-pine-navy-mid" /> {t('expense')}
                  </span>
                </div>
              </div>
              <div className="mt-6 flex items-end gap-4 h-56">
                {series.points.map((point) => (
                  <div key={point.period} className="flex flex-1 flex-col items-center gap-2">
                    <div className="flex w-full flex-1 items-end justify-center gap-1">
                      <div
                        className="w-1/2 rounded-t bg-pine-green transition-all"
                        style={{ height: `${barHeightPercent(point.revenue, max)}%` }}
                        role="img"
                        aria-label={`${point.period} ${t('revenue')}: ${point.revenue}`}
                      />
                      <div
                        className="w-1/2 rounded-t bg-pine-navy-mid transition-all"
                        style={{ height: `${barHeightPercent(point.expense, max)}%` }}
                        role="img"
                        aria-label={`${point.period} ${t('expense')}: ${point.expense}`}
                      />
                    </div>
                    <span className="text-[10px] text-gray-400">{point.period}</span>
                  </div>
                ))}
              </div>
            </div>

            {/* Top customers + top items tables */}
            <div className="mt-8 grid grid-cols-1 gap-4 lg:grid-cols-2">
              <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100">
                <h2 className="text-sm font-medium text-gray-500">{t('topCustomers')}</h2>
                <table className="mt-4 w-full text-sm">
                  <thead>
                    <tr className="text-left text-xs uppercase tracking-wide text-gray-400">
                      <th className="pb-2">{t('customer')}</th>
                      <th className="pb-2 text-right">{t('revenue')}</th>
                      <th className="pb-2 text-right">{t('invoiceCount')}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {customers.map((c) => (
                      <tr key={c.id} className="border-t border-gray-100">
                        <td className="py-2 text-pine-navy">{c.name}</td>
                        <td className="py-2 text-right">{formatRsd(c.revenue)}</td>
                        <td className="py-2 text-right text-gray-500">{c.invoiceCount}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100">
                <h2 className="text-sm font-medium text-gray-500">{t('topItems')}</h2>
                <table className="mt-4 w-full text-sm">
                  <thead>
                    <tr className="text-left text-xs uppercase tracking-wide text-gray-400">
                      <th className="pb-2">{t('number')}</th>
                      <th className="pb-2">{t('description')}</th>
                      <th className="pb-2 text-right">{t('salesVolume')}</th>
                      <th className="pb-2 text-right">{t('salesValue')}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {items.map((i) => (
                      <tr key={i.id} className="border-t border-gray-100">
                        <td className="py-2 font-medium text-pine-navy">{i.number}</td>
                        <td className="py-2 text-gray-600">{i.description}</td>
                        <td className="py-2 text-right">{i.salesVolume}</td>
                        <td className="py-2 text-right">{formatRsd(i.salesValue)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </>
        )
      )}
    </div>
  );
}

function ComparisonCard({
  label,
  current,
  prior,
  deltaPct,
  vsPriorLabel,
}: {
  label: string;
  current: number;
  prior: number;
  deltaPct: number;
  vsPriorLabel: string;
}) {
  const deltaAmount = current - prior;
  return (
    <div className="rounded-xl bg-white p-6 shadow-sm border border-gray-100">
      <p className="text-sm font-medium text-gray-500">{label}</p>
      <p className="mt-2 text-3xl font-semibold text-pine-navy">{formatRsd(current)}</p>
      <div className={`mt-2 flex items-center gap-2 text-sm font-medium ${deltaClass(deltaPct)}`}>
        <span>{deltaArrow(deltaPct)}</span>
        <span>{deltaPct}%</span>
        <span className="text-gray-400">({formatRsd(deltaAmount)})</span>
      </div>
      <p className="mt-1 text-xs text-gray-400">
        {vsPriorLabel}: {formatRsd(prior)}
      </p>
    </div>
  );
}
