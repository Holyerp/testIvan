# Open Clarification Questions — Pinoles

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

---

## Summary

**Total Open:** 2
**P0 (Blocker):** 0  •  **P1 (Important):** 1  •  **P2 (Nice-to-know):** 1

---

## Open Questions

### Q-001: Azure AD App Registration Ownership

**Status:** Resolved
**Priority:** P0 (Blocker)
**Category:** infrastructure
**Asked During:** /init-project on 2026-05-25
**Applies to:** T-002 (BC Middleware Foundation), all BC data stories

**Question:**
Who is responsible for creating the Azure AD App Registration (service principal) used for OAuth 2.0 Client Credentials auth toward Business Central — the client or the development team?

**Default Recommendation:**
Client team creates the App Registration and provides `clientId`, `clientSecret`, and `tenantId` to the dev team via secure channel. Dev team should not have access to the production BC tenant directly.

---

### Q-002: BC Endpoint Type — Standard vs. Custom Extensions

**Status:** Resolved
**Priority:** P0 (Blocker)
**Category:** integration
**Asked During:** /init-project on 2026-05-25
**Applies to:** T-002, US-003 through US-023

**Question:**
Does the client's Business Central tenant use standard Microsoft OData v4 API endpoints, or does it use custom extension endpoints (e.g., BiH/SRB localization extensions for VAT/tax documents)?

**Default Recommendation:**
Assume standard endpoints for MVP development. Client to confirm by Phase 1 start. If custom extensions exist, a mapping layer will be required in BcQueryService.

---

### Q-003: Advance Invoice Format (BiH/SRB Localization)

**Status:** Open
**Priority:** P1 (Important)
**Category:** integration
**Asked During:** /init-project on 2026-05-25
**Applies to:** US-014, US-015 (Advance Invoices)

**Question:**
What is the format/structure of advance invoices in this BC tenant — standard BC advance invoice schema, or a BiH/SRB-specific localized format with different field names?

**Default Recommendation:**
Investigate actual BC API response for advance invoices during Phase 1 kickoff. Design BcQueryService mapper to handle both variants.

---

### Q-004: Audit Log — Required Before Launch?

**Status:** Resolved
**Priority:** P1 (Important)
**Category:** compliance
**Asked During:** /init-project on 2026-05-25
**Applies to:** US-023 (Audit Log), Phase 4

**Question:**
Is the audit log (who viewed which documents) required before the application goes live, or is it acceptable to deliver it in Phase 4 as currently planned?

**Default Recommendation:**
Deliver in Phase 4 as planned. If compliance requires it earlier, move US-023 to Phase 2 and adjust capacity accordingly.

---

### Q-005: Offline Mode Requirement

**Status:** Open
**Priority:** P2 (Nice-to-know)
**Category:** scope
**Asked During:** /init-project on 2026-05-25
**Applies to:** Architecture (caching strategy)

**Question:**
Is there any requirement for the application to function when Business Central is temporarily unavailable (e.g., BC maintenance windows)? Should the portal show stale cached data, or is an "unavailable" message acceptable?

**Default Recommendation:**
Show last cached data (5–10 min TTL) with a "BC Disconnected" status banner in the topbar. Full offline mode is out of scope for v1.

---

## Resolved Questions

*(none yet)*

---

**Resume via:** `/resolve-questions` to run interactive Q&A for any Open entries.
