# Best Practices & Success Metrics

**Version:** 3.3.0
**Last Reviewed:** 2026-04-21
**Purpose:** Guidelines for effective project management and success measurement

---

## 📝 Input Files Best Practices

### 1. Completeness
✅ **Fill all sections** - Don't leave sections empty or with placeholders
✅ **Be comprehensive** - Include all known requirements upfront
✅ **No TODOs** - Replace all `[TODO]` markers before `/init-project`

❌ **Don't:**
- Leave example text in place
- Skip sections thinking "I'll fill it later"
- Use vague descriptions like "TBD" or "To be determined"

### 2. Specificity
✅ **Be precise** - "API response <100ms" not "fast API"
✅ **Use numbers** - "500 concurrent users" not "many users"
✅ **Name technologies** - "PostgreSQL 16.x" not "database"

❌ **Don't:**
- Write vague requirements
- Use ambiguous terms
- Skip version numbers

### 3. Regular Updates
✅ **Update when scope changes** - Keep input files current
✅ **Document new requirements** - Add to backlog immediately
✅ **Track changes** - Use git to version input files

❌ **Don't:**
- Let input files become stale
- Keep requirements only in your head
- Forget to regenerate docs after updates

### 4. Prioritization
✅ **Prioritize ruthlessly** - Not everything can be P0
✅ **Use priority levels correctly:**
  - **P0** - Critical, blocks launch
  - **P1** - High priority, needed for launch
  - **P2** - Nice to have, can defer
  - **P3** - Wishlist, future versions

❌ **Don't:**
- Mark everything as P0
- Skip priority assignment
- Change priorities without documenting why

### 5. Constraints Documentation
✅ **Be realistic and honest** - Over-promising leads to failure
✅ **Document ALL constraints** - Budget, time, team, technical
✅ **Identify risks early** - Don't hide potential problems
✅ **Plan for contingencies** - Buffer time and budget

❌ **Don't:**
- Hide constraints to "look good"
- Ignore technical limitations
- Skip risk assessment

---

## 🎯 Phase Planning Best Practices

### 1. Realistic Milestones
✅ **Plan 1-4 month phases** - Not 2-week sprints
✅ **Set clear milestones** - Each phase has concrete deliverables
✅ **Balance scope** - Mix high-value and foundational work

❌ **Don't:**
- Overstuff phases with too many stories
- Plan unrealistic timelines
- Skip milestone definition

### 2. Respect Velocity
✅ **Use past phase data** - Learn from previous phases
✅ **Track story points** - Measure team capacity
✅ **Adjust estimates** - Based on actual velocity

❌ **Don't:**
- Ignore historical data
- Plan more work than team can handle
- Use wishful thinking for estimates

### 3. Risk Management
✅ **Balance risk** - Mix high-risk and low-risk items
✅ **Check dependencies** - Don't pick blocked work
✅ **Plan for unknowns** - Include research/spike stories

❌ **Don't:**
- Stack all high-risk items in one phase
- Ignore dependencies
- Assume everything will go smoothly

### 4. Automated Planning
✅ **Use `/execute-work phase N`** - Let AI handle planning
✅ **Trust the automation** - Quality gates ensure correctness
✅ **Review plans** - But don't micromanage

❌ **Don't:**
- Manually plan when automation exists
- Skip plan mode review
- Override automation without good reason

---

## 📊 Progress Tracking Best Practices

### 1. Automated Tracking (v3.0)
✅ **Use automated execution** - Progress tracked during `/execute-work`
✅ **Real-time updates** - No manual progress logging needed
✅ **Trust the system** - Automation is accurate

❌ **Don't:**
- Manually update progress when using automation
- Duplicate tracking in external tools
- Disable automatic tracking

### 2. Honesty
✅ **Be honest about blockers** - Report issues immediately
✅ **Don't hide delays** - Transparency enables help
✅ **Celebrate wins** - Log completed work proudly

❌ **Don't:**
- Hide problems hoping they'll resolve
- Inflate progress reports
- Skip blocker documentation

### 3. Documentation
✅ **Document blockers clearly** - What, why, impact, mitigation
✅ **Track resolution** - When/how blockers were resolved
✅ **Learn from blockers** - Post-mortem analysis

❌ **Don't:**
- Leave vague blocker descriptions
- Forget to update blocker status
- Repeat same blockers without learning

### 4. Phase Monitoring
✅ **Track across 1-4 month phases** - Not daily/weekly
✅ **Monitor trends** - Velocity, quality, blockers
✅ **Adjust as needed** - Based on phase progress

❌ **Don't:**
- Obsess over daily progress
- Panic over short-term fluctuations
- Ignore long-term trends

---

## 📚 Documentation Best Practices

### 1. Keep in Sync
✅ **Regenerate when inputs change** - Use `/generate-docs`
✅ **Update inputs, not outputs** - Source of truth is input files
✅ **Version control docs** - Commit with code

