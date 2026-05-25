'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { useAuthStore } from '@/lib/stores/auth-store';
import { canAccessModule, type Module } from '@/lib/auth/module-access';

interface NavItem {
  href: string;
  label: string;
  module: Module;
}

const NAV_ITEMS: NavItem[] = [
  { href: '/dashboard', label: 'Dashboard', module: 'dashboard' },
  { href: '/customers', label: 'Customers', module: 'financial' },
  { href: '/invoices', label: 'Invoices', module: 'financial' },
  { href: '/sales/invoices', label: 'Sales Invoices', module: 'financial' },
  { href: '/sales/advance-invoices', label: 'Advance Invoices', module: 'financial' },
  { href: '/sales/credit-memos', label: 'Credit Memos', module: 'financial' },
  { href: '/credit-documents', label: 'Credit Documents', module: 'financial' },
  { href: '/purchase/invoices', label: 'Purchase Invoices', module: 'financial' },
  { href: '/purchase/advance-invoices', label: 'Purchase Advance Invoices', module: 'financial' },
  { href: '/vendors', label: 'Vendors', module: 'financial' },
  { href: '/items', label: 'Items', module: 'warehouse' },
  { href: '/inventory', label: 'Inventory', module: 'warehouse' },
  { href: '/analytics', label: 'Analytics', module: 'financial' },
  { href: '/admin/users', label: 'User Management', module: 'admin' },
  { href: '/settings', label: 'Settings', module: 'admin' },
];

export function Sidebar() {
  const pathname = usePathname();
  const user = useAuthStore((s) => s.user);
  const clearAuth = useAuthStore((s) => s.clearAuth);

  const visibleItems = NAV_ITEMS.filter(
    (item) => !user || canAccessModule(item.module, user.role)
  );

  const handleLogout = () => {
    clearAuth();
    window.location.href = '/login';
  };

  return (
    <aside className="w-64 bg-pine-navy text-white h-screen flex flex-col">
      <div className="p-6 border-b border-white/10">
        <h1 className="text-xl font-bold">Pinoles</h1>
        {user && (
          <p className="text-xs text-pine-green mt-1 uppercase tracking-wide">
            {user.role}
          </p>
        )}
      </div>

      <nav className="flex-1 p-4 space-y-1">
        {visibleItems.map((item) => (
          <Link
            key={item.href}
            href={item.href}
            className={`block px-4 py-2.5 rounded-lg text-sm font-medium transition-colors ${
              pathname.startsWith(item.href)
                ? 'bg-white/10 text-white'
                : 'text-gray-300 hover:bg-white/10 hover:text-white'
            }`}
          >
            {item.label}
          </Link>
        ))}
      </nav>

      <div className="p-4 border-t border-white/10">
        {user && (
          <div className="mb-3 text-sm text-gray-300 truncate">{user.username}</div>
        )}
        <button
          onClick={handleLogout}
          className="w-full text-left text-sm text-gray-400 hover:text-white transition-colors px-4 py-2"
        >
          Logout
        </button>
      </div>
    </aside>
  );
}
