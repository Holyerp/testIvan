'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/lib/stores/auth-store';

const loginSchema = z.object({
  username: z.string().min(1, 'Korisničko ime je obavezno'),
  password: z.string().min(1, 'Lozinka je obavezna'),
});

type LoginFormData = z.infer<typeof loginSchema>;

const ERROR_MESSAGES: Record<string, string> = {
  AUTH_INVALID_CREDENTIALS: 'Pogrešno korisničko ime ili lozinka.',
  AUTH_RATE_LIMITED: 'Previše neuspješnih pokušaja. Pokušajte za 15 minuta.',
  VALIDATION_REQUIRED_FIELDS: 'Korisničko ime i lozinka su obavezni.',
  INTERNAL_ERROR: 'Greška servera. Pokušajte ponovo.',
};

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';

export default function LoginPage() {
  const router = useRouter();
  const [serverError, setServerError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormData) => {
    setIsLoading(true);
    setServerError(null);
    try {
      const res = await fetch(`${API_BASE_URL}/api/v1/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
        credentials: 'include',
      });
      const json = await res.json();
      if (!res.ok || !json.success) {
        const code: string = json.code ?? 'INTERNAL_ERROR';
        setServerError(ERROR_MESSAGES[code] ?? ERROR_MESSAGES['INTERNAL_ERROR']);
      } else {
        sessionStorage.setItem('access_token', json.data.accessToken);
        sessionStorage.setItem('token_expires_at', json.data.expiresAt);
        sessionStorage.setItem('user', JSON.stringify(json.data.user));
        useAuthStore.getState().setAuth(json.data.accessToken, json.data.expiresAt, json.data.user);
        router.push('/dashboard');
      }
    } catch {
      setServerError(ERROR_MESSAGES['INTERNAL_ERROR']);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-pine-navy flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-xl w-full max-w-md p-8">
        <div className="text-center mb-8">
          <h1 className="text-2xl font-bold text-pine-navy font-sans">Pinoles</h1>
          <p className="text-gray-500 mt-1 text-sm">Interni portal</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5" noValidate>
          <div>
            <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-1">
              Korisničko ime
            </label>
            <input
              id="username"
              type="text"
              autoComplete="username"
              className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-pine-navy focus:border-transparent"
              {...register('username')}
            />
            {errors.username && (
              <p className="text-red-500 text-xs mt-1">{errors.username.message}</p>
            )}
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
              Lozinka
            </label>
            <input
              id="password"
              type="password"
              autoComplete="current-password"
              className="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-pine-navy focus:border-transparent"
              {...register('password')}
            />
            {errors.password && (
              <p className="text-red-500 text-xs mt-1">{errors.password.message}</p>
            )}
          </div>

          {serverError && (
            <div
              role="alert"
              className="bg-red-50 border border-red-200 rounded-lg px-4 py-3 text-sm text-red-700"
            >
              {serverError}
            </div>
          )}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-pine-navy hover:bg-pine-navy-mid text-white font-medium py-2.5 rounded-lg transition-colors disabled:opacity-50"
          >
            {isLoading ? 'Prijavljivanje...' : 'Prijavi se'}
          </button>
        </form>
      </div>
    </div>
  );
}
