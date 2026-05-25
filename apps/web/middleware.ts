import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

/**
 * Auth middleware — coarse server-side guard.
 * sessionStorage (client-only) cannot be read server-side, so the
 * primary auth redirect is handled by the useRequireAuth hook on protected
 * pages. This middleware provides structural route grouping and can be
 * extended to validate a server-side cookie in the future.
 */
export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;

  // Public paths — always allow
  if (pathname.startsWith('/login') || pathname.startsWith('/403')) {
    return NextResponse.next();
  }

  return NextResponse.next();
}

export const config = {
  matcher: ['/((?!api|_next/static|_next/image|favicon.ico).*)'],
};
