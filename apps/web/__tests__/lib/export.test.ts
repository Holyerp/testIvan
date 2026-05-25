import { describe, it, expect, vi, afterEach } from 'vitest';

// jspdf-autotable mutates the jsPDF instance; in jsdom we mock it to a no-op so the
// wrapper can be exercised against a stubbed jsPDF without the plugin internals.
vi.mock('jspdf-autotable', () => ({ default: vi.fn() }));

import {
  extractHeaders,
  rowToCells,
  toRowMatrix,
  exportToExcel,
  exportToPdf,
  type ExportColumn,
} from '@/lib/export';

interface Row {
  number: string;
  amount: number;
  customer: string;
}

const columns: ExportColumn<Row>[] = [
  { header: 'Number', value: (r) => r.number },
  { header: 'Amount', value: (r) => r.amount },
  { header: 'Customer', value: (r) => r.customer },
];

const rows: Row[] = [
  { number: 'SI-001', amount: 45000, customer: 'Acme' },
  { number: 'SI-002', amount: 78000, customer: 'Delta' },
];

describe('extractHeaders', () => {
  it('returns header labels in column order', () => {
    expect(extractHeaders(columns)).toEqual(['Number', 'Amount', 'Customer']);
  });

  it('returns an empty array for no columns', () => {
    expect(extractHeaders<Row>([])).toEqual([]);
  });
});

describe('rowToCells', () => {
  it('maps a row to cells in column order', () => {
    expect(rowToCells(rows[0], columns)).toEqual(['SI-001', 45000, 'Acme']);
  });

  it('preserves numeric cell values (not stringified)', () => {
    const cells = rowToCells(rows[1], columns);
    expect(cells[1]).toBe(78000);
    expect(typeof cells[1]).toBe('number');
  });
});

describe('toRowMatrix', () => {
  it('produces a header row followed by one row per data row', () => {
    const matrix = toRowMatrix(rows, columns);
    expect(matrix).toHaveLength(3);
    expect(matrix[0]).toEqual(['Number', 'Amount', 'Customer']);
    expect(matrix[1]).toEqual(['SI-001', 45000, 'Acme']);
    expect(matrix[2]).toEqual(['SI-002', 78000, 'Delta']);
  });

  it('returns only the header row when there are no data rows', () => {
    const matrix = toRowMatrix<Row>([], columns);
    expect(matrix).toEqual([['Number', 'Amount', 'Customer']]);
  });

  it('handles a single column', () => {
    const single: ExportColumn<Row>[] = [{ header: 'No', value: (r) => r.number }];
    expect(toRowMatrix(rows, single)).toEqual([['No'], ['SI-001'], ['SI-002']]);
  });

  it('applies the column value transform (e.g. derived/formatted cells)', () => {
    const derived: ExportColumn<Row>[] = [
      { header: 'Label', value: (r) => `${r.number} (${r.customer})` },
    ];
    expect(toRowMatrix(rows, derived)).toEqual([
      ['Label'],
      ['SI-001 (Acme)'],
      ['SI-002 (Delta)'],
    ]);
  });
});

describe('exportToExcel', () => {
  afterEach(() => vi.restoreAllMocks());

  it('does not throw and writes a file named with the .xlsx extension', async () => {
    const xlsx = await import('xlsx');
    const writeFile = vi.spyOn(xlsx, 'writeFile').mockImplementation(() => undefined);

    expect(() => exportToExcel(rows, columns, 'invoices')).not.toThrow();
    expect(writeFile).toHaveBeenCalledTimes(1);
    // XLSX.writeFile(workbook, filename) — filename is the second argument.
    expect(writeFile.mock.calls[0][1]).toBe('invoices.xlsx');
  });
});

describe('exportToPdf', () => {
  afterEach(() => vi.restoreAllMocks());

  it('does not throw and saves a file named with the .pdf extension', async () => {
    const jspdfModule = await import('jspdf');
    const saveSpy = vi.fn();
    vi.spyOn(jspdfModule, 'jsPDF').mockImplementation(
      () =>
        ({
          text: vi.fn(),
          save: saveSpy,
        }) as unknown as InstanceType<typeof jspdfModule.jsPDF>
    );

    expect(() => exportToPdf(rows, columns, 'invoices', 'Invoices')).not.toThrow();
    expect(saveSpy).toHaveBeenCalledWith('invoices.pdf');
  });
});
