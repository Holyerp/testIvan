'use client';

import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useRequireModule } from '@/lib/hooks/use-require-module';
import { useAuthStore } from '@/lib/stores/auth-store';
import { EntityTable, type Column } from '@/components/entity-table';
import {
  type AdminUser,
  type CreateUserFormData,
  type EditUserFormData,
  createUserSchema,
  editUserSchema,
  errorMessageKey,
  userStatusBadgeClass,
  USER_ROLES,
} from '@/lib/admin/users';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

function formatDateTime(iso: string | null, neverLabel: string): string {
  if (!iso) return neverLabel;
  const d = new Date(iso);
  return Number.isNaN(d.getTime()) ? neverLabel : d.toLocaleString();
}

export default function UserManagementPage() {
  const t = useTranslations('userManagement');
  const { user } = useRequireModule('admin');
  const accessToken = useAuthStore((s) => s.accessToken);

  const [users, setUsers] = useState<AdminUser[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [banner, setBanner] = useState<{ kind: 'success' | 'error'; message: string } | null>(null);

  const [createOpen, setCreateOpen] = useState(false);
  const [editTarget, setEditTarget] = useState<AdminUser | null>(null);

  const authHeaders = useCallback(
    (): HeadersInit => ({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${accessToken}`,
    }),
    [accessToken]
  );

  const loadUsers = useCallback(async () => {
    if (!accessToken) return;
    setIsLoading(true);
    setError(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/v1/admin/users`, { headers: authHeaders() });
      const json = await res.json();
      if (!res.ok || !json.success) throw new Error(json.code ?? 'INTERNAL_ERROR');
      setUsers(json.data as AdminUser[]);
    } catch {
      setError(t('loadError'));
    } finally {
      setIsLoading(false);
    }
  }, [accessToken, authHeaders, t]);

  useEffect(() => {
    void loadUsers();
  }, [loadUsers]);

  const showError = (code: string | undefined) =>
    setBanner({ kind: 'error', message: t(`errors.${errorMessageKey(code)}`) });

  const handleCreate = async (data: CreateUserFormData) => {
    setBanner(null);
    const res = await fetch(`${API_BASE_URL}/api/v1/admin/users`, {
      method: 'POST',
      headers: authHeaders(),
      body: JSON.stringify(data),
    });
    const json = await res.json();
    if (!res.ok || !json.success) {
      showError(json.code);
      return;
    }
    setCreateOpen(false);
    setBanner({ kind: 'success', message: t('userCreated') });
    await loadUsers();
  };

  const handleEdit = async (data: EditUserFormData) => {
    if (!editTarget) return;
    setBanner(null);
    const res = await fetch(`${API_BASE_URL}/api/v1/admin/users/${editTarget.id}`, {
      method: 'PUT',
      headers: authHeaders(),
      body: JSON.stringify(data),
    });
    const json = await res.json();
    if (!res.ok || !json.success) {
      showError(json.code);
      return;
    }
    setEditTarget(null);
    setBanner({ kind: 'success', message: t('userUpdated') });
    await loadUsers();
  };

  const handleResetPassword = async (target: AdminUser) => {
    if (!window.confirm(t('confirmResetPassword', { name: target.name }))) return;
    setBanner(null);
    const res = await fetch(`${API_BASE_URL}/api/v1/admin/users/${target.id}/reset-password`, {
      method: 'POST',
      headers: authHeaders(),
    });
    const json = await res.json();
    if (!res.ok || !json.success) {
      showError(json.code);
      return;
    }
    setBanner({ kind: 'success', message: t('passwordResetSent') });
  };

  const handleDelete = async (target: AdminUser) => {
    if (!window.confirm(t('confirmDelete', { name: target.name }))) return;
    setBanner(null);
    const res = await fetch(`${API_BASE_URL}/api/v1/admin/users/${target.id}`, {
      method: 'DELETE',
      headers: authHeaders(),
    });
    const json = await res.json();
    if (!res.ok || !json.success) {
      showError(json.code);
      return;
    }
    setBanner({ kind: 'success', message: t('userDeleted') });
    await loadUsers();
  };

  if (!user) return null;

  const columns: Column<AdminUser>[] = [
    { key: 'name', header: t('name'), render: (u) => <span className="font-medium text-pine-navy">{u.name}</span> },
    { key: 'email', header: t('email'), render: (u) => u.email ?? '—' },
    { key: 'role', header: t('role'), render: (u) => t(`roles.${u.role}`) },
    { key: 'lastLogin', header: t('lastLogin'), render: (u) => formatDateTime(u.lastLoginAt, t('never')) },
    {
      key: 'status',
      header: t('status'),
      render: (u) => (
        <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${userStatusBadgeClass(u.status)}`}>
          {t(`statusValue.${u.status}`)}
        </span>
      ),
    },
    {
      key: 'actions',
      header: t('actions'),
      align: 'right',
      render: (u) => (
        <div className="flex justify-end gap-2 text-xs">
          <button type="button" onClick={() => setEditTarget(u)} className="text-pine-navy hover:underline">
            {t('editUser')}
          </button>
          <button type="button" onClick={() => void handleResetPassword(u)} className="text-pine-navy hover:underline">
            {t('resetPassword')}
          </button>
          <button type="button" onClick={() => void handleDelete(u)} className="text-red-600 hover:underline">
            {t('delete')}
          </button>
        </div>
      ),
    },
  ];

  return (
    <div className="p-8 font-sans">
      <div className="flex items-start justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-pine-navy">{t('title')}</h1>
          <p className="mt-1 text-gray-500">{t('subtitle')}</p>
        </div>
        <button
          type="button"
          onClick={() => {
            setBanner(null);
            setCreateOpen(true);
          }}
          className="rounded-lg bg-pine-navy px-4 py-2 text-sm font-medium text-white hover:bg-pine-navy-mid"
        >
          {t('createUser')}
        </button>
      </div>

      {banner && (
        <div
          role="alert"
          className={`mt-4 rounded-lg border px-4 py-3 text-sm ${
            banner.kind === 'success'
              ? 'border-green-200 bg-green-50 text-green-700'
              : 'border-red-200 bg-red-50 text-red-700'
          }`}
        >
          {banner.message}
        </div>
      )}

      <div className="mt-6">
        <EntityTable<AdminUser>
          columns={columns}
          rows={users}
          rowKey={(u) => u.id}
          isLoading={isLoading}
          error={error}
          emptyMessage={t('noResults')}
          page={1}
          totalPages={1}
          onPageChange={() => undefined}
          labels={{ previous: t('previous'), next: t('next'), pageOf: t('pageOf', { page: '1', total: '1' }), loadError: t('loadError') }}
        />
      </div>

      {createOpen && (
        <CreateUserModal onClose={() => setCreateOpen(false)} onSubmit={handleCreate} />
      )}
      {editTarget && (
        <EditUserModal target={editTarget} onClose={() => setEditTarget(null)} onSubmit={handleEdit} />
      )}
    </div>
  );
}

interface ModalShellProps {
  title: string;
  children: React.ReactNode;
}

function ModalShell({ title, children }: ModalShellProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4">
      <div className="w-full max-w-md rounded-2xl bg-white p-6 shadow-xl">
        <h2 className="mb-4 text-lg font-semibold text-pine-navy">{title}</h2>
        {children}
      </div>
    </div>
  );
}

const inputClass =
  'w-full rounded-lg border border-gray-300 px-3 py-2 text-sm focus:border-transparent focus:outline-none focus:ring-2 focus:ring-pine-navy';

function CreateUserModal({
  onClose,
  onSubmit,
}: {
  onClose: () => void;
  onSubmit: (data: CreateUserFormData) => Promise<void>;
}) {
  const t = useTranslations('userManagement');
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<CreateUserFormData>({ resolver: zodResolver(createUserSchema) });

  return (
    <ModalShell title={t('createUser')}>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4" noValidate>
        <Field label={t('name')} error={errors.name && t('errors.requiredFields')}>
          <input className={inputClass} {...register('name')} />
        </Field>
        <Field label={t('email')} error={errors.email && t('errors.invalidEmail')}>
          <input type="email" className={inputClass} {...register('email')} />
        </Field>
        <Field label={t('username')} error={errors.username && t('errors.requiredFields')}>
          <input className={inputClass} {...register('username')} />
        </Field>
        <Field label={t('role')}>
          <select className={inputClass} defaultValue="MANAGER" {...register('role')}>
            {USER_ROLES.map((r) => (
              <option key={r} value={r}>
                {t(`roles.${r}`)}
              </option>
            ))}
          </select>
        </Field>
        <Field label={t('tempPassword')} error={errors.tempPassword && t('errors.passwordTooShort')}>
          <input type="text" className={inputClass} {...register('tempPassword')} />
        </Field>
        <ModalActions onClose={onClose} isSubmitting={isSubmitting} saveLabel={t('save')} cancelLabel={t('cancel')} />
      </form>
    </ModalShell>
  );
}

function EditUserModal({
  target,
  onClose,
  onSubmit,
}: {
  target: AdminUser;
  onClose: () => void;
  onSubmit: (data: EditUserFormData) => Promise<void>;
}) {
  const t = useTranslations('userManagement');
  const {
    register,
    handleSubmit,
    formState: { isSubmitting },
  } = useForm<EditUserFormData>({
    resolver: zodResolver(editUserSchema),
    defaultValues: { role: target.role, isActive: target.status === 'ACTIVE' },
  });

  return (
    <ModalShell title={`${t('editUser')} — ${target.name}`}>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4" noValidate>
        <Field label={t('role')}>
          <select className={inputClass} {...register('role')}>
            {USER_ROLES.map((r) => (
              <option key={r} value={r}>
                {t(`roles.${r}`)}
              </option>
            ))}
          </select>
        </Field>
        <label className="flex items-center gap-2 text-sm text-gray-700">
          <input type="checkbox" {...register('isActive')} />
          {t('active')}
        </label>
        <ModalActions onClose={onClose} isSubmitting={isSubmitting} saveLabel={t('save')} cancelLabel={t('cancel')} />
      </form>
    </ModalShell>
  );
}

function Field({
  label,
  error,
  children,
}: {
  label: string;
  error?: string | false;
  children: React.ReactNode;
}) {
  return (
    <div>
      <label className="mb-1 block text-sm font-medium text-gray-700">{label}</label>
      {children}
      {error ? <p className="mt-1 text-xs text-red-500">{error}</p> : null}
    </div>
  );
}

function ModalActions({
  onClose,
  isSubmitting,
  saveLabel,
  cancelLabel,
}: {
  onClose: () => void;
  isSubmitting: boolean;
  saveLabel: string;
  cancelLabel: string;
}) {
  return (
    <div className="flex justify-end gap-2 pt-2">
      <button
        type="button"
        onClick={onClose}
        className="rounded-lg border border-gray-300 px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-50"
      >
        {cancelLabel}
      </button>
      <button
        type="submit"
        disabled={isSubmitting}
        className="rounded-lg bg-pine-navy px-4 py-2 text-sm font-medium text-white hover:bg-pine-navy-mid disabled:opacity-50"
      >
        {saveLabel}
      </button>
    </div>
  );
}
