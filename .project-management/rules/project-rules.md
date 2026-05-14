# Project-Specific Rules

> This file contains rules and conventions specific to THIS project. These rules complement the global `CLAUDE.md` guidelines.

---

## Project Overview
- **Project Name:** {{PROJECT_NAME}}
- **Type:** {{PROJECT_TYPE}}
- **Tech Stack:** {{TECH_STACK}}

---

## Project-Specific Coding Standards

### Naming Conventions (if different from defaults)
{{NAMING_CONVENTIONS}}

### Project-Specific Patterns
{{PATTERNS}}

### Code Organization Rules
{{ORGANIZATION_RULES}}

---

## Business Logic Rules

### Domain-Specific Rules
{{DOMAIN_RULES}}

**Example:**
- Users must be 18+ to register
- Product prices must be positive numbers
- Order total must match sum of line items plus tax

---

## Data Validation Rules

{{VALIDATION_RULES}}

**Example:**
- Email must be valid format
- Phone numbers must be in E.164 format
- Passwords must contain: 8+ chars, 1 uppercase, 1 lowercase, 1 number

---

## API Conventions

### Response Format
{{RESPONSE_FORMAT}}

### Error Codes
{{ERROR_CODES}}

### Rate Limiting
{{RATE_LIMITING}}

---

## Database Rules

### Schema Conventions
{{SCHEMA_CONVENTIONS}}

### Query Patterns
{{QUERY_PATTERNS}}

---

## Internationalization (i18n)

> **Note:** If your project requires multiple languages, see detailed i18n rules in `.project-management/rules/I18N-RULES.md`. If not needed, delete or ignore that file.

**Configuration:** See `I18N-RULES.md` for:
- Supported languages
- Translation system setup
- Task completion criteria
- Code examples

**Quick reference:**
- ✅ **If i18n is enabled:** All user-facing text MUST be translatable
- ❌ **If i18n is disabled:** Delete or ignore `I18N-RULES.md` file

---

## Testing Requirements

> **Note:** If your project has specific testing requirements beyond defaults, see `.project-management/rules/TESTING-RULES.md`. If not needed, delete or ignore that file.

**General testing rules:** See `.claude/rules/testing.md` for:
- Test coverage requirements (80%+ overall, 95%+ critical paths)
- API status code matrix (200/400/401/403/404/500)
- Test types and organization

**Project-specific testing rules:** See `TESTING-RULES.md` for:
- Critical user flows
- Custom test utilities
- Performance benchmarks
- Security test requirements

**Quick reference:**
- ✅ **If TESTING-RULES.md exists:** Follow project-specific testing requirements
- ❌ **If TESTING-RULES.md missing:** Use only general testing.md rules

---

## Security Rules

### Project-Specific Security Requirements
{{SECURITY_REQUIREMENTS}}

---

## Deployment Rules

### Pre-Deployment Checklist
- [ ] {{CHECKLIST_ITEM_1}}
- [ ] {{CHECKLIST_ITEM_2}}
- [ ] {{CHECKLIST_ITEM_3}}

---

## Integration Rules

### External Service Integration Guidelines
{{INTEGRATION_GUIDELINES}}

---

## Performance Requirements

### Specific Performance Targets
{{PERFORMANCE_TARGETS}}

---

## Compliance Requirements

### Regulatory Requirements
{{COMPLIANCE}}

---

## Notes

{{ADDITIONAL_NOTES}}

---

**Last Updated:** {{DATE}}
