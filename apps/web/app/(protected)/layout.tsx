import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import { AuthProvider } from '@/components/auth-provider';
import { Sidebar } from '@/components/sidebar';
import { GlobalSearch } from '@/components/global-search';
import { NotificationBell } from '@/components/notification-bell';

export default async function ProtectedLayout({ children }: { children: React.ReactNode }) {
  const messages = await getMessages();

  return (
    <NextIntlClientProvider messages={messages}>
      <AuthProvider>
        <div className="flex h-screen bg-gray-50">
          <Sidebar />
          <div className="flex flex-1 flex-col overflow-hidden">
            <header className="flex h-16 items-center justify-between border-b border-gray-200 bg-white px-8">
              <GlobalSearch />
              <NotificationBell />
            </header>
            <main className="flex-1 overflow-auto">{children}</main>
          </div>
        </div>
      </AuthProvider>
    </NextIntlClientProvider>
  );
}
