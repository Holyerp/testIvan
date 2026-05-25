import { describe, it, expect } from 'vitest';
import {
  statusFromActive,
  userStatusBadgeClass,
  errorMessageKey,
  createUserSchema,
  editUserSchema,
  USER_ROLES,
} from '@/lib/admin/users';

describe('statusFromActive', () => {
  it('maps true to ACTIVE', () => {
    expect(statusFromActive(true)).toBe('ACTIVE');
  });
  it('maps false to INACTIVE', () => {
    expect(statusFromActive(false)).toBe('INACTIVE');
  });
});

describe('userStatusBadgeClass', () => {
  it('greens an active status', () => {
    expect(userStatusBadgeClass('ACTIVE')).toContain('green');
  });
  it('grays an inactive status', () => {
    expect(userStatusBadgeClass('INACTIVE')).toContain('gray');
  });
  it('defaults unknown values to gray', () => {
    expect(userStatusBadgeClass('SOMETHING_ELSE')).toContain('gray');
  });
});

describe('errorMessageKey', () => {
  it('maps the self-delete guard', () => {
    expect(errorMessageKey('CONFLICT_CANNOT_DELETE_SELF')).toBe('cannotDeleteSelf');
  });
  it('maps the last-admin guard', () => {
    expect(errorMessageKey('CONFLICT_LAST_ADMIN')).toBe('cannotDeleteLastAdmin');
  });
  it('maps the username conflict', () => {
    expect(errorMessageKey('CONFLICT_USERNAME_TAKEN')).toBe('usernameTaken');
  });
  it('falls back to generic for unknown codes', () => {
    expect(errorMessageKey('SOME_UNKNOWN_CODE')).toBe('generic');
  });
  it('falls back to generic for null/undefined', () => {
    expect(errorMessageKey(null)).toBe('generic');
    expect(errorMessageKey(undefined)).toBe('generic');
  });
});

describe('createUserSchema', () => {
  it('accepts a valid payload', () => {
    const result = createUserSchema.safeParse({
      name: 'Jane Doe',
      email: 'jane@pinoles.local',
      username: 'jane',
      role: 'MANAGER',
      tempPassword: 'TempPass123!',
    });
    expect(result.success).toBe(true);
  });
  it('accepts an empty email (optional)', () => {
    const result = createUserSchema.safeParse({
      name: 'Jane Doe',
      email: '',
      username: 'jane',
      role: 'MANAGER',
      tempPassword: 'TempPass123!',
    });
    expect(result.success).toBe(true);
    if (result.success) expect(result.data.email).toBeUndefined();
  });
  it('rejects a short password', () => {
    const result = createUserSchema.safeParse({
      name: 'Jane Doe',
      username: 'jane',
      role: 'MANAGER',
      tempPassword: 'short',
    });
    expect(result.success).toBe(false);
  });
  it('rejects an invalid role', () => {
    const result = createUserSchema.safeParse({
      name: 'Jane Doe',
      username: 'jane',
      role: 'SUPERUSER',
      tempPassword: 'TempPass123!',
    });
    expect(result.success).toBe(false);
  });
  it('rejects a missing name', () => {
    const result = createUserSchema.safeParse({
      name: '',
      username: 'jane',
      role: 'MANAGER',
      tempPassword: 'TempPass123!',
    });
    expect(result.success).toBe(false);
  });
});

describe('editUserSchema', () => {
  it('accepts a valid edit payload', () => {
    expect(editUserSchema.safeParse({ role: 'ADMIN', isActive: true }).success).toBe(true);
  });
  it('rejects an invalid role', () => {
    expect(editUserSchema.safeParse({ role: 'NOPE', isActive: true }).success).toBe(false);
  });
});

describe('USER_ROLES', () => {
  it('lists the four roles', () => {
    expect(USER_ROLES).toEqual(['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE']);
  });
});
