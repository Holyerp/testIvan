# Internationalization (i18n) Setup Guide

This guide explains how to enable and configure internationalization for your project.

---

## Do You Need i18n?

**Enable i18n if your project:**
- ✅ Targets users in multiple countries
- ✅ Requires multiple language support
- ✅ Has international clients/customers
- ✅ Needs localization (dates, currencies, number formats)

**Skip i18n if your project:**
- ❌ Only serves a single-language market
- ❌ Is an internal tool for one team
- ❌ Has no internationalization requirements

---

## How to Enable i18n

### Step 1: Edit `project-rules.md`

Open `.project-management/rules/project-rules.md` and find the **Internationalization (i18n)** section.

### Step 2: Configure Your Languages

Replace placeholders with your actual languages:

```markdown
**MANDATORY**: All user-facing features MUST include translations for:

- ✅ **English** (default) - `en.json`
- ✅ **German** - `de.json`           ← Replace with your language
- ✅ **French** - `fr.json`           ← Replace with your language
- ✅ **Spanish** - `es.json`          ← Add more or remove
```

### Step 3: Configure Technology

Set your i18n library:

```markdown
**Technology:** react-i18next

**Translation files location:** /app/locales/
```

**Popular i18n libraries:**
- React: `react-i18next`, `react-intl`, `next-intl` (Next.js)
- Vue: `vue-i18n`
- Angular: `@angular/localize`, `ngx-translate`
- Vanilla JS: `i18next`

### Step 4: Set Translation File Path

Update the path where translation files are stored:

```markdown
**Translation files location:** /app/locales/
```

Common paths:
- `/app/locales/` (Remix, React Router 7)
- `/public/locales/` (Next.js)
- `/src/i18n/` (Create React App)
- `/src/locales/` (Vue, Angular)

---

## Example: React Project with 3 Languages

### Configuration in `project-rules.md`:

```markdown
## Internationalization (i18n)

### Languages Supported

**MANDATORY**: All user-facing features MUST include translations for:

- ✅ **English** (default) - `en.json`
- ✅ **German** - `de.json`
- ✅ **French** - `fr.json`

### Translation System

**Technology:** react-i18next

**Translation files location:** /public/locales/

### Task Completion Criteria

Before marking task complete:
- [ ] All UI text uses translation keys
- [ ] English translations added
- [ ] German translations added
- [ ] French translations added
- [ ] Tested in all 3 languages
```

---

## File Structure Examples

### React with react-i18next:

```
public/
└── locales/
    ├── en/
    │   ├── common.json
    │   ├── auth.json
    │   └── errors.json
    ├── de/
    │   ├── common.json
    │   ├── auth.json
    │   └── errors.json
    └── fr/
        ├── common.json
        ├── auth.json
        └── errors.json
```

### Next.js with next-intl:

```
messages/
├── en.json
├── de.json
└── fr.json
```

### Remix/React Router 7:

```
app/
└── locales/
    ├── en.json
    ├── de.json
    └── fr.json
```

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

## Common Language Codes (ISO 639-1)

| Language | Code | Native Name |
|----------|------|-------------|
| English | `en` | English |
| German | `de` | Deutsch |
| French | `fr` | Français |
| Spanish | `es` | Español |
| Italian | `it` | Italiano |
| Portuguese | `pt` | Português |
| Dutch | `nl` | Nederlands |
| Polish | `pl` | Polski |
| Russian | `ru` | Русский |
| Japanese | `ja` | 日本語 |
| Chinese (Simplified) | `zh` | 中文 |
| Korean | `ko` | 한국어 |
| Arabic | `ar` | العربية |
| Turkish | `tr` | Türkçe |
| Swedish | `sv` | Svenska |

**Regional variants:**
- `en-US` - English (United States)
- `en-GB` - English (United Kingdom)
- `pt-BR` - Portuguese (Brazil)
- `pt-PT` - Portuguese (Portugal)
- `zh-CN` - Chinese (Simplified, China)
- `zh-TW` - Chinese (Traditional, Taiwan)

---

## Implementation Checklist

### 1. Install i18n Library

```bash
# React
npm install react-i18next i18next

# Next.js
npm install next-intl

# Vue
npm install vue-i18n
```

### 2. Create Translation Files

Create files for each language in your configured path.

### 3. Configure i18n Library

Setup initialization and language detection.

### 4. Add Language Switcher

Allow users to change language (optional but recommended).

### 5. Update Task Completion Checklist

Claude will check for translations before marking tasks complete.

---

## Testing i18n

**Manual testing:**
1. Switch to each language
2. Navigate through all pages
3. Trigger all error messages
4. Test form validation messages
5. Check date/number formatting

**Automated testing:**
```typescript
describe('Translations', () => {
  it('should have all required translation keys', () => {
    const en = require('./locales/en.json');
    const de = require('./locales/de.json');

    expect(Object.keys(en)).toEqual(Object.keys(de));
  });
});
```

---

## If You Don't Need i18n

Simply **delete the entire i18n section** from `project-rules.md`:

1. Open `.project-management/rules/project-rules.md`
2. Find `## Internationalization (i18n)`
3. Delete the entire section
4. Claude will not check for translations

---

## Need Help?

- **React i18n guide:** https://react.i18next.com/
- **Next.js i18n:** https://next-intl-docs.vercel.app/
- **Vue i18n:** https://vue-i18n.intlify.dev/
- **i18next docs:** https://www.i18next.com/

---

**Related Files:**
- Configuration: `.project-management/rules/project-rules.md`
- Quality gates: `.CLAUDE.MD` (checks for translations if i18n enabled)
