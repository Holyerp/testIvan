'use client';

import { useAuthStore } from '@/lib/stores/auth-store';

interface RoleGuardProps {
  roles: string[];
  children: React.ReactNode;
  fallback?: React.ReactNode;
}

export function RoleGuard({ roles, children, fallback = null }: RoleGuardProps) {
  const user = useAuthStore((s) => s.user);

  if (!user) return <>{fallback}</>;
  if (!roles.includes(user.role)) return <>{fallback}</>;

  return <>{children}</>;
}
