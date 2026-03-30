# Internationalization (i18n) Rules

> **Note:** This file contains i18n-specific rules. Include these rules ONLY if your project requires multiple languages. If not needed, this file can be ignored or deleted.

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

- ✅ **English** (default) - `en.json` or `en.ts`
- ✅ **{{LANGUAGE_2}}** - `{{CODE_2}}.json` (e.g., `de.json` for German)
- ✅ **{{LANGUAGE_3}}** - `{{CODE_3}}.json` (e.g., `fr.json` for French)
- ✅ **{{LANGUAGE_4}}** - `{{CODE_4}}.json` (e.g., `es.json` for Spanish)

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
- `en-US` - English (United States)
- `en-GB` - English (United Kingdom)
- `pt-BR` - Portuguese (Brazil)
- `pt-PT` - Portuguese (Portugal)
- `zh-CN` - Chinese (Simplified, China)
- `zh-TW` - Chinese (Traditional, Taiwan)

---

## Translation System

**Technology:** {{I18N_LIBRARY}} (e.g., `react-i18next`, `next-intl`, `vue-i18n`, `i18next`)

**Translation files location:** `{{TRANSLATION_PATH}}` (e.g., `/app/locales/`, `/public/locales/`, `/src/i18n/`)

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

**Alternative structures:**

Next.js with next-intl:
```
messages/
├── en.json
├── de.json
└── fr.json
```

Remix/React Router 7:
```
app/
└── locales/
    ├── en.json
    ├── de.json
    └── fr.json
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

## Example Implementation

### React with react-i18next:

```typescript
import { useTranslation } from 'react-i18next';

function LoginForm() {
  const { t } = useTranslation();

  return (
    <form>
      <label>{t('auth.login.emailLabel')}</label>
      <input placeholder={t('auth.login.emailPlaceholder')} />
      <button>{t('auth.login.submitButton')}</button>
    </form>
  );
}
```

**Translation file (en.json):**
```json
{
  "auth": {
    "login": {
      "emailLabel": "Email Address",
      "emailPlaceholder": "Enter your email",
      "submitButton": "Sign In",
      "errors": {
        "invalidCredentials": "Invalid email or password"
      }
    }
  }
}
```

### Vue with vue-i18n:

```vue
<template>
  <form>
    <label>{{ $t('auth.login.emailLabel') }}</label>
    <input :placeholder="$t('auth.login.emailPlaceholder')" />
    <button>{{ $t('auth.login.submitButton') }}</button>
  </form>
</template>
```

---

## Validation Errors

**All validation errors MUST be translatable:**

```typescript
// ✅ GOOD: Translatable
const schema = z.object({
  email: z.string().email({ message: t('validation.email.invalid') }),
  password: z.string().min(8, { message: t('validation.password.tooShort') }),
});

// ❌ BAD: Hardcoded
const schema = z.object({
  email: z.string().email({ message: 'Invalid email' }),
});
```

**Common validation messages:**
```json
{
  "validation": {
    "required": "This field is required",
    "email": {
      "invalid": "Invalid email address",
      "required": "Email is required"
    },
    "password": {
      "tooShort": "Password must be at least 8 characters",
      "required": "Password is required"
    }
  }
}
```

---

## Language Switcher

**If required, implement language switcher:**
- Display current language flag/name
- Allow user to select from available languages
- Store preference in localStorage/cookies
- Apply immediately without page reload

**Example implementation:**
```typescript
import { useTranslation } from 'react-i18next';

function LanguageSwitcher() {
  const { i18n } = useTranslation();

  const changeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
    localStorage.setItem('language', lng);
  };

  return (
    <select value={i18n.language} onChange={(e) => changeLanguage(e.target.value)}>
      <option value="en">English</option>
      <option value="de">Deutsch</option>
      <option value="fr">Français</option>
    </select>
  );
}
```

---

## Testing i18n

### Manual testing:
1. Switch to each language
2. Navigate through all pages
3. Trigger all error messages
4. Test form validation messages
5. Check date/number formatting

### Automated testing:
```typescript
describe('Translations', () => {
  it('should have all required translation keys', () => {
    const en = require('./locales/en.json');
    const de = require('./locales/de.json');

    expect(Object.keys(en)).toEqual(Object.keys(de));
  });

  it('should not have missing translations', () => {
    const en = require('./locales/en.json');
    const de = require('./locales/de.json');

    function getAllKeys(obj: any, prefix = ''): string[] {
      return Object.keys(obj).flatMap(key => {
        const value = obj[key];
        const newKey = prefix ? `${prefix}.${key}` : key;
        return typeof value === 'object' ? getAllKeys(value, newKey) : [newKey];
      });
    }

    const enKeys = getAllKeys(en);
    const deKeys = getAllKeys(de);

    expect(enKeys.sort()).toEqual(deKeys.sort());
  });
});
```

---

## RTL Support (Right-to-Left)

**If supporting Arabic, Hebrew, or other RTL languages:**

```css
/* Add direction support */
[dir="rtl"] {
  direction: rtl;
  text-align: right;
}

[dir="ltr"] {
  direction: ltr;
  text-align: left;
}
```

```typescript
// Set direction based on language
useEffect(() => {
  const dir = ['ar', 'he'].includes(i18n.language) ? 'rtl' : 'ltr';
  document.documentElement.setAttribute('dir', dir);
}, [i18n.language]);
```

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

## Resources

- **React i18n guide:** https://react.i18next.com/
- **Next.js i18n:** https://next-intl-docs.vercel.app/
- **Vue i18n:** https://vue-i18n.intlify.dev/
- **i18next docs:** https://www.i18next.com/

---

**Related Files:**
- Setup guide: `.project-management/rules/I18N-SETUP.md`
- Quality gates: `.CLAUDE.MD` (checks for translations if i18n enabled)
- Project rules: `.project-management/rules/project-rules.md`

---

**Last Updated:** 2026-03-26
