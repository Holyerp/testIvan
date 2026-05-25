// Admin user-management domain types + pure helpers (US-021), extracted so they can be
// unit-tested without rendering the page or mocking fetch. Mirrors the backend wire shape
// (UserListItemDto). `role` and `status` are SCREAMING_SNAKE_CASE cross-layer enums the UI
// maps to i18n labels.

import { z } from 'zod';

export type UserRole = 'ADMIN' | 'MANAGER' | 'ACCOUNTING' | 'WAREHOUSE';
export type UserStatus = 'ACTIVE' | 'INACTIVE';

export const USER_ROLES: UserRole[] = ['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE'];

export interface AdminUser {
  id: string;
  name: string;
  email: string | null;
  username: string;
  role: UserRole;
  status: UserStatus;
  lastLoginAt: string | null;
  createdAt: string;
}

/**
 * The status wire value for a boolean active flag. Pure — mirrors the backend
 * `UserStatus.From(bool)` so the create/edit form and the backend agree on the enum.
 */
export function statusFromActive(isActive: boolean): UserStatus {
  return isActive ? 'ACTIVE' : 'INACTIVE';
}

/**
 * Tailwind classes for the status badge, keyed by the SCREAMING_SNAKE wire value.
 * ACTIVE is green, INACTIVE is gray; unknown values default to gray. Pure — unit-tested.
 */
export function userStatusBadgeClass(status: string): string {
  return status === 'ACTIVE' ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600';
}

// i18n keys (under `userManagement.errors.*`) the page resolves for each backend error code.
const ERROR_CODE_KEYS: Record<string, string> = {
  CONFLICT_CANNOT_DELETE_SELF: 'cannotDeleteSelf',
  CONFLICT_LAST_ADMIN: 'cannotDeleteLastAdmin',
  CONFLICT_USERNAME_TAKEN: 'usernameTaken',
  CONFLICT_EMAIL_TAKEN: 'emailTaken',
  VALIDATION_INVALID_ROLE: 'invalidRole',
  VALIDATION_PASSWORD_TOO_SHORT: 'passwordTooShort',
  VALIDATION_REQUIRED_FIELDS: 'requiredFields',
  NOT_FOUND_USER: 'notFound',
};

/**
 * Map a backend error code to the i18n key under `userManagement.errors.*`. Unknown codes
 * fall back to `generic` so an unexpected code still renders a friendly message rather than
 * a raw code. Pure — unit-tested.
 */
export function errorMessageKey(code: string | undefined | null): string {
  if (!code) return 'generic';
  return ERROR_CODE_KEYS[code] ?? 'generic';
}

/**
 * Zod schema for the create-user form. Mirrors the backend validation: name + username
 * required, role from the enum, temp password ≥ 8 chars, email optional but valid when
 * present. Exported so the page binds it via @hookform/resolvers and tests exercise it.
 */
export const createUserSchema = z.object({
  name: z.string().min(1),
  email: z
    .string()
    .email()
    .optional()
    .or(z.literal('').transform(() => undefined)),
  username: z.string().min(1),
  role: z.enum(['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE']),
  tempPassword: z.string().min(8),
});

export type CreateUserFormData = z.infer<typeof createUserSchema>;

/**
 * Zod schema for the edit-user form (role + active status only). Pure schema; exported for
 * the page and tests.
 */
export const editUserSchema = z.object({
  role: z.enum(['ADMIN', 'MANAGER', 'ACCOUNTING', 'WAREHOUSE']),
  isActive: z.boolean(),
});

export type EditUserFormData = z.infer<typeof editUserSchema>;
