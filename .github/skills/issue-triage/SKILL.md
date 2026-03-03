---
name: issue-triage
description: Queries and triages open GitHub issues for the syncfusion/maui-toolkit repository. Helps identify issues needing milestones, labels, or investigation.
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires GitHub CLI (gh) installed and authenticated.
---

# Issue Triage Skill

This skill helps triage open GitHub issues in the syncfusion/maui-toolkit repository by:
1. Initializing a session with current milestones and labels
2. Loading a batch of issues into memory
3. Presenting issues ONE AT A TIME for triage decisions
4. Suggesting milestones based on issue characteristics
5. Tracking progress through a triage session

## Prerequisites

**GitHub CLI (gh) must be installed and authenticated:**

```bash
# macOS:
brew install gh

# Authenticate
gh auth login
```

## When to Use

- "Find issues to triage"
- "Let's triage issues"
- "Grab me 10 issues to triage"
- "Triage Android issues"

## Triage Workflow

**🚨 CRITICAL: ALWAYS use the skill scripts. NEVER use ad-hoc GitHub API queries.**

### Step 1: Initialize Session

```bash
pwsh .github/skills/issue-triage/scripts/init-triage-session.ps1
```

**What this does:**
- Fetches current milestones from GitHub API
- Displays active milestones for reference during triage
- Creates session file to track progress

### Step 2: Load Issues Into Memory

```bash
pwsh .github/skills/issue-triage/scripts/query-issues.ps1 -Limit 50 -OutputFormat triage
```

**What this does:**
- Queries GitHub with exclusion filters
- Returns issues without milestones
- Stores results for one-at-a-time presentation

### Step 3: Present ONE Issue at a Time

**IMPORTANT**: Present only ONE issue at a time:

```markdown
## Issue #XXXXX

**[Title]**

🔗 [URL]

| Field | Value |
|-------|-------|
| **Author** | username |
| **Platform** | platform |
| **Labels** | labels |
| **Linked PR** | PR info if available |
| **Regression** | Yes/No |
| **Comments** | count |

**Comment Summary** (if any):
- [Author] Comment preview...

**My Suggestion**: `Milestone` - Reason

---

What would you like to do with this issue?
```

### Step 4: Wait for User Decision

Wait for user to say:
- A milestone name (e.g., "Backlog", "v1.1")
- "yes" to accept suggestion
- "skip" or "next" to move on
- Specific instructions (e.g., "add regression label")

### Step 5: Move to Next Issue

After user decision, automatically present the NEXT issue.

### Step 6: When Batch is Empty

**🚨 Automatically reload more issues:**

```bash
pwsh .github/skills/issue-triage/scripts/query-issues.ps1 -Limit 50 -Skip <current_count> -OutputFormat triage
```

## Script Parameters

### query-issues.ps1

| Parameter | Values | Default | Description |
|-----------|--------|---------|-------------|
| `-Platform` | android, ios, windows, maccatalyst, all | all | Filter by platform |
| `-Label` | Any label string | "" | Filter by label |
| `-Limit` | 1-1000 | 50 | Maximum issues to fetch |
| `-Skip` | 0+ | 0 | Skip first N issues (pagination) |
| `-OutputFormat` | table, json, triage | table | Output format |

## Milestone Suggestion Logic

### Suggestion Guidelines

| Condition | Suggested Milestone | Reason |
|-----------|---------------------|--------|
| Linked PR has milestone | PR's milestone | "PR already has milestone" |
| Has regression label | Current servicing milestone | "Regression - needs servicing" |
| Has open linked PR | Current milestone | "Has open PR" |
| Default | Backlog | "No PR, not a regression" |

## Applying Triage Decisions

```bash
# Set milestone only
gh issue edit ISSUE_NUMBER --repo syncfusion/maui-toolkit --milestone "Backlog"

# Set milestone and add labels
gh issue edit ISSUE_NUMBER --repo syncfusion/maui-toolkit --milestone "MILESTONE_NAME" --add-label "i/regression"
```

## Label Quick Reference

**Bug Labels:**
- `t/bug` - Bug report
- `i/regression` - Confirmed regression

**Platform Labels:**
- Platform is captured in issue template dropdown

**Priority Labels:**
- `p/0` - Critical
- `p/1` - High
- `p/2` - Medium
- `p/3` - Low

## Common Mistakes to Avoid

| Mistake | Correct Approach |
|---------|------------------|
| ❌ Stopping when batch is empty | ✅ Automatically reload with -Skip N |
| ❌ Asking "Load more?" | ✅ Just load more automatically |
| ❌ Guessing milestone names | ✅ Use actual names from init-triage-session.ps1 |
