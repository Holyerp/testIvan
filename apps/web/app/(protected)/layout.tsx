import { AuthProvider } from '@/components/auth-provider';
import { Sidebar } from '@/components/sidebar';

export default function ProtectedLayout({ children }: { children: React.ReactNode }) {
  return (
    <AuthProvider>
      <div className="flex h-screen bg-gray-50">
        <Sidebar />
        <main className="flex-1 overflow-auto">{children}</main>
      </div>
    </AuthProvider>
  );
}
