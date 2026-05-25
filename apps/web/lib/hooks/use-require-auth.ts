'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/lib/stores/auth-store';

export function useRequireAuth(requiredRoles?: string[]) {
  const router = useRouter();
  const { isAuthenticated, user, initFromSession } = useAuthStore();

  useEffect(() => {
    initFromSession();
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }
    if (requiredRoles && user && !requiredRoles.includes(user.role)) {
      router.push('/403');
    }
  }, [isAuthenticated, user, router, requiredRoles, initFromSession]);

  return { isAuthenticated, user };
}
