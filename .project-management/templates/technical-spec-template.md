# Technical Specification

> **IMPORTANT:** This document must be written in **English only**. No exceptions.

**Project Name:** {{PROJECT_NAME}}
**Version:** 1.0
**Date:** {{DATE}}
**Status:** Draft / In Review / Approved
**Author:** {{AUTHOR}}
**Related Documents:** [PRD](../output/docs/prd.md), [Architecture](../output/docs/architecture.md)

---

## 1. Overview

### 1.1 Purpose
{{PURPOSE}}

### 1.2 Scope
{{SCOPE}}

### 1.3 Definitions & Acronyms
| Term | Definition |
|------|------------|
| {{TERM_1}} | {{DEFINITION_1}} |
| {{TERM_2}} | {{DEFINITION_2}} |

---

## 2. Technology Stack

### 2.1 Frontend
- **Framework:** {{FRONTEND_FRAMEWORK}}
- **Language:** {{FRONTEND_LANGUAGE}}
- **State Management:** {{STATE_MANAGEMENT}}
- **UI Library:** {{UI_LIBRARY}}
- **Build Tool:** {{BUILD_TOOL}}

### 2.2 Backend
- **Runtime:** {{BACKEND_RUNTIME}}
- **Framework:** {{BACKEND_FRAMEWORK}}
- **Language:** {{BACKEND_LANGUAGE}}
- **API Style:** {{API_STYLE}}

### 2.3 Database
- **Primary Database:** {{DATABASE}}
- **ORM/ODM:** {{ORM}}
- **Caching:** {{CACHE}}

### 2.4 Infrastructure
- **Hosting:** {{HOSTING}}
- **CI/CD:** {{CICD}}
- **Monitoring:** {{MONITORING}}

---

## 3. System Architecture

### 3.1 High-Level Architecture
{{HIGH_LEVEL_ARCHITECTURE}}

```
[Frontend] <---> [API Gateway] <---> [Backend Services] <---> [Database]
                                            |
                                            v
                                    [External Services]
```

### 3.2 Component Diagram
{{COMPONENT_DIAGRAM}}

### 3.3 Data Flow
{{DATA_FLOW}}

---

## 4. Frontend Architecture

### 4.1 Directory Structure
```
frontend/
├── src/
│   ├── components/       # Reusable components
│   │   ├── common/       # Generic components
│   │   └── features/     # Feature-specific components
│   ├── pages/            # Route pages
│   ├── hooks/            # Custom React hooks
│   ├── services/         # API calls
│   ├── utils/            # Helper functions
│   ├── types/            # TypeScript types
│   ├── config/           # Configuration
│   ├── routes/           # Route definitions
│   ├── styles/           # Global styles
│   └── App.tsx           # Root component
├── public/               # Static assets
└── tests/                # Test files
```

### 4.2 Routing Strategy
{{ROUTING_STRATEGY}}

**Route Table:**
| Path | Component | Protected | Loader |
|------|-----------|-----------|--------|
| {{PATH_1}} | {{COMPONENT_1}} | {{YES/NO}} | {{LOADER_1}} |
| {{PATH_2}} | {{COMPONENT_2}} | {{YES/NO}} | {{LOADER_2}} |

### 4.3 State Management Strategy
{{STATE_MANAGEMENT_STRATEGY}}

### 4.4 API Integration
{{API_INTEGRATION}}

---

## 5. Backend Architecture

### 5.1 Directory Structure
```
backend/
├── src/
│   ├── routes/           # Route definitions
│   ├── controllers/      # Request handlers
│   ├── services/         # Business logic
│   ├── models/           # Data models
│   ├── middleware/       # Express middleware
│   ├── utils/            # Helper functions
│   ├── types/            # TypeScript types
│   ├── config/           # Configuration
│   └── server.ts         # Entry point
├── tests/                # Test files
└── prisma/               # Database schema (if using Prisma)
    ├── schema.prisma
    └── migrations/
```

### 5.2 API Design

#### 5.2.1 RESTful Endpoints
{{API_ENDPOINTS}}

#### 5.2.2 Request/Response Format
```json
// Standard Success Response
{
  "success": true,
  "data": { ... }
}

// Standard Error Response
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable message"
  }
}
```

#### 5.2.3 Authentication Flow
{{AUTH_FLOW}}

### 5.3 Middleware Stack
1. {{MIDDLEWARE_1}}
2. {{MIDDLEWARE_2}}
3. {{MIDDLEWARE_3}}

---

## 6. Database Design

### 6.1 Entity Relationship Diagram (ERD)
{{ERD}}

### 6.2 Schema Design

#### Table: users
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| id | UUID | PK | User ID |
| email | VARCHAR(255) | UNIQUE, NOT NULL | User email |
| password_hash | VARCHAR(255) | NOT NULL | Hashed password |
| role | ENUM | NOT NULL | User role |
| created_at | TIMESTAMP | NOT NULL | Creation timestamp |
| updated_at | TIMESTAMP | NOT NULL | Last update timestamp |

#### Table: {{TABLE_2}}
{{TABLE_2_SCHEMA}}

### 6.3 Indexes
{{INDEXES}}

### 6.4 Database Migrations Strategy
{{MIGRATION_STRATEGY}}

