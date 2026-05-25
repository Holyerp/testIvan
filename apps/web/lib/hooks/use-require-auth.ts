'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/lib/stores/auth-store';

export function useRequireAuth(requiredRoles?: string[]) {
  const router = useRouter();
  const { isAuthenticated, user } = useAuthStore();

  useEffect(() => {
    // Hydrate from sessionStorage, then decide using the FRESH store state.
    // Zustand's set() is synchronous, so getState() already reflects
    // initFromSession() here. Reading the render-time `isAuthenticated` would
    // be stale on first mount (false) and bounce a valid session to /login.
    useAuthStore.getState().initFromSession();
    const { isAuthenticated: authed, user: currentUser } = useAuthStore.getState();

    if (!authed) {
      router.push('/login');
      return;
    }
    if (requiredRoles && currentUser && !requiredRoles.includes(currentUser.role)) {
      router.push('/403');
    }
  }, [router, requiredRoles]);

  return { isAuthenticated, user };
}
