import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import { AuthProvider } from '@/components/auth-provider';
import { Sidebar } from '@/components/sidebar';

export default async function ProtectedLayout({ children }: { children: React.ReactNode }) {
  const messages = await getMessages();

  return (
    <NextIntlClientProvider messages={messages}>
      <AuthProvider>
        <div className="flex h-screen bg-gray-50">
          <Sidebar />
          <main className="flex-1 overflow-auto">{children}</main>
        </div>
      </AuthProvider>
    </NextIntlClientProvider>
  );
}
