# Init Project - i18n Setup Module

**Referenced by:** `init-project.md` STEP 2

---

## STEP 2: INTERNATIONALIZATION (i18n) SETUP

## Step 1: Enable i18n?

**Ask via AskUserQuestion** (gating, no Skip):

```
question: "Enable internationalization (i18n)?"
header: "i18n"
skippable: false
options:
  - label: "No (Recommended)"
    description: "English only. Add i18n later if needed via /add-scope."
  - label: "Yes — multiple languages"
    description: "Set up i18next + locale files. You'll pick the default + additional languages next."
```

If "No" → exit this module, default to English only.
If "Yes" → proceed to Step 2.

---

### Option 1: Configure i18n

**If user selects "Yes — multiple languages":**

## Step 2: Default language

**Ask via AskUserQuestion** (`skippable: true` — Skip = English):

```
question: "Default (primary) language?"
header: "Language"
skippable: true
default: "English"
options:
  - label: "English (Recommended)"
    description: "Code: en. Most common default."
  - label: "Spanish"
    description: "Code: es."
  - label: "German"
    description: "Code: de."
applies_to: [ input/i18n.md ]
```

The user can pick the AskUserQuestion native `Other` to type any other language (free-text). The free-text passes through the anonymization rule (defensive) before being persisted with its ISO code derived from a lookup table in this file.

## Step 3: Additional languages (loop)

After the default is set, ask iteratively:

```
question: "Add another language?"
header: "More langs"
skippable: false
options:
  - label: "No — I'm done"
    description: "Finalize i18n config with the languages chosen so far."
  - label: "Yes — add another"
    description: "Pick another language to support."
```

If "Yes — add another" → fire the same AskUserQuestion as Step 2 (default language). Loop until user picks "No — I'm done".

The user can pick the AskUserQuestion native `Other` to type any non-listed language (free-text → anonymized → ISO code lookup → persisted).

## ISO code lookup table

Used to derive a language code from a free-text `Other` answer:

| Free-text | ISO 639-1 |
|-----------|-----------|
| English   | en |
| Spanish   | es |
| German    | de |
| French    | fr |
| Italian   | it |
| Portuguese| pt |
| Dutch     | nl |
| Polish    | pl |
| Russian   | ru |
| Serbian   | sr |
| Croatian  | hr |
| Slovenian | sl |
| Czech     | cs |
| Slovak    | sk |

If the free-text doesn't match the table, emit a warning to the STEP G summary ("Language '<x>' not recognized; using as-is, may need manual ISO code correction in input/i18n.md") and proceed.

**After user provides languages:**

**1. Create `.project-management/rules/I18N-RULES.md`:**

```markdown
# Internationalization (i18n) Rules

**Status:** ✅ ENABLED

---

## Supported Languages

**Default Language:** English (en)

**Additional Languages:**
{{For each selected language:}}
- {{language_name}} ({{code}})

---

## Framework

**i18n Library:** i18next + react-i18next + remix-i18next
(From default stack)

**Translation Files Location:**
```
public/locales/
├── en/
│   └── translation.json
├── de/
│   └── translation.json
└── sr/
    └── translation.json
```

---

## Requirements

### All User-Facing Text MUST Be Translatable

**✅ CORRECT:**
```typescript
import { useTranslation } from 'react-i18next';

function Welcome() {
  const { t } = useTranslation();
  return <h1>{t('welcome.title')}</h1>;
}
```

**❌ INCORRECT:**
```typescript
function Welcome() {
  return <h1>Welcome to our app</h1>; // Hardcoded!
}
```

---

## Task Completion Criteria

**Story is NOT complete until:**
- [ ] All user-facing text uses translation keys
- [ ] Translation keys added to ALL language files ({{languages}})
- [ ] No hardcoded text in components
- [ ] Translation keys follow naming convention: `section.subsection.key`

---

## Translation Key Naming Convention

```
auth.login.title             → "Login"
auth.login.emailLabel        → "Email Address"
auth.login.passwordLabel     → "Password"
auth.login.submitButton      → "Sign In"
auth.login.forgotPassword    → "Forgot Password?"

dashboard.welcome            → "Welcome, {{name}}!"
dashboard.stats.users        → "Total Users"
dashboard.stats.revenue      → "Revenue"

errors.network.title         → "Connection Error"
errors.network.message       → "Please check your internet connection"
errors.validation.required   → "This field is required"
```

---

## Validation Before Marking Story Complete

**Execute these checks:**

1. **Search for hardcoded strings:**
   ```bash
   grep -r "\"[A-Z].*\"" src/components --include="*.tsx"
   grep -r "'[A-Z].*'" src/components --include="*.tsx"
   ```

2. **Verify translation files exist:**
   ```bash
   ls public/locales/{{lang}}/translation.json
   ```

3. **Check all keys present in all languages:**
   - en/translation.json has key `auth.login.title`
   - de/translation.json has key `auth.login.title`
   - sr/translation.json has key `auth.login.title`

---

**Last Updated:** {{date}}
```

**2. Create initial translation file structure:**

```bash
mkdir -p public/locales/{en,de,sr}
touch public/locales/en/translation.json
touch public/locales/de/translation.json
touch public/locales/sr/translation.json
```

**3. Create sample translation.json:**

```json
{
  "common": {
    "loading": "Loading...",
    "save": "Save",
    "cancel": "Cancel",
    "delete": "Delete",
    "edit": "Edit"
  },
  "auth": {
    "login": {
      "title": "Login",
      "emailLabel": "Email Address",
      "passwordLabel": "Password",
      "submitButton": "Sign In"
    }
  }
}
```

**4. Display confirmation:**

```
✅ i18n Configured Successfully

Default Language: English (en)
Additional Languages: German (de), Serbian (sr)

Translation Files Created:
✅ public/locales/en/translation.json
✅ public/locales/de/translation.json
✅ public/locales/sr/translation.json

⚠️  IMPORTANT: All user-facing text MUST use translation keys!

See rules: .project-management/rules/I18N-RULES.md
```

---

### Option 2: Skip i18n

**If user selects "No (Recommended)":**

**Display:**
```
✅ i18n Skipped

No translation requirements.
All text can be hardcoded.

I18N-RULES.md will not be created.
```

**Do NOT create I18N-RULES.md file.**

---

**Next Step:** Return to main `init-project.md` STEP 3 (Read input files)
