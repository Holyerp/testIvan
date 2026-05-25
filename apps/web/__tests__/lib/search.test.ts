import { describe, it, expect } from 'vitest';
import {
  flattenHits,
  hitHref,
  nextIndex,
  type SearchHit,
  type SearchResults,
} from '@/lib/search';

const customer: SearchHit = { id: 'c001', type: 'CUSTOMER', title: 'Acme', subtitle: 'C001' };
const vendor: SearchHit = { id: 'v001', type: 'VENDOR', title: 'Supplier A', subtitle: 'V001' };
const sales: SearchHit = { id: 'inv001', type: 'SALES_INVOICE', title: 'SI-001', subtitle: 'Acme' };
const purchase: SearchHit = { id: 'pinv001', type: 'PURCHASE_INVOICE', title: 'PI-001', subtitle: 'Supplier A' };

const results: SearchResults = {
  customers: [customer],
  vendors: [vendor],
  salesInvoices: [sales],
  purchaseInvoices: [purchase],
};

describe('flattenHits', () => {
  it('flattens groups in order: customers, vendors, sales, purchase', () => {
    const flat = flattenHits(results);
    expect(flat.map((h) => h.id)).toEqual(['c001', 'v001', 'inv001', 'pinv001']);
  });

  it('counts every hit across groups', () => {
    const multi: SearchResults = {
      customers: [customer, { ...customer, id: 'c002' }],
      vendors: [vendor],
      salesInvoices: [],
      purchaseInvoices: [purchase],
    };
    expect(flattenHits(multi)).toHaveLength(4);
  });

  it('returns an empty array for null/empty results', () => {
    expect(flattenHits(null)).toEqual([]);
    expect(
      flattenHits({ customers: [], vendors: [], salesInvoices: [], purchaseInvoices: [] })
    ).toEqual([]);
  });
});

describe('hitHref', () => {
  it('maps CUSTOMER to the customer detail route', () => {
    expect(hitHref(customer)).toBe('/customers/c001');
  });

  it('maps VENDOR to the vendor detail route', () => {
    expect(hitHref(vendor)).toBe('/vendors/v001');
  });

  it('maps SALES_INVOICE to the sales invoice detail route', () => {
    expect(hitHref(sales)).toBe('/sales/invoices/inv001');
  });

  it('maps PURCHASE_INVOICE to the purchase invoice detail route', () => {
    expect(hitHref(purchase)).toBe('/purchase/invoices/pinv001');
  });

  it('URL-encodes ids with reserved characters', () => {
    expect(hitHref({ ...customer, id: 'a b/c' })).toBe('/customers/a%20b%2Fc');
  });
});

describe('nextIndex', () => {
  it('moves down within range', () => {
    expect(nextIndex(0, 4, 1)).toBe(1);
  });

  it('wraps down past the end to the first item', () => {
    expect(nextIndex(3, 4, 1)).toBe(0);
  });

  it('wraps up past the start to the last item', () => {
    expect(nextIndex(0, 4, -1)).toBe(3);
  });

  it('moves up within range', () => {
    expect(nextIndex(2, 4, -1)).toBe(1);
  });

  it('treats -1 (nothing highlighted) + down as the first item', () => {
    expect(nextIndex(-1, 4, 1)).toBe(0);
  });

  it('returns 0 when there are no items', () => {
    expect(nextIndex(0, 0, 1)).toBe(0);
    expect(nextIndex(5, 0, -1)).toBe(0);
  });
});
