'use client';

import Link from 'next/link';

export default function ForbiddenPage() {
  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
      <div className="text-center max-w-md">
        <div className="text-6xl font-bold text-pine-navy mb-4">403</div>
        <h1 className="text-2xl font-bold text-gray-900 mb-2">Access Forbidden</h1>
        <p className="text-gray-500 mb-8">
          You don&apos;t have permission to access this page.
        </p>
        <Link
          href="/dashboard"
          className="bg-pine-navy text-white px-6 py-2.5 rounded-lg hover:opacity-90 transition-opacity inline-block"
        >
          Back to Dashboard
        </Link>
      </div>
    </div>
  );
}
