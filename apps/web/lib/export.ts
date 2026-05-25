// Client-side export utilities (T-005). Pure transforms are isolated from the
// library + DOM side effects so the data shaping (toRowMatrix / extractHeaders) can be
// unit-tested without xlsx/jsPDF or a real download. The wrappers (exportToExcel /
// exportToPdf) layer the library + browser download on top of those pure parts.

import * as XLSX from 'xlsx';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';

export interface ExportColumn<T> {
  header: string;
  value: (row: T) => string | number;
}

/**
 * The header labels for a column set, in order. Pure.
 */
export function extractHeaders<T>(columns: ExportColumn<T>[]): string[] {
  return columns.map((c) => c.header);
}

/**
 * The data cells for one row, in column order. Pure.
 */
export function rowToCells<T>(row: T, columns: ExportColumn<T>[]): (string | number)[] {
  return columns.map((c) => c.value(row));
}

/**
 * Build a 2D matrix: a header row followed by one row of cells per data row.
 * Pure and side-effect-free — the building block both export wrappers share.
 */
export function toRowMatrix<T>(rows: T[], columns: ExportColumn<T>[]): (string | number)[][] {
  return [extractHeaders(columns), ...rows.map((row) => rowToCells(row, columns))];
}

/**
 * Build an .xlsx workbook from the rows/columns and trigger a browser download named
 * `${filename}.xlsx`. The data shaping is delegated to the pure {@link toRowMatrix}.
 */
export function exportToExcel<T>(rows: T[], columns: ExportColumn<T>[], filename: string): void {
  const matrix = toRowMatrix(rows, columns);
  const worksheet = XLSX.utils.aoa_to_sheet(matrix);
  const workbook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
  XLSX.writeFile(workbook, `${filename}.xlsx`);
}

/**
 * Build a PDF with a title and a table from the rows/columns and trigger a browser
 * download named `${filename}.pdf`. Header/body shaping reuses the pure helpers.
 */
export function exportToPdf<T>(
  rows: T[],
  columns: ExportColumn<T>[],
  filename: string,
  title: string
): void {
  const doc = new jsPDF();
  doc.text(title, 14, 18);
  autoTable(doc, {
    head: [extractHeaders(columns)],
    body: rows.map((row) => rowToCells(row, columns)),
    startY: 24,
  });
  doc.save(`${filename}.pdf`);
}
