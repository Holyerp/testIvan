# Internationalization (i18n) Rules

> **Note:** This file contains i18n-specific rules. Include these rules ONLY if your project requires multiple languages. If not needed, this file can be ignored or deleted.

**📖 Detailed Implementation:** See [modules/i18n-implementation.md](modules/i18n-implementation.md) for code examples, testing, and RTL support.

---

## When to Use This File

**Enable i18n if your project:**
- ✅ Targets users in multiple countries
- ✅ Requires multiple language support
- ✅ Has international clients/customers
- ✅ Needs localization (dates, currencies, number formats)

**Skip i18n if your project:**
- ❌ Only serves a single-language market
- ❌ Is an internal tool for one team
- ❌ Has no internationalization requirements

**If you don't need i18n:** Simply delete or ignore this file. Claude will not enforce translation requirements.

---

## Languages Supported

**MANDATORY**: All user-facing features MUST include translations for:

- ✅ **Serbian** (default / primary) - `sr.json`
- ✅ **English** (secondary) - `en.json`

**Common language codes:**
- `de` - German (Deutsch)
- `fr` - French (Français)
- `es` - Spanish (Español)
- `it` - Italian (Italiano)
- `pt` - Portuguese (Português)
- `nl` - Dutch (Nederlands)
- `pl` - Polish (Polski)
- `ru` - Russian (Русский)
- `ja` - Japanese (日本語)
- `zh` - Chinese (中文)

**Regional variants:**
- `en-US` / `en-GB` (English variants)
- `pt-BR` / `pt-PT` (Portuguese variants)
- `zh-CN` / `zh-TW` (Chinese variants)

---

## Translation System

**Technology:** `next-intl` (Next.js 14 App Router compatible)

**Translation files location:** `apps/web/messages/` — one JSON file per locale

**Popular i18n libraries:**
- React: `react-i18next`, `react-intl`, `next-intl` (Next.js)
- Vue: `vue-i18n`
- Angular: `@angular/localize`, `ngx-translate`
- Vanilla JS: `i18next`

**File structure example:**
```
locales/
├── en.json          # English (default)
├── de.json          # German
├── fr.json          # French
└── common/
    ├── errors.json  # Common error messages
    └── forms.json   # Common form labels
```

---

## What Must Be Translated

**ALL user-facing text:**
- ✅ UI labels and buttons
- ✅ Form labels and placeholders
- ✅ Form validation messages
- ✅ Error messages
- ✅ Success notifications
- ✅ Toast/alert messages
- ✅ Page titles and descriptions (SEO)
- ✅ Email templates (if applicable)
- ✅ Push notifications (if applicable)

**Do NOT translate:**
- ❌ Code identifiers (variable names, function names)
- ❌ Database field names
- ❌ API endpoint paths
- ❌ Log messages (for developers)
- ❌ Technical error codes

---

## Translation Keys Convention

**Use namespaced keys:**
```typescript
// ✅ GOOD: Namespaced, descriptive
t('auth.login.button')
t('auth.errors.invalidCredentials')
t('products.list.title')
t('products.form.nameLabel')

// ❌ BAD: Flat, ambiguous
t('login')
t('error')
t('title')
```

**Naming patterns:**
- Use dot notation for namespacing
- Group by feature/module (e.g., `auth.*`, `products.*`)
- Use camelCase for key names
- Be descriptive and specific

---

## Translation File Format

### Flat Structure (Simple projects):

```json
{
  "welcome": "Welcome",
  "loginButton": "Sign In",
  "logoutButton": "Sign Out"
}
```

### Namespaced Structure (Recommended):

```json
{
  "auth": {
    "login": {
      "title": "Sign In",
      "emailLabel": "Email Address",
      "passwordLabel": "Password",
      "submitButton": "Sign In",
      "forgotPassword": "Forgot password?"
    },
    "register": {
      "title": "Create Account",
      "submitButton": "Sign Up"
    }
  },
  "errors": {
    "required": "This field is required",
    "invalidEmail": "Invalid email address"
  }
}
```

**For detailed examples:** See [modules/i18n-implementation.md](modules/i18n-implementation.md)

---

## Task Completion Criteria

**Do NOT count any task as finished until translations are implemented!**

Before marking task complete:
- [ ] All UI text uses translation keys (no hardcoded strings)
- [ ] English translations added
- [ ] All additional language translations added
- [ ] Translation keys are descriptive and namespaced
- [ ] Tested in all supported languages
- [ ] RTL support implemented (if applicable for Arabic/Hebrew)

---

## Best Practices

1. **Always use translation keys** - Never hardcode user-facing text
2. **Keep keys organized** - Use namespacing by feature/module
3. **Provide context** - Use descriptive key names (not just `button1`, `button2`)
4. **Test all languages** - Don't assume translations work without testing
5. **Handle plurals** - Use i18n library plural support for dynamic counts
6. **Format dates/numbers** - Use library formatting for locale-specific formats
7. **Keep translations in sync** - Ensure all languages have the same keys
8. **Document custom keys** - If using custom translation patterns, document them

---

## Implementation Guides

| Topic | Documentation |
|-------|---------------|
| **Detailed examples** | [modules/i18n-implementation.md](modules/i18n-implementation.md) |
| **Validation errors** | [modules/i18n-implementation.md#validation-errors](modules/i18n-implementation.md) |
| **Language switcher** | [modules/i18n-implementation.md#language-switcher](modules/i18n-implementation.md) |
| **Testing i18n** | [modules/i18n-implementation.md#testing-i18n](modules/i18n-implementation.md) |
| **RTL support** | [modules/i18n-implementation.md#rtl-support](modules/i18n-implementation.md) |

---

## Resources

- **React i18n guide:** https://react.i18next.com/
- **Next.js i18n:** https://next-intl-docs.vercel.app/
- **Vue i18n:** https://vue-i18n.intlify.dev/
- **i18next docs:** https://www.i18next.com/

---

**Related Files:**
- Setup guide: `.project-management/rules/I18N-SETUP.md`
- Quality gates: `CLAUDE.md` (checks for translations if i18n enabled)
- Project rules: `.project-management/rules/project-rules.md`

---

**Last Updated:** 2026-03-26
