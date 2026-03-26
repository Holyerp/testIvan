# Technology Stack

> **Instructions:** Define the technologies you'll use for this project. Be specific about versions where applicable. Claude will use this to generate appropriate code and architecture decisions.

---

## Frontend

### Core Framework
- **Framework:** React 18.x
- **Language:** TypeScript 5.x
- **Build Tool:** Vite 5.x / Create React App
- **Package Manager:** npm / yarn / pnpm

### Routing
- **Router:** React Router v6
- **Why:** Client-side routing, nested routes, data loading

### State Management
- **Global State:** Redux Toolkit / Zustand / Context API
- **Server State:** TanStack Query (React Query) / SWR
- **Form State:** React Hook Form / Formik
- **Why:** [Explain your choice]

### UI & Styling
- **CSS Framework:** Tailwind CSS / Material-UI / Chakra UI / Bootstrap
- **Component Library:** [If any]
- **Icons:** React Icons / Heroicons / Font Awesome
- **Animations:** Framer Motion / React Spring

### Data Fetching
- **HTTP Client:** Axios / Fetch API
- **Real-time:** Socket.io / WebSockets
- **Why:** [Explain your choice]

### Additional Frontend Libraries
- **Date Handling:** date-fns / Day.js / Luxon
- **Form Validation:** Zod / Yup / Joi
- **File Upload:** React Dropzone
- **Rich Text Editor:** [If needed]
- **Charts/Graphs:** [If needed - Recharts, Chart.js, etc.]

---

## Backend

### Runtime & Framework
- **Runtime:** Node.js 20.x LTS
- **Framework:** Express.js / Fastify / NestJS / Koa
- **Language:** TypeScript / JavaScript
- **Why:** [Explain your choice]

### Database

#### Primary Database
- **Type:** PostgreSQL / MySQL / MongoDB
- **Version:** [Specify]
- **ORM/ODM:** Prisma / TypeORM / Sequelize / Mongoose
- **Why:** [Explain your choice]

#### Caching
- **Cache:** Redis / Memcached
- **Use Cases:** Session storage, rate limiting, caching
- **Why:** [If applicable]

### Authentication & Authorization
- **Strategy:** JWT / Session-based / OAuth 2.0
- **Library:** Passport.js / jsonwebtoken / Auth0
- **Password Hashing:** bcrypt / argon2
- **Why:** [Explain your choice]

### API Design
- **Architecture:** RESTful / GraphQL / tRPC
- **Documentation:** Swagger/OpenAPI / GraphQL Playground
- **Validation:** Zod / Joi / Ajv / class-validator
- **Why:** [Explain your choice]

### File Storage
- **Service:** AWS S3 / Cloudinary / Local Storage
- **Library:** multer / multer-s3
- **Why:** [Explain your choice]

### Email Service
- **Provider:** SendGrid / AWS SES / Mailgun / Nodemailer
- **Templates:** Handlebars / EJS / React Email
- **Why:** [Explain your choice]

### Payment Processing
- **Provider:** Stripe / PayPal / Square
- **SDK:** @stripe/stripe-js
- **Why:** [Explain your choice]

---

## DevOps & Infrastructure

### Deployment
- **Hosting:** AWS / Vercel / Netlify / Railway / Heroku / DigitalOcean
- **Container:** Docker / Docker Compose
- **Orchestration:** Kubernetes / AWS ECS (if applicable)

### CI/CD
- **Pipeline:** GitHub Actions / GitLab CI / CircleCI / Jenkins
- **Automated Tests:** On every PR
- **Automated Deployment:** On merge to main

### Monitoring & Logging
- **APM:** New Relic / Datadog / Sentry
- **Logging:** Winston / Pino / Morgan
- **Error Tracking:** Sentry / Rollbar
- **Why:** [Explain your choice]

### Environment Variables
- **Management:** dotenv / docker secrets / AWS Parameter Store
- **Validation:** Zod environment schemas

---

## Testing

### Frontend Testing
- **Unit Tests:** Vitest / Jest
- **Component Tests:** React Testing Library
- **E2E Tests:** Playwright / Cypress
- **Coverage Tool:** Istanbul / c8

### Backend Testing
- **Unit Tests:** Jest / Vitest / Mocha
- **Integration Tests:** Supertest
- **Load Testing:** k6 / Apache JMeter
- **Mocking:** Jest mocks / Sinon

### Testing Strategy
- **Coverage Target:** 80%+
- **Run Tests:** Pre-commit hook, CI/CD pipeline
- **Critical Paths:** Authentication, payment, checkout

---

## Code Quality

### Linting & Formatting
- **Linter:** ESLint
- **Formatter:** Prettier
- **Git Hooks:** Husky + lint-staged
- **Config:** Shared ESLint config

### Type Checking
- **TypeScript:** Strict mode enabled
- **Type Coverage:** 90%+

### Code Review
- **Required Reviewers:** 1+
- **Automated Checks:** Tests, linting, build

---

## Third-Party Integrations

