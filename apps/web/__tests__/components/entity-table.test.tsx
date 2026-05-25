import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { EntityTable, type Column } from '@/components/entity-table';

interface Row {
  id: string;
  name: string;
  amount: number;
}

const columns: Column<Row>[] = [
  { key: 'name', header: 'Name', sortable: true, render: (r) => r.name },
  { key: 'amount', header: 'Amount', align: 'right', render: (r) => r.amount },
];

const rows: Row[] = [
  { id: '1', name: 'Acme', amount: 100 },
  { id: '2', name: 'Delta', amount: 200 },
];

const labels = { previous: 'Previous', next: 'Next', pageOf: 'Page {page} of {total}', loadError: 'Load error' };

function renderTable(props: Partial<React.ComponentProps<typeof EntityTable<Row>>> = {}) {
  return render(
    <EntityTable<Row>
      columns={columns}
      rows={rows}
      rowKey={(r) => r.id}
      emptyMessage="No rows"
      page={1}
      totalPages={3}
      onPageChange={vi.fn()}
      labels={labels}
      {...props}
    />
  );
}

describe('EntityTable', () => {
  it('renders rows', () => {
    renderTable();
    expect(screen.getByText('Acme')).toBeDefined();
    expect(screen.getByText('Delta')).toBeDefined();
  });

  it('shows empty message when there are no rows', () => {
    renderTable({ rows: [] });
    expect(screen.getByText('No rows')).toBeDefined();
  });

  it('shows skeleton (no data) when loading', () => {
    renderTable({ isLoading: true });
    expect(screen.queryByText('Acme')).toBeNull();
  });

  it('shows error banner when error is set', () => {
    renderTable({ error: 'INTERNAL_ERROR' });
    expect(screen.getByText('Load error')).toBeDefined();
    expect(screen.queryByText('Acme')).toBeNull();
  });

  it('calls onSort when a sortable header is clicked', () => {
    const onSort = vi.fn();
    renderTable({ onSort });
    fireEvent.click(screen.getByText('Name'));
    expect(onSort).toHaveBeenCalledWith('name');
  });

  it('interpolates page and total into the pageOf label', () => {
    renderTable({ page: 2, totalPages: 5 });
    expect(screen.getByText('Page 2 of 5')).toBeDefined();
  });

  it('disables Previous on page 1', () => {
    renderTable({ page: 1, totalPages: 3 });
    expect((screen.getByText('Previous') as HTMLButtonElement).disabled).toBe(true);
  });

  it('disables Next on the last page', () => {
    renderTable({ page: 3, totalPages: 3 });
    expect((screen.getByText('Next') as HTMLButtonElement).disabled).toBe(true);
  });

  it('calls onRowClick when a row is clicked', () => {
    const onRowClick = vi.fn();
    renderTable({ onRowClick });
    fireEvent.click(screen.getByText('Acme'));
    expect(onRowClick).toHaveBeenCalledWith(rows[0]);
  });
});