❌ **Don't:**
- Edit generated docs directly (they'll be overwritten)
- Let docs drift from code
- Skip git commits for documentation

### 2. Use as Reference
✅ **Refer to docs during development** - They're there to help
✅ **Share with team** - Everyone should have access
✅ **Link in code comments** - Reference relevant docs

❌ **Don't:**
- Generate docs and never read them
- Keep docs hidden from team
- Duplicate documentation

### 3. Preserve Manual Additions
✅ **Mark manual sections** - Use `<!-- MANUAL -->` comments
✅ **Extract to separate files** - For extensive manual content
✅ **Document in input files** - To persist across regenerations

❌ **Don't:**
- Add extensive manual content to generated files
- Forget to mark manual sections
- Lose manual work during regeneration

### 4. Language Policy
✅ **English only** - All documentation in English
✅ **Industry standard** - Enables collaboration
✅ **AI optimized** - Claude works best with English

❌ **Don't:**
- Mix languages in documentation
- Use non-English technical terms unnecessarily
- Translate code comments to other languages

---

## 🐛 Bug Management Best Practices

### 1. Immediate Reporting
✅ **Add bugs promptly** - Use `/add-bug` immediately
✅ **Clear reproduction steps** - Enable quick fixing
✅ **Accurate severity** - Critical/High/Medium/Low

❌ **Don't:**
- Delay bug reporting
- Write vague bug descriptions
- Overuse "Critical" severity

### 2. Prioritization
✅ **Critical → Immediate** - Drop everything and fix
✅ **High → This phase** - Fix before phase completion
✅ **Medium → When possible** - Fit into phase planning
✅ **Low → Backlog** - Fix when convenient

❌ **Don't:**
- Ignore critical bugs
- Let high-severity bugs linger
- Mark everything as critical

### 3. Fix Verification
✅ **Test the fix** - Ensure bug is actually resolved
✅ **Regression test** - Ensure no new bugs introduced
✅ **Document in archive** - Move to bug-archive.md

❌ **Don't:**
- Mark bugs fixed without testing
- Skip regression testing
- Delete bug history

---

## ✅ Code Quality Best Practices

### 1. Follow Standards
✅ **Read `CLAUDE.md`** - Project coding standards
✅ **Apply SOLID principles** - Clean, maintainable code
✅ **Follow DRY** - Don't repeat yourself

❌ **Don't:**
- Skip code quality reviews
- Ignore established patterns
- Write duplicate code

### 2. Testing
✅ **80%+ coverage** - Comprehensive test suite
✅ **Test all status codes** - 200, 400, 401, 403, 404, 500
✅ **Automated testing** - Tests run automatically (v3.0)

❌ **Don't:**
- Skip tests to "save time"
- Test only happy paths
- Ignore test failures

### 3. Git Conventions
✅ **Conventional commits** - `feat:`, `fix:`, `docs:`, etc.
✅ **NO AI credits in commits** - Keep commits professional
✅ **Automated commits** - Let `/execute-work` create commits

❌ **Don't:**
- Write vague commit messages
- Add AI credits to commits
- Skip git commit conventions

---

## 📈 Success Metrics

### Planning Accuracy

**Phase Completion Rate**
- **Target:** >80%
- **Measure:** Stories completed / Stories planned
- **Track:** Per phase, overall project

**Velocity Predictability**
- **Target:** <15% variance
- **Measure:** Actual velocity vs predicted
- **Track:** Phase over phase

### Documentation Quality

**Docs Up-to-Date**
- **Target:** 100%
- **Check:** Weekly review
- **Measure:** Input changes vs doc regenerations

**Developer Satisfaction**
- **Target:** >4/5 rating
- **Survey:** Monthly team feedback
- **Measure:** Documentation usefulness

### Progress Visibility

**Stakeholder Satisfaction**
- **Target:** >4/5 rating
- **Survey:** After each phase
- **Measure:** Clarity of progress reports

**Time Saved on Reports**
- **Target:** >80% reduction
- **Measure:** Time before vs after automation
- **Track:** Monthly

### Risk Management

**Blockers Identified Early**
- **Target:** >90% before critical
- **Measure:** Blockers reported vs discovered
- **Track:** Throughout project

**Issues Resolved Proactively**
- **Target:** >70%
- **Measure:** Issues prevented vs reacted to
- **Track:** Post-phase review

### Quality Metrics

**Test Coverage**
- **Target:** >80%
- **Measure:** Lines covered / Total lines
- **Track:** Automated (v3.0)

**Bug Density**
- **Target:** <0.5 bugs per story
- **Measure:** Total bugs / Total stories
- **Track:** Per phase

**Code Review Pass Rate**
- **Target:** >90% first attempt
- **Measure:** PRs approved / Total PRs
- **Track:** Weekly

---

## 🎯 System Effectiveness Indicators

### Green Flags (System Working Well)
✅ Phases completing on time
✅ Documentation always current
✅ Minimal manual progress tracking
✅ Blockers identified and resolved quickly
✅ Team refers to documentation regularly
✅ Stakeholders satisfied with visibility

### Yellow Flags (Needs Attention)
⚠️ Phases occasionally slip
⚠️ Documentation sometimes outdated
⚠️ Some manual tracking still happening
⚠️ Blockers taking longer to resolve
⚠️ Team sometimes confused on priorities

### Red Flags (System Not Working)
🔴 Phases consistently miss deadlines
🔴 Documentation rarely updated
🔴 Heavy manual tracking required
🔴 Blockers unresolved for weeks
🔴 Team ignores documentation
🔴 Stakeholders complaining about visibility

**If you see red flags:** Review this guide, ensure input files are complete, use automation fully.

---

[← Back to README](../README.md)