### APIs & Services
1. **[Service Name]**
   - Purpose: [What it's used for]
   - SDK/Library: [Package name]
   - Documentation: [Link]

2. **Google Maps API** (Example)
   - Purpose: Location services, address autocomplete
   - SDK/Library: @googlemaps/js-api-loader
   - Documentation: https://developers.google.com/maps

---

## Development Tools

### Required Software
- **Node.js:** v20.x LTS
- **IDE:** VS Code / WebStorm
- **Database Client:** DBeaver / TablePlus / pgAdmin
- **API Client:** Postman / Insomnia / Thunder Client

### VS Code Extensions (Recommended)
- ESLint
- Prettier
- TypeScript and JavaScript Language Features
- GitLens
- Thunder Client / REST Client
- Tailwind CSS IntelliSense (if using Tailwind)

### CLI Tools
- **Version Control:** git
- **Database Migrations:** Prisma CLI / TypeORM CLI
- **Package Scripts:** npm-run-all / concurrently

---

## Project Structure (Proposed)

```
project-root/
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── hooks/
│   │   ├── services/
│   │   ├── utils/
│   │   ├── types/
│   │   └── routes/
│   ├── public/
│   ├── package.json
│   └── vite.config.ts
│
├── backend/
│   ├── src/
│   │   ├── routes/
│   │   ├── controllers/
│   │   ├── services/
│   │   ├── models/
│   │   ├── middleware/
│   │   ├── utils/
│   │   └── types/
│   ├── tests/
│   ├── package.json
│   └── tsconfig.json
│
├── database/
│   ├── migrations/
│   ├── seeds/
│   └── schema/
│
├── docker/
│   ├── docker-compose.yml
│   ├── Dockerfile.frontend
│   └── Dockerfile.backend
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
└── docs/
    └── api/
```

---

## Performance Targets

- **First Contentful Paint:** < 1.5s
- **Time to Interactive:** < 3.5s
- **API Response Time (p95):** < 500ms
- **Database Query Time (p95):** < 100ms
- **Lighthouse Score:** > 90

---

## Browser & Platform Support

### Browsers
- Chrome (last 2 versions)
- Firefox (last 2 versions)
- Safari (last 2 versions)
- Edge (last 2 versions)

### Devices
- Desktop: 1920x1080 and above
- Tablet: 768px - 1024px
- Mobile: 375px - 768px

---

## Security Considerations

- **HTTPS Only:** Enforce SSL/TLS
- **CORS:** Properly configured
- **Rate Limiting:** API endpoints
- **Input Validation:** All user inputs
- **SQL Injection:** Parameterized queries
- **XSS Prevention:** Output escaping
- **CSRF Protection:** Tokens for forms
- **Dependency Scanning:** Snyk / npm audit
- **Secrets Management:** Never commit secrets

---

## Example: Modern Production Stack

> **Reference:** This is a real-world production stack from a deployed application with 848 passing tests on Railway.

### Frontend Stack
- **React** 19.0.0 - Latest stable with improved performance
- **React Router** 7.13.0 (SSR framework mode) - Full-stack routing with server-side rendering
- **TypeScript** 5.7.0 - Type safety across the stack
- **Vite** 6.1.0 - Lightning-fast build tool and dev server
- **Tailwind CSS** 4.1.0 (v4 Oxide engine) - Utility-first CSS with new engine
- **shadcn/ui** + **Radix UI** - Accessible component library
- **react-hook-form** + **zod** - Type-safe form handling and validation

### Backend Stack
- **PostgreSQL** 16.x - Production-grade relational database
- **Prisma** 6.19.0 - Type-safe ORM with excellent DX
- **React Router** (serve mode) - Unified frontend/backend in single framework

### Testing Stack
- **Vitest** 4.0.0 - Fast unit testing framework
- **@testing-library/react** 16.0.0 - User-centric component testing
- **Playwright** 1.58.0 - Reliable end-to-end testing
- **MSW** 2.7.0 - Mock Service Worker for API mocking

### Why This Stack?

**React Router 7 (Framework Mode):**
- Single framework for both frontend and backend
- Built-in SSR/SSG support
- Type-safe routing and data loading
- No need for separate Express/Fastify backend

**Prisma:**
- Excellent TypeScript integration
- Auto-generated types from schema
- Migration system for production safety
- Great developer experience

**Tailwind CSS v4:**
- New Oxide engine (Rust-based, 10x faster)
- Better performance than v3
- Zero-runtime CSS-in-JS alternative

**Vitest:**
- Faster than Jest (Vite-powered)
- Better TypeScript support
- Compatible with Jest API

**Maturity:** Production-ready (848 tests passing, deployed to Railway)

---

## Notes

<!-- Add any additional notes, constraints, or considerations -->

**Example:**
- We're using TypeScript strict mode throughout
- All API responses follow a consistent format: `{ success: boolean, data?: any, error?: string }`
- We prioritize developer experience with hot reloading and fast build times
- Mobile-first approach for responsive design

---

**Last Updated:** [Date]
**Updated By:** [Your Name]
