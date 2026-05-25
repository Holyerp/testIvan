'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd, statusBadgeClass } from '@/lib/format';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

interface VendorProfile {
  id: string;
  number: string;
  displayName: string;
  address: string;
  city: string;
  phone: string;
  email: string;
  vatNumber: string;
  balance: number;
  paymentTerms: string;
}

interface VendorInvoice {
  id: string;
  number: string;
  postingDate: string;
  amount: number;
  status: string;
}

interface VendorDetail {
  vendor: VendorProfile;
  invoices: VendorInvoice[];
}

function DetailSkeleton() {
  return (
    <div className="mt-6 animate-pulse space-y-6">
      <div className="h-8 w-64 rounded bg-gray-200" />
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <div className="h-28 rounded-xl bg-gray-200" />
        <div className="h-28 rounded-xl bg-gray-200" />
      </div>
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

export default function VendorDetailPage() {
  const t = useTranslations('vendorDetail');
  const tPurchase = useTranslations('purchase');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const accessToken = useAuthStore((s) => s.accessToken);
  const params = useParams();
  const id = Array.isArray(params.id) ? params.id[0] : params.id;

  const [detail, setDetail] = useState<VendorDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    if (!accessToken || !id) return;

    let active = true;
    setIsLoading(true);
    setNotFound(false);
    setHasError(false);

    fetch(`${API_BASE_URL}/api/v1/vendors/${encodeURIComponent(id)}`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    })
      .then(async (res) => {
        const json = await res.json();
        if (res.status === 404 || json.code === 'NOT_FOUND_VENDOR') {
          const err = new Error('NOT_FOUND_VENDOR');
          err.name = 'NOT_FOUND_VENDOR';
          throw err;
        }
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as VendorDetail;
      })
      .then((data) => {
        if (active) {
          setDetail(data);
          setIsLoading(false);
        }
      })
      .catch((err: Error) => {
        if (!active) return;
        if (err.name === 'NOT_FOUND_VENDOR') {
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
        href="/vendors"
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
            <h1 className="text-2xl font-semibold text-pine-navy">
              {detail.vendor.displayName}
            </h1>
            <p className="mt-1 text-sm text-gray-500">
              {detail.vendor.number}
              {detail.vendor.city ? ` · ${detail.vendor.city}` : ''}
            </p>
          </div>

          <div className="mt-6 rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
            <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
              <ProfileField label={t('address')} value={detail.vendor.address} />
              <ProfileField label={t('phone')} value={detail.vendor.phone} />
              <ProfileField label={t('email')} value={detail.vendor.email} />
              <ProfileField label={t('vatNumber')} value={detail.vendor.vatNumber} />
            </dl>
          </div>

          <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
            <div className="rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
              <p className="text-sm font-medium text-gray-500">{t('balance')}</p>
              <p className="mt-2 text-3xl font-semibold text-pine-navy tabular-nums">
                {formatRsd(detail.vendor.balance)}
              </p>
            </div>
            <div className="rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
              <p className="text-sm font-medium text-gray-500">{t('paymentTerms')}</p>
              <p className="mt-2 text-3xl font-semibold text-pine-navy">
                {detail.vendor.paymentTerms || '—'}
              </p>
            </div>
          </div>

          <div className="mt-8">
            <h2 className="text-sm font-medium text-gray-500">{t('purchaseHistory')}</h2>
            <div className="mt-3 overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
              <table className="w-full text-left text-sm">
                <thead>
                  <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
                    <th className="px-4 py-3 font-medium">{t('invoiceNumber')}</th>
                    <th className="px-4 py-3 font-medium">{t('date')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('amount')}</th>
                    <th className="px-4 py-3 font-medium">{t('status')}</th>
                  </tr>
                </thead>
                <tbody>
                  {detail.invoices.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="px-4 py-10 text-center text-gray-400">
                        {t('noInvoices')}
                      </td>
                    </tr>
                  ) : (
                    detail.invoices.map((inv) => (
                      <tr
                        key={inv.id}
                        className="border-b border-gray-50 last:border-0 hover:bg-gray-50"
                      >
                        <td className="px-4 py-3 font-medium text-pine-navy">{inv.number}</td>
                        <td className="px-4 py-3 text-gray-500">{inv.postingDate}</td>
                        <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                          {formatRsd(inv.amount)}
                        </td>
                        <td className="px-4 py-3">
                          <span
                            className={`inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium ${statusBadgeClass(
                              inv.status,
                            )}`}
                          >
                            {tPurchase(`status.${inv.status}`)}
                          </span>
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
