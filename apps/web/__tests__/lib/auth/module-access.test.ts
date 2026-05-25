import { describe, it, expect } from 'vitest';
import { canAccessModule, MODULE_ACCESS, type Module } from '@/lib/auth/module-access';

describe('canAccessModule', () => {
  it('dashboard is open to all four roles', () => {
    expect(canAccessModule('dashboard', 'ADMIN')).toBe(true);
    expect(canAccessModule('dashboard', 'MANAGER')).toBe(true);
    expect(canAccessModule('dashboard', 'ACCOUNTING')).toBe(true);
    expect(canAccessModule('dashboard', 'WAREHOUSE')).toBe(true);
  });

  it('financial excludes WAREHOUSE', () => {
    expect(canAccessModule('financial', 'ADMIN')).toBe(true);
    expect(canAccessModule('financial', 'MANAGER')).toBe(true);
    expect(canAccessModule('financial', 'ACCOUNTING')).toBe(true);
    expect(canAccessModule('financial', 'WAREHOUSE')).toBe(false);
  });

  it('warehouse excludes ACCOUNTING', () => {
    expect(canAccessModule('warehouse', 'ADMIN')).toBe(true);
    expect(canAccessModule('warehouse', 'MANAGER')).toBe(true);
    expect(canAccessModule('warehouse', 'WAREHOUSE')).toBe(true);
    expect(canAccessModule('warehouse', 'ACCOUNTING')).toBe(false);
  });

  it('admin is restricted to ADMIN only', () => {
    expect(canAccessModule('admin', 'ADMIN')).toBe(true);
    expect(canAccessModule('admin', 'MANAGER')).toBe(false);
    expect(canAccessModule('admin', 'ACCOUNTING')).toBe(false);
    expect(canAccessModule('admin', 'WAREHOUSE')).toBe(false);
  });

  it('returns false for null, undefined, or unknown role', () => {
    expect(canAccessModule('dashboard', null)).toBe(false);
    expect(canAccessModule('dashboard', undefined)).toBe(false);
    expect(canAccessModule('financial', 'SUPERUSER')).toBe(false);
    expect(canAccessModule('warehouse', '')).toBe(false);
  });

  it('exposes a map with all four modules', () => {
    const modules: Module[] = ['dashboard', 'financial', 'warehouse', 'admin'];
    modules.forEach((m) => expect(MODULE_ACCESS[m]).toBeDefined());
  });
});
