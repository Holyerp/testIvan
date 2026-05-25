'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/lib/stores/auth-store';
import { canAccessModule, type Module } from '@/lib/auth/module-access';

export function useRequireModule(module: Module) {
  const router = useRouter();
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
  const user = useAuthStore((s) => s.user);

  useEffect(() => {
    // Hydrate from sessionStorage, then decide using the FRESH store state.
    // Reading the render-time state would be stale on first mount and bounce a
    // valid session. Mirrors the getState() pattern in use-require-auth.ts.
    useAuthStore.getState().initFromSession();
    const { isAuthenticated: authed, user: currentUser } = useAuthStore.getState();

    if (!authed) {
      router.push('/login');
      return;
    }
    if (!canAccessModule(module, currentUser?.role)) {
      router.push('/403');
    }
  }, [router, module]);

  return { isAuthenticated, user };
}
