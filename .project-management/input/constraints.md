# Project Constraints

> **Instructions:** Define all constraints that will impact the project. Be realistic and specific. Claude will use this to plan sprints and make realistic estimates.

---

## Timeline Constraints

### Project Duration
- **Start Date:** [YYYY-MM-DD]
- **Target Launch Date:** [YYYY-MM-DD]
- **Total Duration:** [X weeks/months]
- **Hard Deadline:** Yes / No
- **Reason for Deadline:** [If applicable]

### Milestones
| Milestone | Target Date | Description |
|-----------|-------------|-------------|
| MVP Complete | [Date] | Core features functional |
| Beta Launch | [Date] | Limited user testing |
| Public Launch | [Date] | Full public release |
| [Other] | [Date] | [Description] |

---

## Budget Constraints

### Development Budget
- **Total Budget:** $[Amount] / €[Amount]
- **Development Costs:** $[Amount]
- **Infrastructure Costs:** $[Amount/month]
- **Third-Party Services:** $[Amount/month]
- **Contingency:** $[Amount] ([X]%)

### Ongoing Costs (Monthly)
| Item | Cost | Notes |
|------|------|-------|
| Hosting | $[X] | AWS/Vercel/etc. |
| Database | $[X] | PostgreSQL hosting |
| CDN | $[X] | CloudFlare/AWS CloudFront |
| Email Service | $[X] | SendGrid/SES |
| Payment Processing | [X]% | Stripe fees |
| Monitoring | $[X] | Sentry/Datadog |
| **Total** | **$[X]** | |

### Cost Optimization Requirements
- Must use free tier where possible
- Optimize for cost efficiency
- Budget alerts at 80% and 95%

---

## Team Constraints

### Team Size
- **Developers:** [Number]
- **Designers:** [Number]
- **QA/Testers:** [Number]
- **DevOps:** [Number]
- **Project Manager:** [Number]

### Team Availability
- **Full-time:** [Number] people
- **Part-time:** [Number] people ([X] hours/week)
- **Freelance/Contract:** [Number] people

### Team Skills & Experience
| Team Member | Role | Skills | Availability |
|-------------|------|--------|--------------|
| [Name] | Full-stack Dev | React, Node.js, PostgreSQL | 40h/week |
| [Name] | Frontend Dev | React, TypeScript, CSS | 20h/week |
| [Name] | Backend Dev | Node.js, Express, APIs | 40h/week |

### Team Limitations
- **Example:** Only one senior developer available for architectural decisions
- **Example:** Designer available only 2 days per week
- **Example:** No dedicated DevOps - developers must handle deployment

---

## Technical Constraints

### Infrastructure Limitations
- **Server Specs:** [If self-hosted]
  - CPU: [Cores]
  - RAM: [GB]
  - Storage: [GB]
  - Bandwidth: [GB/month]

- **Cloud Provider Restrictions:**
  - Must use [Provider] (company policy)
  - Region restrictions: [Regions]
  - Service limitations: [Any specific limitations]

### Technology Restrictions
- **Must Use:** [Required technologies]
  - Example: Must use company's existing PostgreSQL database
  - Example: Must integrate with existing SSO system

- **Cannot Use:** [Prohibited technologies]
  - Example: No AWS services (using Azure)
  - Example: No GPL-licensed libraries
  - Example: No cloud-based AI services (privacy concerns)

### Performance Constraints
- **Maximum Page Load Time:** [X] seconds
- **API Response Time (p95):** < [X]ms
- **Concurrent Users:** Support [X] concurrent users
- **Database Size Limit:** [X] GB
- **File Upload Size:** Max [X] MB per file

### Browser/Platform Support
- **Must Support:**
  - Modern browsers only (Chrome, Firefox, Safari, Edge - last 2 versions)
  - OR: IE11 support required (legacy users)

- **Device Support:**
  - Desktop only
  - OR: Responsive design (mobile, tablet, desktop)
  - OR: Mobile-first approach

---

## Compliance & Legal Constraints

### Data Privacy
- **GDPR Compliance:** Required / Not Required
- **CCPA Compliance:** Required / Not Required
- **HIPAA Compliance:** Required / Not Required
- **Data Residency:** Data must stay in [Region/Country]

### Security Requirements
- **Penetration Testing:** Required before launch
- **Security Audit:** [Frequency]
- **Compliance Standards:** [ISO 27001, SOC 2, etc.]
- **Data Encryption:** At rest and in transit
- **Two-Factor Authentication:** Required / Optional

### Legal Requirements
- **Terms of Service:** Required
- **Privacy Policy:** Required
- **Cookie Consent:** Required (EU users)
- **Age Restrictions:** [If applicable]
- **Content Moderation:** [If applicable]

---

## Integration Constraints

