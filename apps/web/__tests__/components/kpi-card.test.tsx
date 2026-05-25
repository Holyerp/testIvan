import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';

function KpiCard({
  label,
  value,
  subtitle,
}: {
  label: string;
  value: string;
  subtitle?: string;
}) {
  return (
    <div>
      <p>{label}</p>
      <p>{value}</p>
      {subtitle && <p>{subtitle}</p>}
    </div>
  );
}

describe('KpiCard', () => {
  it('renders label and value', () => {
    render(<KpiCard label="Total Customers" value="42" />);
    expect(screen.getByText('Total Customers')).toBeDefined();
    expect(screen.getByText('42')).toBeDefined();
  });

  it('renders subtitle when provided', () => {
    render(<KpiCard label="Invoices" value="1000" subtitle="total amount" />);
    expect(screen.getByText('total amount')).toBeDefined();
  });

  it('omits subtitle when not provided', () => {
    render(<KpiCard label="Customers" value="5" />);
    expect(screen.queryByText('total amount')).toBeNull();
  });
});
