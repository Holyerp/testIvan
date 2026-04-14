# Init Project - Structure Setup Module

**Purpose:** Configure project structure (monorepo vs single app) and setup workspace.

**Parent Command:** `/init-project`

---

## STEP 0: PROJECT STRUCTURE SELECTION

**Ask user to choose project structure:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🏗️  PROJECT STRUCTURE SETUP
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

What type of project are you building?

[1] Backend Only
    → Single backend API application
    → Express/NestJS/FastAPI server

[2] Backend + Mobile App (Monorepo) ⭐ RECOMMENDED
    → Backend API + React Native/Flutter mobile app
    → Shared TypeScript types and utilities
    → Single repository, unified development

[3] Backend + Web + Mobile (Full Monorepo)
    → Backend API + Web app + Mobile app
    → Maximum code sharing
    → Enterprise-grade setup

[4] Web Only
    → Frontend-only application
    → React/Next.js/Vue
    → Consumes external APIs

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Enter your choice [1-4]:
```

---

## Processing User Selection

### Option 1: Backend Only

**Structure:**
```
project-root/
├── src/
├── tests/
├── package.json
├── tsconfig.json
└── .project-management/
```

**Setup Steps:**
1. Create standard single-app structure
2. No workspace configuration needed
3. Single package.json
4. Proceed to tech stack selection

**Technologies.md note:**
```markdown
**Project Type:** Backend Only
**Repository Type:** Single Application
```

---

### Option 2: Backend + Mobile (Monorepo) ⭐

**Structure:**
```
project-root/
├── apps/
│   ├── backend/              # Backend API
│   │   ├── src/
│   │   ├── tests/
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   └── mobile/               # Mobile app
│       ├── src/
│       ├── android/
│       ├── ios/
│       ├── package.json
│       └── app.json
│
├── packages/
│   ├── shared-types/         # TypeScript interfaces
│   │   ├── src/
│   │   │   ├── User.ts
│   │   │   ├── Product.ts
│   │   │   └── index.ts
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   ├── api-client/           # API wrapper for mobile
│   │   ├── src/
│   │   │   └── client.ts
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   └── shared-utils/         # Common utilities
│       ├── src/
│       │   ├── validation.ts
│       │   └── constants.ts
│       ├── package.json
│       └── tsconfig.json
│
├── .project-management/
├── package.json              # Root workspace config
├── pnpm-workspace.yaml
├── turbo.json
└── .gitignore
```

**Setup Steps:**

**1. Create Root package.json:**
```json
{
  "name": "{{PROJECT_NAME}}",
  "private": true,
  "version": "1.0.0",
  "scripts": {
    "dev": "turbo run dev",
    "build": "turbo run build",
    "test": "turbo run test",
    "lint": "turbo run lint",
    "backend:dev": "turbo run dev --filter=backend",
    "mobile:dev": "turbo run dev --filter=mobile"
  },
  "devDependencies": {
    "turbo": "^2.0.0"
  },
  "engines": {
    "node": ">=20.0.0",
    "pnpm": ">=9.0.0"
  },
  "packageManager": "pnpm@9.0.0"
}
```

**2. Create pnpm-workspace.yaml:**
```yaml
packages:
  - 'apps/*'
  - 'packages/*'
