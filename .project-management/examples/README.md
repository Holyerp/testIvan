# Project Examples

This folder contains example files demonstrating best practices for different project structures.

---

## 📁 Available Examples

### `backlog-monorepo-example.md`

**Use case:** Backend + Mobile App (Monorepo)

**Demonstrates:**
- Prefix convention: [BE], [Mobile], [Shared], [Full-stack]
- Full-stack story coordination (e.g., US-007: Social login touches both BE and Mobile)
- Shared packages organization (shared-types, api-client, shared-utils)
- Epic organization for monorepo projects

**When to use:**
- Building mobile app with custom backend
- Need to share TypeScript types between backend and mobile
- Want unified codebase for better collaboration

**See:** [backlog-monorepo-example.md](backlog-monorepo-example.md)

---

## 🚀 How to Use These Examples

### When Starting a New Monorepo Project:

1. **Run `/init-project`**
   - Choose option [2] Backend + Mobile App (Monorepo)

2. **Copy prefix structure from example:**
   - Review `backlog-monorepo-example.md`
   - Use same prefix convention in your `backlog.md`
   - Organize stories by component ([BE], [Mobile], [Shared], [Full-stack])

3. **Generated monorepo structure:**
   ```
   your-project/
   ├── apps/
   │   ├── backend/
   │   └── mobile/
   ├── packages/
   │   ├── shared-types/
   │   ├── api-client/
   │   └── shared-utils/
   ├── package.json (root)
   ├── pnpm-workspace.yaml
   └── turbo.json
   ```

4. **Execute work:**
   ```bash
   /execute-work story US-001  # Backend story
   /execute-work story US-004  # Mobile story
   /execute-work story US-007  # Full-stack story (BE + Mobile)
   ```

---

## 📝 Backlog Prefix Guide

### For Monorepo Projects:

| Prefix | Meaning | Example |
|--------|---------|---------|
| `[BE]` | Backend only | US-001: [BE] JWT authentication API |
| `[Mobile]` | Mobile app only | US-004: [Mobile] Login screen UI |
| `[Web]` | Web app only (if option 3) | US-010: [Web] Admin dashboard |
| `[Shared]` | Shared package | US-002: [Shared] User TypeScript types |
| `[Full-stack]` | Multiple apps | US-007: [Full-stack] Social login (BE + Mobile) |

### For Single App Projects:

**No prefix needed** - all stories apply to single application

---

## 🎯 Story Breakdown Best Practices

### Full-stack Stories

When a feature touches multiple apps, create:

**Option 1: Single full-stack story (for small features)**
```markdown
- US-007: [Full-stack] Social login (Google/Apple)
  - Story Points: 13
  - Includes: BE OAuth endpoints + Mobile sign-in buttons + Shared types
```

**Option 2: Separate stories (for large features)**
```markdown
- US-007: [BE] Social login OAuth endpoints (8 pts)
- US-008: [Shared] Social auth TypeScript types (2 pts)
- US-009: [Mobile] Google/Apple sign-in UI (5 pts)
- Dependencies: US-009 depends on US-007, US-008
```

**Recommendation:** Use Option 2 for better granularity and parallel work

---

## 🏗️ Monorepo Setup Details

### What `/init-project` Creates (Option 2):

**Root Configuration:**
- `package.json` - Workspace configuration with dev/build/test scripts
- `pnpm-workspace.yaml` - Workspace definition for pnpm
- `turbo.json` - Build pipeline configuration
- `.gitignore` - Updated for monorepo (node_modules, dist, .turbo)

**Backend App (`apps/backend/`):**
- `package.json` - Backend dependencies
- `tsconfig.json` - TypeScript config
- `src/` - Source code directory
- `tests/` - Test directory

**Mobile App (`apps/mobile/`):**
- `package.json` - React Native dependencies
- `app.json` - Expo configuration
- `src/` - Source code directory
- `android/` - Android platform files
- `ios/` - iOS platform files

**Shared Types (`packages/shared-types/`):**
- `package.json` - Package config
- `tsconfig.json` - TypeScript config
- `src/index.ts` - Type exports (User, Product, ApiResponse, etc.)

**API Client (`packages/api-client/`):**
- `package.json` - Package config
- `src/client.ts` - API wrapper class with methods

**Shared Utils (`packages/shared-utils/`):**
- `package.json` - Package config
- `src/validation.ts` - Validation functions
- `src/constants.ts` - Shared constants

---

## 💡 Tips

### When to use Monorepo:
- ✅ Building mobile app with custom backend
- ✅ Need to share types/utilities between apps
- ✅ Want synchronized versioning
- ✅ Single team working on full stack

### When NOT to use Monorepo:
- ❌ Consuming third-party API only (use option 4: Web Only)
- ❌ Backend and mobile are completely independent with different teams
- ❌ Very different deployment schedules (backend 10x/day, mobile 1x/month still works with monorepo, but consider multi-repo)

### Best Practices:
1. **Keep shared packages focused** - Don't dump everything into shared-utils
2. **Use TypeScript** - Shared types are the biggest monorepo benefit
3. **Test independently** - Each app/package should have its own tests
4. **Build incrementally** - Turbo only rebuilds changed packages
5. **Document dependencies** - Use story dependencies in backlog

---

## 📚 Related Documentation

- **Init Project Command:** [.claude/commands/init-project.md](../../.claude/commands/init-project.md)
- **Structure Setup Module:** [.claude/commands/modules/init-project-structure-setup.md](../../.claude/commands/modules/init-project-structure-setup.md)
- **Quick Start Guide:** [.claude/commands/how-to-use/start-project.md](../../.claude/commands/how-to-use/start-project.md)
- **Integration Guide:** [.project-management/INTEGRATION-GUIDE.md](../INTEGRATION-GUIDE.md)

---

**Last Updated:** 2026-04-14
