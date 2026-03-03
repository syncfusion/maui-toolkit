---
name: pr-finalize
description: Finalizes any PR for merge by verifying title/description match implementation AND performing code review for best practices. Use when asked to "finalize PR", "check PR description", "review commit message", or before merging any PR.
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires GitHub CLI (gh) authenticated with access to syncfusion/maui-toolkit repository.
---

# PR Finalize

Ensures PR title and description accurately reflect the implementation, and performs a **code review** for best practices before merge.

**Standalone skill** - Can be used on any PR, not just PRs created by the bug-fix agent.

## Two-Phase Workflow

1. **Title & Description Review** - Verify PR metadata matches implementation
2. **Code Review** - Review code for best practices and potential issues

---

## 🚨 CRITICAL RULES

### 1. NEVER Approve or Request Changes

**AI agents must NEVER use `--approve` or `--request-changes` flags.**

| Action | Allowed? | Why |
|--------|----------|-----|
| `gh pr review --approve` | ❌ **NEVER** | Approval is a human decision |
| `gh pr review --request-changes` | ❌ **NEVER** | Blocking PRs is a human decision |

### 2. NEVER Post Comments Directly

**This skill is ANALYSIS ONLY.** Never post comments using `gh` commands.

| Action | Allowed? | Why |
|--------|----------|-----|
| `gh pr review --comment` | ❌ **NEVER** | Use ai-summary-comment skill instead |
| `gh pr comment` | ❌ **NEVER** | Use ai-summary-comment skill instead |
| Analyze and report findings | ✅ **YES** | This is the skill's purpose |

---

## Phase 1: Title & Description

### Core Principle: Preserve Quality

**Review existing description BEFORE suggesting changes.** Many PR authors write excellent, detailed descriptions. Your job is to:

1. **Evaluate first** - Is the existing description good?
2. **Preserve quality** - Don't replace a thorough description with a generic template
3. **Enhance, don't replace** - Add missing required elements without rewriting good content
4. **Only rewrite if needed** - When description is stale, inaccurate, or missing key information

### Title Requirements

**The title becomes the commit message headline.** Make it searchable and informative.

| Requirement | Good | Bad |
|-------------|------|-----|
| Platform prefix (if specific) | `[iOS] Fix SafeArea padding` | `Fix SafeArea padding` |
| Describes behavior, not issue | `[iOS] SafeArea: Return Empty for non-views` | `Fix #23892` |
| Captures the "what" | `Return Empty for non-ISafeAreaView` | `Fix SafeArea bug` |

#### Title Formula

```
[Platform] Component: What changed
```

Examples:
- `[iOS] BottomSheet: Fix crash when closed rapidly`
- `[Android] Calendar: Fix date selection not updating`
- `SfButton: Fix disabled state visual not applying`

### Description Requirements

PR description should:
1. Start with root cause section
2. Include description of change section
3. Reference the issue number
4. Match the actual implementation

```markdown
### Root Cause of the Issue

[Why the bug occurred - be specific about the code path]

### Description of Change

[What the code now does]

### Issues Fixed

Fixes #XXXXX

### Screenshots

#### Before:
#### After:
```

## Usage

```bash
# Get current state
gh pr view XXXXX --json title,body
gh pr view XXXXX --json files --jq '.files[].path'

# Review actual code changes
gh pr diff XXXXX
```

## Evaluation Workflow

### Step 1: Review Existing Description Quality

| Quality Indicator | Look For |
|-------------------|----------|
| **Structure** | Clear sections, headers, organized flow |
| **Technical depth** | Specific code references, root cause |
| **Accuracy** | Matches actual diff - not stale |
| **Completeness** | Platforms, breaking changes noted |

### Step 2: Compare to Template

Ask: "Is the existing description better than what my template would produce?"
- **If YES**: Keep existing, only add missing required elements
- **If NO**: Suggest improvements or replacement

### Step 3: Produce Output

- Recommended PR title (if change needed)
- Assessment of existing description
- Specific additions needed
- Only full replacement if description is inadequate

## Output Format

### When Existing Description is Good

```markdown
## PR #XXXXX Finalization Review

### ✅ Title: Good
**Current:** "Existing title"

### ✅ Description: Excellent - Keep As-Is

**Quality assessment:**
- Structure: ✅ Clear sections
- Technical depth: ✅ Root cause explained
- Accuracy: ✅ Matches implementation
- Completeness: ✅ Platforms noted

**Action:** No changes needed.
```

### When Description Needs Update

```markdown
## PR #XXXXX Finalization Review

### ⚠️ Title: Needs Update
**Current:** "Fix bug"
**Recommended:** "[iOS] BottomSheet: Fix crash when sheet dismissed during animation"

### ⚠️ Description: Needs Update

**Missing:**
- Root cause section
- Issue reference

**Recommended additions:**
[specific text to add]
```

---

## Phase 2: Code Review

After verifying title/description, perform a **code review**.

### Review Focus Areas

1. **Code quality** - Clean code, good naming, appropriate abstractions
2. **Error handling** - Null checks, exception handling, boundary conditions
3. **Performance** - Unnecessary allocations, blocking calls
4. **Platform-specific** - iOS/Android/Windows/macOS differences
5. **Breaking changes** - API changes, behavior changes

### How to Review

```bash
gh pr diff XXXXX
```

### Output Format

```markdown
## Code Review Findings

### 🔴 Critical Issues

**[Issue Title]**
- **File:** [path/to/file.cs]
- **Problem:** [Description]
- **Recommendation:** [Code fix or approach]

### 🟡 Suggestions

- [Suggestion 1]
- [Suggestion 2]

### ✅ Looks Good

- [Positive observation 1]
- [Positive observation 2]
```

## Common Issues

| Problem | Solution |
|---------|----------|
| Description doesn't match code | Update description to match actual diff |
| Missing root cause | Add root cause from issue/analysis |
| Missing issue reference | Add `Fixes #XXXXX` |
| Good description replaced | Evaluate existing quality first! |
