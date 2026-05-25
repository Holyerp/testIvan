import type { Metadata } from 'next';
import './globals.css';

export const metadata: Metadata = {
  title: 'Pinoles',
  description: 'Internal Business Portal',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="sr">
      <body>{children}</body>
    </html>
  );
}
