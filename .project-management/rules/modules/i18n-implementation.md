# i18n Implementation Guide

**Purpose:** Detailed implementation patterns, examples, and testing for internationalization.

**Parent:** `.project-management/rules/I18N-RULES.md`

---

## Example Implementation

### React with react-i18next

**Setup:**
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

### Vue with vue-i18n

**Setup:**
```vue
<template>
  <form>
    <label>{{ $t('auth.login.emailLabel') }}</label>
    <input :placeholder="$t('auth.login.emailPlaceholder')" />
    <button>{{ $t('auth.login.submitButton') }}</button>
  </form>
</template>
```

**Translation file structure (same as React example above)**

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
    },
    "username": {
      "tooShort": "Username must be at least 3 characters",
      "tooLong": "Username must be less than 20 characters",
      "invalid": "Username can only contain letters, numbers, and underscores"
    },
    "phone": {
      "invalid": "Invalid phone number format"
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

**With language flags:**
```typescript
const languages = [
  { code: 'en', name: 'English', flag: '🇬🇧' },
  { code: 'de', name: 'Deutsch', flag: '🇩🇪' },
  { code: 'fr', name: 'Français', flag: '🇫🇷' },
  { code: 'es', name: 'Español', flag: '🇪🇸' },
];

function LanguageSwitcher() {
  const { i18n } = useTranslation();

  return (
    <div className="language-switcher">
      {languages.map(lang => (
        <button
          key={lang.code}
          onClick={() => i18n.changeLanguage(lang.code)}
          className={i18n.language === lang.code ? 'active' : ''}
        >
          <span>{lang.flag}</span>
          <span>{lang.name}</span>
        </button>
      ))}
    </div>
  );
}
```

---

## Testing i18n

### Manual Testing Checklist

1. **Switch to each language** - Test all supported languages
2. **Navigate through all pages** - Ensure all text is translated
3. **Trigger all error messages** - Check error translations
4. **Test form validation messages** - Verify validation translations
5. **Check date/number formatting** - Ensure locale-specific formats work
6. **Test dynamic content** - Check plurals, interpolations work
7. **Test RTL languages** - If supporting Arabic/Hebrew, test layout

### Automated Testing

**Test translation key completeness:**
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

  it('should not have empty translation values', () => {
    const en = require('./locales/en.json');

    function getAllValues(obj: any): string[] {
      return Object.keys(obj).flatMap(key => {
        const value = obj[key];
        return typeof value === 'object' ? getAllValues(value) : [value];
      });
    }

    const values = getAllValues(en);
    const emptyValues = values.filter(v => !v || v.trim() === '');

    expect(emptyValues).toHaveLength(0);
  });
});
```

**Test component translations:**
```typescript
import { render, screen } from '@testing-library/react';
import { I18nextProvider } from 'react-i18next';
import i18n from '../i18n'; // Your i18n config

describe('LoginForm', () => {
  it('should render in English', async () => {
    await i18n.changeLanguage('en');
    render(
      <I18nextProvider i18n={i18n}>
        <LoginForm />
      </I18nextProvider>
    );

    expect(screen.getByText('Email Address')).toBeInTheDocument();
  });

  it('should render in German', async () => {
    await i18n.changeLanguage('de');
    render(
      <I18nextProvider i18n={i18n}>
        <LoginForm />
      </I18nextProvider>
    );

    expect(screen.getByText('E-Mail-Adresse')).toBeInTheDocument();
  });
});
```

---

## RTL Support (Right-to-Left)

**If supporting Arabic, Hebrew, or other RTL languages:**

### CSS Setup

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

/* Adjust padding/margin for RTL */
[dir="rtl"] .sidebar {
  padding-right: 20px;
  padding-left: 0;
}

[dir="ltr"] .sidebar {
  padding-left: 20px;
  padding-right: 0;
}

/* Use logical properties (modern approach) */
.element {
  margin-inline-start: 10px; /* Left in LTR, right in RTL */
  margin-inline-end: 20px;   /* Right in LTR, left in RTL */
  padding-inline: 15px;       /* Padding on both sides */
}
```

### JavaScript Setup

```typescript
import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';

function App() {
  const { i18n } = useTranslation();

  useEffect(() => {
    // Set direction based on language
    const dir = ['ar', 'he', 'fa', 'ur'].includes(i18n.language) ? 'rtl' : 'ltr';
    document.documentElement.setAttribute('dir', dir);
    document.documentElement.setAttribute('lang', i18n.language);
  }, [i18n.language]);

  return <div>{/* Your app */}</div>;
}
```

### RTL Language Codes

Common RTL languages:
- `ar` - Arabic (العربية)
- `he` - Hebrew (עברית)
- `fa` - Persian/Farsi (فارسی)
- `ur` - Urdu (اردو)

### Testing RTL

1. **Switch to RTL language** (e.g., Arabic)
2. **Check layout** - Ensure elements flip horizontally
3. **Test navigation** - Menus, sidebars should be on the right
4. **Test icons** - Directional icons (arrows) should flip
5. **Test forms** - Input fields should align right
6. **Test scrollbars** - Should appear on the left

---

[← Back to I18N-RULES.md](../I18N-RULES.md)
