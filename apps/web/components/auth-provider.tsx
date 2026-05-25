'use client';

import { useEffect } from 'react';
import { useAuthStore } from '@/lib/stores/auth-store';

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const initFromSession = useAuthStore((s) => s.initFromSession);

  useEffect(() => {
    initFromSession();
  }, [initFromSession]);

  return <>{children}</>;
}
