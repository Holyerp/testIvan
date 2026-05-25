import { describe, it, expect, vi, beforeEach } from 'vitest';
import { apiRequest, ApiError } from '@/lib/api/client';

describe('apiRequest', () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  it('returns data on successful response', async () => {
    global.fetch = vi.fn().mockResolvedValueOnce({
      ok: true,
      json: async () => ({ success: true, data: { id: '1', name: 'Test' } }),
    } as Response);

    const result = await apiRequest<{ id: string; name: string }>('/test');
    expect(result).toEqual({ id: '1', name: 'Test' });
  });

  it('throws ApiError on non-success response', async () => {
    global.fetch = vi.fn().mockResolvedValue({
      ok: false,
      status: 401,
      json: async () => ({ success: false, error: 'Unauthorized', code: 'AUTH_REQUIRED' }),
    } as Response);

    await expect(apiRequest('/test')).rejects.toThrow(ApiError);
    await expect(apiRequest('/test')).rejects.toMatchObject({
      code: 'AUTH_REQUIRED',
      status: 401,
    });
  });

  it('throws ApiError with INTERNAL_ERROR when code missing', async () => {
    global.fetch = vi.fn().mockResolvedValueOnce({
      ok: false,
      status: 500,
      json: async () => ({ success: false, error: 'Server error' }),
    } as Response);

    await expect(apiRequest('/test')).rejects.toMatchObject({
      code: 'INTERNAL_ERROR',
      status: 500,
    });
  });
});
