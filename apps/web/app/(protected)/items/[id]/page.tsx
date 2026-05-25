'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd, ledgerTypeKey, formatSignedQuantity } from '@/lib/format';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

interface ItemProfile {
  id: string;
  number: string;
  description: string;
  category: string;
  unitOfMeasure: string;
  quantityOnHand: number;
  minimumStock: number;
  unitCost: number;
  unitPrice: number;
  isLowStock: boolean;
}

interface StockByLocation {
  location: string;
  quantityOnHand: number;
  quantityReserved: number;
}

interface ItemLedgerEntry {
  date: string;
  entryType: string;
  quantity: number;
  remaining: number;
}

interface ItemDetail {
  item: ItemProfile;
  stockByLocation: StockByLocation[];
  recentLedgerEntries: ItemLedgerEntry[];
}

function DetailSkeleton() {
  return (
    <div className="mt-6 animate-pulse space-y-6">
      <div className="h-8 w-64 rounded bg-gray-200" />
      <div className="h-28 rounded-xl bg-gray-200" />
      <div className="h-48 rounded-xl bg-gray-200" />
      <div className="h-64 rounded-xl bg-gray-200" />
    </div>
  );
}

function ProfileField({ label, value }: { label: string; value: string }) {
  if (!value) return null;
  return (
    <div>
      <dt className="text-xs font-medium uppercase tracking-wide text-gray-400">{label}</dt>
      <dd className="mt-0.5 text-sm text-gray-700">{value}</dd>
    </div>
  );
}

export default function ItemDetailPage() {
  const t = useTranslations('itemDetail');
  const tItems = useTranslations('items');
  // WAREHOUSE module guard: ACCOUNTING is redirected to /403; ADMIN/MANAGER/WAREHOUSE pass.
  const { user } = useRequireModule('warehouse');
  const accessToken = useAuthStore((s) => s.accessToken);
  const params = useParams();
  const id = Array.isArray(params.id) ? params.id[0] : params.id;

  const [detail, setDetail] = useState<ItemDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    if (!accessToken || !id) return;

    let active = true;
    setIsLoading(true);
    setNotFound(false);
    setHasError(false);

    fetch(`${API_BASE_URL}/api/v1/items/${encodeURIComponent(id)}`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    })
      .then(async (res) => {
        const json = await res.json();
        if (res.status === 404 || json.code === 'NOT_FOUND_ITEM') {
          const err = new Error('NOT_FOUND_ITEM');
          err.name = 'NOT_FOUND_ITEM';
          throw err;
        }
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as ItemDetail;
      })
      .then((data) => {
        if (active) {
          setDetail(data);
          setIsLoading(false);
        }
      })
      .catch((err: Error) => {
        if (!active) return;
        if (err.name === 'NOT_FOUND_ITEM') {
          setNotFound(true);
        } else {
          setHasError(true);
        }
        setIsLoading(false);
      });

    return () => {
      active = false;
    };
  }, [accessToken, id]);

  if (!user) return null;

  return (
    <div className="p-8 font-sans">
      <Link
        href="/items"
        className="inline-flex items-center gap-1 text-sm font-medium text-pine-green hover:text-pine-green-dark"
      >
        ← {t('back')}
      </Link>

      {notFound ? (
        <div className="mt-6 rounded-lg border border-gray-200 bg-white px-4 py-10 text-center text-gray-500">
          {t('notFound')}
        </div>
      ) : hasError ? (
        <div className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {t('loadError')}
        </div>
      ) : isLoading ? (
        <DetailSkeleton />
      ) : detail ? (
        <>
          <div className="mt-6">
            <h1 className="text-2xl font-semibold text-pine-navy">{detail.item.number}</h1>
            <p className="mt-1 text-sm text-gray-500">{detail.item.description}</p>
          </div>

          {detail.item.isLowStock && (
            <div className="mt-4 rounded-lg border border-amber-200 bg-amber-50 px-4 py-3 text-sm font-medium text-amber-700">
              {t('lowStockAlert')}
            </div>
          )}

          <div className="mt-6 rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
            <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
              <ProfileField label={t('category')} value={detail.item.category} />
              <ProfileField label={t('unitOfMeasure')} value={detail.item.unitOfMeasure} />
              <ProfileField label={t('unitCost')} value={formatRsd(detail.item.unitCost)} />
              <ProfileField label={t('unitPrice')} value={formatRsd(detail.item.unitPrice)} />
            </dl>
          </div>

          <div className="mt-8">
            <h2 className="text-sm font-medium text-gray-500">{t('stockByLocation')}</h2>
            <div className="mt-3 overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
              <table className="w-full text-left text-sm">
                <thead>
                  <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
                    <th className="px-4 py-3 font-medium">{t('location')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('quantityOnHand')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('quantityReserved')}</th>
                  </tr>
                </thead>
                <tbody>
                  {detail.stockByLocation.map((s) => (
                    <tr
                      key={s.location}
                      className="border-b border-gray-50 last:border-0 hover:bg-gray-50"
                    >
                      <td className="px-4 py-3 font-medium text-pine-navy">{s.location}</td>
                      <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                        {s.quantityOnHand}
                      </td>
                      <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                        {s.quantityReserved}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>

          <div className="mt-8">
            <h2 className="text-sm font-medium text-gray-500">{t('ledgerEntries')}</h2>
            <div className="mt-3 overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
              <table className="w-full text-left text-sm">
                <thead>
                  <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
                    <th className="px-4 py-3 font-medium">{t('date')}</th>
                    <th className="px-4 py-3 font-medium">{t('type')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('quantity')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('remaining')}</th>
                  </tr>
                </thead>
                <tbody>
                  {detail.recentLedgerEntries.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="px-4 py-10 text-center text-gray-400">
                        {t('noEntries')}
                      </td>
                    </tr>
                  ) : (
                    detail.recentLedgerEntries.map((e, idx) => (
                      <tr
                        key={`${e.date}-${idx}`}
                        className="border-b border-gray-50 last:border-0 hover:bg-gray-50"
                      >
                        <td className="px-4 py-3 text-gray-500">{e.date}</td>
                        <td className="px-4 py-3">
                          <span className="inline-flex rounded-full bg-gray-100 px-2.5 py-0.5 text-xs font-medium text-gray-700">
                            {tItems(`ledgerType.${ledgerTypeKey(e.entryType)}`)}
                          </span>
                        </td>
                        <td
                          className={`px-4 py-3 text-right tabular-nums ${
                            e.quantity < 0 ? 'text-red-600' : 'text-green-700'
                          }`}
                        >
                          {formatSignedQuantity(e.quantity)}
                        </td>
                        <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                          {e.remaining}
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </>
      ) : null}
    </div>
  );
}
