'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRequireAuth } from '@/lib/hooks/use-require-auth';
import { useAuthStore } from '@/lib/stores/auth-store';
import { formatRsd, isOverdue, statusBadgeClass } from '@/lib/format';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

interface InvoiceHeader {
  id: string;
  number: string;
  customerName: string;
  billToAddress: string;
  postingDate: string;
  dueDate: string;
  paymentTerms: string;
  status: string; // OPEN | PARTIAL | PAID
}

interface InvoiceLineItem {
  description: string;
  quantity: number;
  unitPrice: number;
  vatPercent: number;
  lineTotal: number;
}

interface InvoiceTotalsData {
  subtotal: number;
  vatAmount: number;
  total: number;
}

interface SalesInvoiceDetail {
  header: InvoiceHeader;
  lines: InvoiceLineItem[];
  totals: InvoiceTotalsData;
}

function DetailSkeleton() {
  return (
    <div className="mt-6 animate-pulse space-y-6">
      <div className="h-8 w-64 rounded bg-gray-200" />
      <div className="h-40 rounded-xl bg-gray-200" />
      <div className="h-64 rounded-xl bg-gray-200" />
    </div>
  );
}

export default function SalesInvoiceDetailPage() {
  const t = useTranslations('salesDetail');
  const tSales = useTranslations('sales');
  const { user } = useRequireAuth(['ADMIN', 'MANAGER', 'ACCOUNTING']);
  const accessToken = useAuthStore((s) => s.accessToken);
  const params = useParams();
  const id = Array.isArray(params.id) ? params.id[0] : params.id;
  const today = new Date().toISOString().slice(0, 10);

  const [detail, setDetail] = useState<SalesInvoiceDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    if (!accessToken || !id) return;

    let active = true;
    setIsLoading(true);
    setNotFound(false);
    setHasError(false);

    fetch(`${API_BASE_URL}/api/v1/sales/invoices/${encodeURIComponent(id)}`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    })
      .then(async (res) => {
        const json = await res.json();
        if (res.status === 404 || json.code === 'NOT_FOUND_SALES_INVOICE') {
          const err = new Error('NOT_FOUND_SALES_INVOICE');
          err.name = 'NOT_FOUND_SALES_INVOICE';
          throw err;
        }
        if (!res.ok || !json.success) throw new Error(json.code ?? 'INTEGRATION_BC_UNAVAILABLE');
        return json.data as SalesInvoiceDetail;
      })
      .then((data) => {
        if (active) {
          setDetail(data);
          setIsLoading(false);
        }
      })
      .catch((err: Error) => {
        if (!active) return;
        if (err.name === 'NOT_FOUND_SALES_INVOICE') {
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

  const overdue = detail
    ? isOverdue(detail.header.status, detail.header.dueDate, today)
    : false;

  return (
    <div className="p-8 font-sans">
      <Link
        href="/sales/invoices"
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
          <div className="mt-6 flex flex-wrap items-center gap-3">
            <h1 className="text-2xl font-semibold text-pine-navy">{detail.header.number}</h1>
            <span
              className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-medium ${statusBadgeClass(detail.header.status)}`}
            >
              {tSales(`status.${detail.header.status}`)}
            </span>
            {overdue && (
              <span className="inline-block rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-700">
                {t('overdue')}
              </span>
            )}
          </div>

          {/* Header card */}
          <div className="mt-6 grid grid-cols-1 gap-x-8 gap-y-4 rounded-xl border border-gray-100 bg-white p-6 shadow-sm sm:grid-cols-2">
            <Field label={t('customer')} value={detail.header.customerName} />
            <Field label={t('billTo')} value={detail.header.billToAddress} />
            <Field label={t('postingDate')} value={detail.header.postingDate} />
            <Field
              label={t('dueDate')}
              value={detail.header.dueDate}
              valueClassName={overdue ? 'text-red-600' : undefined}
            />
            <Field label={t('paymentTerms')} value={detail.header.paymentTerms} />
          </div>

          {/* Line items */}
          <div className="mt-8">
            <h2 className="text-sm font-medium text-gray-500">{t('lineItems')}</h2>
            <div className="mt-3 overflow-hidden rounded-xl border border-gray-100 bg-white shadow-sm">
              <table className="w-full text-left text-sm">
                <thead>
                  <tr className="border-b border-gray-100 bg-gray-50 text-xs uppercase tracking-wide text-gray-500">
                    <th className="px-4 py-3 font-medium">{t('lineDescription')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('quantity')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('unitPrice')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('vatPercent')}</th>
                    <th className="px-4 py-3 text-right font-medium">{t('lineTotal')}</th>
                  </tr>
                </thead>
                <tbody>
                  {detail.lines.length === 0 ? (
                    <tr>
                      <td colSpan={5} className="px-4 py-10 text-center text-gray-400">
                        {t('noLines')}
                      </td>
                    </tr>
                  ) : (
                    detail.lines.map((line, idx) => (
                      <tr
                        key={`${line.description}-${idx}`}
                        className="border-b border-gray-50 last:border-0"
                      >
                        <td className="px-4 py-3 text-gray-700">{line.description}</td>
                        <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                          {line.quantity}
                        </td>
                        <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                          {formatRsd(line.unitPrice)}
                        </td>
                        <td className="px-4 py-3 text-right tabular-nums text-gray-700">
                          {line.vatPercent}%
                        </td>
                        <td className="px-4 py-3 text-right tabular-nums font-medium text-pine-navy">
                          {formatRsd(line.lineTotal)}
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>

          {/* Totals */}
          <div className="mt-6 flex justify-end">
            <div className="w-full max-w-xs space-y-2 rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
              <TotalRow label={t('subtotal')} value={detail.totals.subtotal} />
              <TotalRow label={t('vat')} value={detail.totals.vatAmount} />
              <div className="border-t border-gray-100 pt-2">
                <TotalRow label={t('total')} value={detail.totals.total} emphasized />
              </div>
            </div>
          </div>
        </>
      ) : null}
    </div>
  );
}

function Field({
  label,
  value,
  valueClassName,
}: {
  label: string;
  value: string;
  valueClassName?: string;
}) {
  return (
    <div>
      <p className="text-xs font-medium uppercase tracking-wide text-gray-500">{label}</p>
      <p className={`mt-1 text-sm text-pine-navy ${valueClassName ?? ''}`}>{value || '—'}</p>
    </div>
  );
}

function TotalRow({
  label,
  value,
  emphasized = false,
}: {
  label: string;
  value: number;
  emphasized?: boolean;
}) {
  return (
    <div className="flex items-center justify-between">
      <span className={emphasized ? 'text-sm font-semibold text-pine-navy' : 'text-sm text-gray-500'}>
        {label}
      </span>
      <span
        className={`tabular-nums ${emphasized ? 'text-lg font-semibold text-pine-navy' : 'text-sm text-gray-700'}`}
      >
        {formatRsd(value)}
      </span>
    </div>
  );
}