```

**3. Create turbo.json:**
```json
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", "build/**", ".next/**"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    },
    "test": {
      "dependsOn": ["build"],
      "outputs": ["coverage/**"]
    },
    "lint": {
      "outputs": []
    }
  }
}
```

**4. Create apps/backend/package.json:**
```json
{
  "name": "backend",
  "version": "1.0.0",
  "private": true,
  "main": "dist/index.js",
  "scripts": {
    "dev": "tsx watch src/index.ts",
    "build": "tsc",
    "test": "vitest",
    "lint": "eslint src"
  },
  "dependencies": {
    "shared-types": "workspace:*"
  },
  "devDependencies": {
    "typescript": "^5.0.0",
    "tsx": "^4.0.0"
  }
}
```

**5. Create apps/mobile/package.json:**
```json
{
  "name": "mobile",
  "version": "1.0.0",
  "private": true,
  "main": "index.js",
  "scripts": {
    "start": "expo start",
    "android": "expo start --android",
    "ios": "expo start --ios",
    "web": "expo start --web",
    "build": "expo export",
    "test": "jest"
  },
  "dependencies": {
    "shared-types": "workspace:*",
    "api-client": "workspace:*",
    "shared-utils": "workspace:*"
  }
}
```

**6. Create packages/shared-types/package.json:**
```json
{
  "name": "shared-types",
  "version": "1.0.0",
  "private": true,
  "main": "dist/index.js",
  "types": "dist/index.d.ts",
  "scripts": {
    "build": "tsc",
    "dev": "tsc --watch"
  },
  "devDependencies": {
    "typescript": "^5.0.0"
  }
}
```

**7. Create packages/shared-types/src/index.ts:**
```typescript
// Example shared types
export interface User {
  id: string;
  email: string;
  name: string;
  createdAt: Date;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}
```

**8. Create packages/api-client/src/client.ts:**
```typescript
import type { User, ApiResponse } from 'shared-types';

export class ApiClient {
  private baseUrl: string;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  async getUser(id: string): Promise<ApiResponse<User>> {
    const response = await fetch(`${this.baseUrl}/users/${id}`);
    return response.json();
  }
}
```

**9. Update .gitignore:**
```
# Dependencies
node_modules/
.pnp
.pnp.js

# Build outputs
dist/
build/
.next/
.expo/

# Monorepo
.turbo/

# Environment
.env
.env.local

# Mobile
android/app/build/
ios/Pods/
*.ipa
*.apk

# IDE
.vscode/
.idea/
```

**10. Create README.md in root:**
```markdown
# {{PROJECT_NAME}}

Monorepo with Backend + Mobile

## Structure

- `apps/backend` - Backend API server
- `apps/mobile` - React Native mobile app
- `packages/shared-types` - Shared TypeScript types
- `packages/api-client` - API wrapper for mobile
- `packages/shared-utils` - Common utilities

## Getting Started

### Prerequisites

- Node.js 20+
- pnpm 9+

### Installation

```bash
pnpm install
```

### Development

```bash
# Run all apps
pnpm dev

# Run backend only
pnpm backend:dev

# Run mobile only
pnpm mobile:dev
```

### Testing

```bash
pnpm test
```

### Building

```bash
pnpm build
```
```

**Technologies.md note:**
```markdown
**Project Type:** Backend + Mobile App
**Repository Type:** Monorepo (pnpm workspaces + Turborepo)

**Applications:**
- Backend: Node.js + TypeScript + Express
- Mobile: React Native + Expo + TypeScript

**Shared Packages:**
- shared-types: TypeScript interfaces/types
- api-client: API wrapper for mobile
- shared-utils: Common utilities

**Package Manager:** pnpm 9.x
**Build Tool:** Turborepo 2.x
```

---

### Option 3: Backend + Web + Mobile (Full Monorepo)

**Structure:**
```
project-root/
├── apps/
│   ├── backend/
│   ├── web/                  # Web application
│   └── mobile/
├── packages/
│   ├── shared-types/
│   ├── api-client/
│   ├── ui-components/        # Shared UI (if using React Native Web)
│   └── shared-utils/
├── package.json
├── pnpm-workspace.yaml
└── turbo.json
```

**Additional Setup:**
- Add `apps/web/package.json` for Next.js/Remix/Vite
- Add `packages/ui-components` if using shared React components

---

### Option 4: Web Only

**Structure:**
```
project-root/
├── src/
├── public/
├── package.json
└── .project-management/
```

**Setup Steps:**
1. Create standard frontend structure
2. No monorepo needed
3. Configure for Next.js/Vite/Create React App

---

## Backlog.md Prefix Convention

**For monorepo projects, use prefixes in backlog.md:**

```markdown
## Epic 1: User Authentication
- **US-001**: [BE] JWT authentication API (5 pts)
- **US-002**: [Mobile] Login screen with biometric (3 pts)
- **US-003**: [Shared] Auth types and interfaces (2 pts)

## Epic 2: Product Catalog
- **US-004**: [BE] Products CRUD API (8 pts)
- **US-005**: [Shared] Product TypeScript types (2 pts)
- **US-006**: [Mobile] Product listing screen (5 pts)
- **US-007**: [Mobile] Product detail screen (3 pts)

## Epic 3: Shopping Cart
- **US-008**: [BE] Cart session management (5 pts)
- **US-009**: [Mobile] Cart UI with animations (8 pts)
- **US-010**: [Full-stack] Cart sync across devices (13 pts)
```

**Prefix Legend:**
- `[BE]` - Backend only
- `[Mobile]` - Mobile only
- `[Web]` - Web only (if option 3)
- `[Shared]` - Shared package
- `[Full-stack]` - Touches multiple apps (BE + Mobile/Web)

---

## Post-Setup Actions

**After structure is created:**

1. **Run initial install:**
   ```bash
   pnpm install
   ```

2. **Verify workspace setup:**
   ```bash
   pnpm list --depth 0
   ```

3. **Update technologies.md** with structure details

4. **Proceed to STEP 1: Tech Stack Selection**

---

## Summary Display

**Show user what was created:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ PROJECT STRUCTURE CREATED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📦 Repository Type: Monorepo (Backend + Mobile)

📁 Structure Created:
   ├── apps/
   │   ├── backend/     ✅ Backend API server
   │   └── mobile/      ✅ React Native app
   ├── packages/
   │   ├── shared-types/    ✅ TypeScript types
   │   ├── api-client/      ✅ API wrapper
   │   └── shared-utils/    ✅ Common utilities
   └── Configuration files  ✅ pnpm, turbo, gitignore

🔧 Package Manager: pnpm 9.x
🚀 Build Tool: Turborepo 2.x

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

**Related:**
- Parent: `.claude/commands/init-project.md`
- Next: `modules/init-project-stack-selection.md`
