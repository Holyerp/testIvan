import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, act } from '@testing-library/react';
import { FilterPanel, type FilterField } from '@/components/filter-panel';

describe('FilterPanel', () => {
  afterEach(() => {
    vi.useRealTimers();
  });

  describe('search field (debounced)', () => {
    beforeEach(() => {
      vi.useFakeTimers();
    });

    it('fires onChange ~400ms after typing', () => {
      const onChange = vi.fn();
      const fields: FilterField[] = [
        { type: 'search', key: 'q', placeholder: 'Search', value: '' },
      ];
      render(<FilterPanel fields={fields} onChange={onChange} />);

      // Initial mount fires once with the empty value.
      act(() => vi.advanceTimersByTime(400));
      onChange.mockClear();

      fireEvent.change(screen.getByLabelText('Search'), { target: { value: 'abc' } });
      expect(onChange).not.toHaveBeenCalled(); // debounced, not yet

      act(() => vi.advanceTimersByTime(400));
      expect(onChange).toHaveBeenCalledWith('q', 'abc');
    });
  });

  describe('non-debounced fields', () => {
    it('select fires onChange immediately with the selected value', () => {
      const onChange = vi.fn();
      const fields: FilterField[] = [
        {
          type: 'select',
          key: 'status',
          label: 'Status',
          value: 'ALL',
          options: [
            { value: 'ALL', label: 'All' },
            { value: 'OPEN', label: 'Open' },
          ],
        },
      ];
      render(<FilterPanel fields={fields} onChange={onChange} />);

      fireEvent.change(screen.getByLabelText('Status'), { target: { value: 'OPEN' } });
      expect(onChange).toHaveBeenCalledWith('status', 'OPEN');
    });

    it('dateRange fires onChange with from/to immediately', () => {
      const onChange = vi.fn();
      const fields: FilterField[] = [
        { type: 'dateRange', key: 'date', label: 'Date', from: '', to: '' },
      ];
      render(<FilterPanel fields={fields} onChange={onChange} />);

      fireEvent.change(screen.getByLabelText('Date from'), { target: { value: '2026-01-01' } });
      expect(onChange).toHaveBeenCalledWith('date', { from: '2026-01-01', to: '' });

      fireEvent.change(screen.getByLabelText('Date to'), { target: { value: '2026-02-01' } });
      expect(onChange).toHaveBeenCalledWith('date', { from: '', to: '2026-02-01' });
    });

    it('amountRange fires onChange with min/max immediately', () => {
      const onChange = vi.fn();
      const fields: FilterField[] = [
        { type: 'amountRange', key: 'amount', label: 'Amount', min: '', max: '' },
      ];
      render(<FilterPanel fields={fields} onChange={onChange} />);

      fireEvent.change(screen.getByLabelText('Amount min'), { target: { value: '10' } });
      expect(onChange).toHaveBeenCalledWith('amount', { min: '10', max: '' });

      fireEvent.change(screen.getByLabelText('Amount max'), { target: { value: '99' } });
      expect(onChange).toHaveBeenCalledWith('amount', { min: '', max: '99' });
    });
  });

  it('renders multiple field types together', () => {
    const fields: FilterField[] = [
      { type: 'search', key: 'q', placeholder: 'Search', value: '' },
      {
        type: 'select',
        key: 'status',
        label: 'Status',
        value: 'ALL',
        options: [{ value: 'ALL', label: 'All' }],
      },
      { type: 'amountRange', key: 'amount', label: 'Amount', min: '', max: '' },
    ];
    render(<FilterPanel fields={fields} onChange={vi.fn()} />);

    expect(screen.getByLabelText('Search')).toBeDefined();
    expect(screen.getByLabelText('Status')).toBeDefined();
    expect(screen.getByLabelText('Amount min')).toBeDefined();
  });
});
