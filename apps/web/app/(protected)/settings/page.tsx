'use client';

import { useCallback, useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { useAuthStore } from '@/lib/stores/auth-store';
import {
  type BcConfig,
  type BcConnectionTestResult,
  connectionModeBadgeClass,
  connectionModeKey,
  formatEntityType,
  formatSyncTimestamp,
} from '@/lib/settings';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

export default function SettingsPage() {
  const t = useTranslations('settings');
  const { user } = useRequireModule('admin');
  const accessToken = useAuthStore((s) => s.accessToken);

  const [config, setConfig] = useState<BcConfig | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isTesting, setIsTesting] = useState(false);
  const [testResult, setTestResult] = useState<BcConnectionTestResult | null>(null);

  const authHeaders = useCallback(
    (): HeadersInit => ({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${accessToken}`,
    }),
    [accessToken]
  );

  const loadConfig = useCallback(async () => {
    if (!accessToken) return;
    setIsLoading(true);
    setError(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/v1/settings/bc-config`, { headers: authHeaders() });
      const json = await res.json();
      if (!res.ok || !json.success) throw new Error(json.code ?? 'INTERNAL_ERROR');
      setConfig(json.data as BcConfig);
    } catch {
      setError(t('loadError'));
    } finally {
      setIsLoading(false);
    }
  }, [accessToken, authHeaders, t]);

  useEffect(() => {
    void loadConfig();
  }, [loadConfig]);

  const handleTestConnection = async () => {
    if (!accessToken) return;
    setIsTesting(true);
    setTestResult(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/v1/settings/bc-config/test`, {
        method: 'POST',
        headers: authHeaders(),
      });
      const json = await res.json();
      if (!res.ok || !json.success) throw new Error(json.code ?? 'INTERNAL_ERROR');
      setTestResult(json.data as BcConnectionTestResult);
      // Refresh the config so the "Last Sync" table reflects a successful probe.
      if ((json.data as BcConnectionTestResult).success) void loadConfig();
    } catch {
      setTestResult({ success: false, durationMs: 0, lastSuccessfulSyncAt: null, errorCode: 'INTEGRATION_BC_UNAVAILABLE' });
    } finally {
      setIsTesting(false);
    }
  };

  if (!user) return null;

  return (
    <div className="p-8 font-sans">
      <div>
        <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
        <p className="mt-1 text-gray-500">{t('subtitle')}</p>
      </div>

      {error && (
        <div role="alert" className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {error}
        </div>
      )}

      {isLoading ? (
        <div className="mt-6 h-48 animate-pulse rounded-xl border border-gray-100 bg-white" />
      ) : config ? (
        <>
          <section className="mt-6 rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
            <div className="flex items-start justify-between">
              <h2 className="text-lg font-semibold text-pine-navy">{t('bcConnection')}</h2>
              <span
                className={`inline-flex items-center gap-2 rounded-full px-3 py-1 text-xs font-medium ${connectionModeBadgeClass(
                  config.useMock
                )}`}
              >
                <span className={`h-2 w-2 rounded-full ${config.useMock ? 'bg-amber-500' : 'bg-pine-green'}`} />
                {t(connectionModeKey(config.useMock))}
              </span>
            </div>

            <dl className="mt-4 grid grid-cols-1 gap-4 sm:grid-cols-2">
              <ConfigField label={t('tenantId')} value={config.tenantId || '—'} />
              <ConfigField label={t('companyId')} value={config.companyId || '—'} />
              <ConfigField label={t('environment')} value={config.environment || '—'} />
              <ConfigField label={t('baseUrl')} value={config.baseUrl || '—'} />
              <ConfigField label={t('mode')} value={t(connectionModeKey(config.useMock))} />
            </dl>

            <div className="mt-6 flex items-center gap-4">
              <button
                type="button"
                onClick={() => void handleTestConnection()}
                disabled={isTesting}
                className="rounded-lg bg-pine-navy px-4 py-2 text-sm font-medium text-white hover:bg-pine-navy-mid disabled:opacity-50"
              >
                {isTesting ? t('testing') : t('testConnection')}
              </button>

              {isTesting && (
                <span className="inline-block h-4 w-4 animate-spin rounded-full border-2 border-pine-navy border-t-transparent" />
              )}

              {testResult && !isTesting && (
                <span
                  className={`text-sm font-medium ${testResult.success ? 'text-green-700' : 'text-red-600'}`}
                  role="status"
                >
                  {testResult.success
                    ? `✓ ${t('connected', {
                        ms: String(testResult.durationMs),
                        time: formatSyncTimestamp(testResult.lastSuccessfulSyncAt, t('never')),
                      })}`
                    : `✕ ${t('connectionFailed')}`}
                </span>
              )}
            </div>
          </section>

          <section className="mt-6 rounded-xl border border-gray-100 bg-white p-6 shadow-sm">
            <h2 className="text-lg font-semibold text-pine-navy">{t('lastSync')}</h2>
            <table className="mt-4 w-full text-sm">
              <thead>
                <tr className="border-b border-gray-100 text-left text-gray-500">
                  <th className="pb-2 font-medium">{t('entityType')}</th>
                  <th className="pb-2 font-medium">{t('lastSyncedAt')}</th>
                </tr>
              </thead>
              <tbody>
                {config.lastSync.map((row) => (
                  <tr key={row.entityType} className="border-b border-gray-50 last:border-0">
                    <td className="py-2 font-medium text-pine-navy">{formatEntityType(row.entityType)}</td>
                    <td className="py-2 text-gray-600">{formatSyncTimestamp(row.lastSyncedAt, t('never'))}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </section>
        </>
      ) : null}
    </div>
  );
}

function ConfigField({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <dt className="text-xs font-medium uppercase tracking-wide text-gray-400">{label}</dt>
      <dd className="mt-1 break-all text-sm text-pine-navy">{value}</dd>
    </div>
  );
}
