import { describe, it, expect } from 'vitest';
import { buildListQueryString } from '@/lib/query';

describe('buildListQueryString', () => {
  it('always emits page and pageSize', () => {
    expect(buildListQueryString({ page: 1, pageSize: 20 })).toBe('page=1&pageSize=20');
  });

  it('emits sortBy and sortDir together when sortBy is set', () => {
    expect(
      buildListQueryString({ page: 2, pageSize: 10, sortBy: 'displayName', sortDir: 'desc' })
    ).toBe('page=2&pageSize=10&sortBy=displayName&sortDir=desc');
  });

  it('defaults sortDir to asc when only sortBy is provided', () => {
    expect(buildListQueryString({ page: 1, pageSize: 20, sortBy: 'number' })).toBe(
      'page=1&pageSize=20&sortBy=number&sortDir=asc'
    );
  });

  it('omits empty and undefined filter params, encodes the rest', () => {
    const qs = buildListQueryString({
      page: 1,
      pageSize: 20,
      params: { search: 'Acme & Co', status: '', tab: undefined },
    });
    expect(qs).toBe('page=1&pageSize=20&search=Acme%20%26%20Co');
  });

  it('includes a non-empty filter param', () => {
    expect(
      buildListQueryString({ page: 3, pageSize: 5, params: { status: 'OPEN' } })
    ).toBe('page=3&pageSize=5&status=OPEN');
  });
});