### Existing Systems
- **Must Integrate With:**
  1. **[System Name]**
     - Type: [CRM, ERP, etc.]
     - API Available: Yes / No
     - Documentation: [Link or "Limited"]
     - Constraint: [Any limitations]

  2. **[Example: Existing User Database]**
     - Type: SQL Server 2019
     - API: REST API available
     - Constraint: Read-only access, cannot modify schema

### Third-Party Dependencies
- **Payment Gateway:**
  - Provider: [Stripe/PayPal/etc.]
  - Constraint: [Transaction limits, fees, country restrictions]

- **Email Service:**
  - Provider: [SendGrid/etc.]
  - Constraint: [Daily send limit]

---

## Scope Constraints

### Features That Must Be Included
1. [Feature]: [Reason why it's mandatory]
2. [Feature]: [Reason why it's mandatory]

### Features That Cannot Be Included
1. [Feature]: [Reason for exclusion]
2. [Feature]: [Reason for exclusion]

### Known Limitations
- **Example:** Real-time notifications not in MVP (future phase)
- **Example:** Mobile app not in scope (web only)
- **Example:** Multi-language support not in v1

---

## Quality Constraints

### Testing Requirements
- **Code Coverage:** Minimum [X]%
- **Manual Testing:** Required for critical paths
- **UAT (User Acceptance Testing):** [X] days before launch
- **Performance Testing:** Load test with [X] concurrent users
- **Security Testing:** [Required/Not Required]

### Code Quality Standards
- **Code Review:** Required for all PRs
- **Linting:** Must pass ESLint with no errors
- **Type Safety:** TypeScript strict mode
- **Documentation:** All public APIs must be documented

---

## Operational Constraints

### Availability Requirements
- **Uptime Target:** [99%, 99.9%, 99.99%]
- **Maintenance Windows:** [When allowed]
- **Support Hours:** [24/7, business hours, etc.]

### Backup Requirements
- **Database Backups:** [Frequency]
- **Retention Period:** [X days/months]
- **Disaster Recovery:** RPO: [X hours], RTO: [X hours]

### Monitoring Requirements
- **Uptime Monitoring:** Required
- **Error Tracking:** Required
- **Performance Monitoring:** Required
- **Alert Response Time:** [X minutes]

---

## Dependencies on External Factors

### External Dependencies
1. **Design Approval:**
   - Depends on: [Stakeholder name]
   - Potential Delay: [X days]

2. **Legal Review:**
   - Depends on: Legal team
   - Potential Delay: [X days]

3. **Third-Party API:**
   - Depends on: [Provider] launching new API version
   - Risk: [Medium/High/Low]

---

## Risk Mitigation

### Identified Risks with Constraints

1. **Risk:** Tight deadline
   - **Impact:** May need to cut scope
   - **Mitigation:** Prioritize MVP features, defer nice-to-haves

2. **Risk:** Limited budget
   - **Impact:** Cannot use premium services
   - **Mitigation:** Use open-source alternatives, free tiers

3. **Risk:** Small team
   - **Impact:** Slower development
   - **Mitigation:** Limit scope, use libraries/frameworks to speed up development

4. **Risk:** [Your risk]
   - **Impact:** [Description]
   - **Mitigation:** [Strategy]

---

## Communication Constraints

### Reporting Requirements
- **Status Updates:** [Frequency - daily, weekly, bi-weekly]
- **Format:** [Email, Slack, meetings, dashboard]
- **Stakeholder Meetings:** [Frequency]

### Documentation Requirements
- **Code Documentation:** Required / Not Required
- **User Documentation:** Required / Not Required
- **API Documentation:** Required / Not Required
- **Handover Documentation:** Required / Not Required

---

## Change Management

### Change Request Process
- **Minor Changes:** [How approved]
- **Major Changes:** [How approved, who approves]
- **Scope Creep Prevention:** [Strategy]

### Version Control
- **Branching Strategy:** [GitFlow, trunk-based, etc.]
- **Release Cycle:** [Continuous, weekly, monthly]
- **Hotfix Process:** [Defined process]

---

## Notes

<!-- Any additional constraints or considerations -->

**Example:**
- Client prefers weekly demos every Friday
- No deployments on Fridays (company policy)
- Must maintain compatibility with existing mobile app API
- Legacy users still on old system until [date]

---

## Assumptions

<!-- What we're assuming to be true -->

1. [Assumption 1]
2. [Assumption 2]
3. [Assumption 3]

---

**Last Updated:** [Date]
**Updated By:** [Your Name]

---

## Summary Table

| Constraint Type | Key Limitation | Impact on Project |
|----------------|----------------|-------------------|
| Timeline | [X weeks/months] | [High/Medium/Low] |
| Budget | $[Amount] | [High/Medium/Low] |
| Team | [X developers] | [High/Medium/Low] |
| Technical | [Key tech constraint] | [High/Medium/Low] |
| Compliance | [Key requirement] | [High/Medium/Low] |
