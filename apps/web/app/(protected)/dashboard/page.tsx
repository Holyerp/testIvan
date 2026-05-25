'use client';

import { useRequireAuth } from '@/lib/hooks/use-require-auth';

export default function DashboardPage() {
  const { user } = useRequireAuth();

  if (!user) return null;

  return (
    <main className="p-8">
      <h1 className="text-2xl font-semibold text-pine-navy">Dashboard</h1>
      <p className="text-gray-500 mt-2">KPI overview — coming in US-003.</p>
      <div className="mt-4 text-sm text-gray-400">
        Logged in as <strong>{user.username}</strong> ({user.role})
      </div>
    </main>
  );
}