---

## 7. API Specification

### 7.1 Authentication Endpoints

#### POST /api/auth/register
**Description:** Register a new user

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123",
  "name": "John Doe"
}
```

**Response (200):**
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "uuid",
      "email": "user@example.com",
      "name": "John Doe"
    },
    "token": "jwt-token"
  }
}
```

**Errors:**
- 400: Invalid input
- 409: Email already exists

---

#### POST /api/auth/login
{{LOGIN_ENDPOINT}}

---

### 7.2 User Endpoints
{{USER_ENDPOINTS}}

### 7.3 Product Endpoints
{{PRODUCT_ENDPOINTS}}

---

## 8. Security Implementation

### 8.1 Authentication
{{AUTH_IMPLEMENTATION}}

### 8.2 Authorization
{{AUTHZ_IMPLEMENTATION}}

### 8.3 Data Protection
- **Encryption at Rest:** {{ENCRYPTION_REST}}
- **Encryption in Transit:** TLS 1.3
- **Password Hashing:** {{PASSWORD_HASHING}}
- **Sensitive Data:** {{SENSITIVE_DATA_HANDLING}}

### 8.4 Input Validation
{{INPUT_VALIDATION}}

### 8.5 Rate Limiting
{{RATE_LIMITING}}

### 8.6 CORS Configuration
{{CORS_CONFIG}}

---

## 9. Error Handling

### 9.1 Error Codes
| Code | HTTP Status | Description |
|------|-------------|-------------|
| {{CODE_1}} | {{STATUS_1}} | {{DESC_1}} |
| {{CODE_2}} | {{STATUS_2}} | {{DESC_2}} |

### 9.2 Error Logging
{{ERROR_LOGGING}}

---

## 10. Testing Strategy

### 10.1 Unit Testing
{{UNIT_TESTING}}

### 10.2 Integration Testing
{{INTEGRATION_TESTING}}

### 10.3 E2E Testing
{{E2E_TESTING}}

### 10.4 Performance Testing
{{PERFORMANCE_TESTING}}

### 10.5 Test Coverage Goals
- **Backend:** 80%+
- **Frontend:** 70%+
- **Critical Paths:** 95%+

---

## 11. Deployment Strategy

### 11.1 Environments
| Environment | Purpose | URL |
|-------------|---------|-----|
| Development | Local development | localhost |
| Staging | Pre-production testing | {{STAGING_URL}} |
| Production | Live environment | {{PRODUCTION_URL}} |

### 11.2 CI/CD Pipeline
{{CICD_PIPELINE}}

### 11.3 Deployment Process
{{DEPLOYMENT_PROCESS}}

### 11.4 Rollback Strategy
{{ROLLBACK_STRATEGY}}

---

## 12. Monitoring & Logging

### 12.1 Application Monitoring
{{APP_MONITORING}}

### 12.2 Error Tracking
{{ERROR_TRACKING}}

### 12.3 Performance Monitoring
{{PERF_MONITORING}}

### 12.4 Logging Strategy
{{LOGGING}}

---

## 13. Performance Optimization

### 13.1 Frontend Optimization
- {{FRONTEND_OPT_1}}
- {{FRONTEND_OPT_2}}
- {{FRONTEND_OPT_3}}

### 13.2 Backend Optimization
- {{BACKEND_OPT_1}}
- {{BACKEND_OPT_2}}
- {{BACKEND_OPT_3}}

### 13.3 Database Optimization
- {{DB_OPT_1}}
- {{DB_OPT_2}}
- {{DB_OPT_3}}

### 13.4 Caching Strategy
{{CACHING}}

---

## 14. Third-Party Integrations

### 14.1 Payment Processing
{{PAYMENT_INTEGRATION}}

### 14.2 Email Service
{{EMAIL_INTEGRATION}}

### 14.3 File Storage
{{STORAGE_INTEGRATION}}

### 14.4 Other Integrations
{{OTHER_INTEGRATIONS}}

---

## 15. Data Management

### 15.1 Backup Strategy
{{BACKUP_STRATEGY}}

### 15.2 Data Retention
{{DATA_RETENTION}}

### 15.3 GDPR Compliance
{{GDPR_COMPLIANCE}}

---

## 16. Development Guidelines

### 16.1 Code Style
{{CODE_STYLE}}

### 16.2 Git Workflow
{{GIT_WORKFLOW}}

### 16.3 Code Review Process
{{CODE_REVIEW}}

### 16.4 Documentation Standards
{{DOCUMENTATION}}

---

## 17. Technical Debt & Future Improvements

{{TECHNICAL_DEBT}}

---

## 18. Open Technical Questions

1. {{TECH_QUESTION_1}}
2. {{TECH_QUESTION_2}}
3. {{TECH_QUESTION_3}}

---

## 19. Appendix

### 19.1 References
- {{REFERENCE_1}}
- {{REFERENCE_2}}

### 19.2 Related Documents
- [PRD](../output/docs/prd.md)
- [Architecture Diagram](../output/docs/architecture.md)
- [API Documentation](../output/docs/api-spec.md)

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | {{DATE}} | {{AUTHOR}} | Initial draft |

---

**Document Owner:** {{OWNER}}
**Last Updated:** {{DATE}}
