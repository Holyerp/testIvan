import { describe, it, expect } from 'vitest';
import type { UserRole } from '@/types';

describe('UserRole type', () => {
  it('accepts valid roles', () => {
    const roles: UserRole[] = ['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE'];
    expect(roles).toHaveLength(4);
    roles.forEach(role => expect(typeof role).toBe('string'));
  });
});
