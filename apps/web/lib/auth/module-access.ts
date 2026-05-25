export type Module = 'dashboard' | 'financial' | 'warehouse' | 'admin';

export const MODULE_ACCESS: Record<Module, string[]> = {
  dashboard: ['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE'],
  financial: ['ADMIN', 'MANAGER', 'ACCOUNTING'],
  warehouse: ['ADMIN', 'MANAGER', 'WAREHOUSE'],
  admin: ['ADMIN'],
};

export function canAccessModule(module: Module, role: string | undefined | null): boolean {
  if (!role) return false;
  return MODULE_ACCESS[module].includes(role);
}
