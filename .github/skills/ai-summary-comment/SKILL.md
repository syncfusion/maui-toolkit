---
name: ai-summary-comment
description: Posts or updates automated progress comments on GitHub PRs. Use after completing any bug-fix agent phase (pre-flight, gate, fix, report). Creates single aggregated review comment with collapsible sections.
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires GitHub CLI (gh) authenticated with access to syncfusion/maui-toolkit repository.
---

# PR Comment Skill

This skill posts automated progress comments to GitHub Pull Requests during the bug-fix workflow. Comments are **self-contained** with collapsible details, providing rich context to maintainers and contributors.

**⚠️ Self-Contained Rule**: All content in PR comments must be self-contained. Never reference local files like `CustomAgentLogsTmp/` — GitHub users cannot access your local filesystem.

**Key Features:**
- **Single Unified Comment**: ONE comment per PR containing ALL sections
- **Section-Based Updates**: Each script updates only its section, preserving others
- **Duplicate Prevention**: Finds existing `<!-- AI Summary -->` comment and updates it

## Comment Architecture

### Unified AI Summary Comment

Scripts post to the **same single comment** identified by `<!-- AI Summary -->`. Each script updates its own section:

```markdown
<!-- AI Summary -->

## 🤖 AI Summary

<!-- SECTION:PR-REVIEW -->
... PR review phases ...
<!-- /SECTION:PR-REVIEW -->

<!-- SECTION:TRY-FIX -->
... try-fix attempts ...
<!-- /SECTION:TRY-FIX -->

<!-- SECTION:VERIFY-TESTS -->
... test verification results ...
<!-- /SECTION:VERIFY-TESTS -->
```

## Section Scripts

| Section | Script | Description |
|---------|--------|-------------|
| `PR-REVIEW` | `post-ai-summary-comment.ps1` | PR agent review phases |
| `TRY-FIX` | `post-try-fix-comment.ps1` | Try-fix attempt results |
| `VERIFY-TESTS` | `post-verify-tests-comment.ps1` | Test verification results |
| PR Finalization | `post-pr-finalize-comment.ps1` | Separate comment for finalization |

## Supported Phases

| Phase | Description | When to Post |
|-------|-------------|--------------|
| `pre-flight` | Context gathering complete | After documenting issue and files |
| `gate` | Test validation complete | After running test verification |
| `fix` | Solution implemented | After implementing and testing fix |
| `report` | Final analysis complete | After generating review summary |

## Usage

### Post AI Summary Comment

```bash
# Auto-loads from CustomAgentLogsTmp/PRState/pr-XXXXX.md
pwsh .github/skills/ai-summary-comment/scripts/post-ai-summary-comment.ps1 -PRNumber XXXXX
```

### Post Try-Fix Comment

```bash
# Auto-discovers attempts from CustomAgentLogsTmp/PRState/XXXXX/try-fix/
pwsh .github/skills/ai-summary-comment/scripts/post-try-fix-comment.ps1 -IssueNumber XXXXX
```

### Post Verify-Tests Comment

```bash
# Auto-loads from CustomAgentLogsTmp/PRState/XXXXX/verify-tests-fail/
pwsh .github/skills/ai-summary-comment/scripts/post-verify-tests-comment.ps1 -PRNumber XXXXX
```

### Post PR Finalize Comment

```bash
pwsh .github/skills/ai-summary-comment/scripts/post-pr-finalize-comment.ps1 \
  -PRNumber XXXXX \
  -TitleStatus "Good" \
  -DescriptionStatus "NeedsUpdate" \
  -DescriptionAssessment "Missing root cause section"
```

### DryRun Mode

All scripts support `-DryRun` to preview without posting:

```bash
pwsh .github/skills/ai-summary-comment/scripts/post-ai-summary-comment.ps1 -PRNumber XXXXX -DryRun
```

## Comment Format

Comments are formatted with:
- **Phase badge** (🔍 Pre-Flight, 🚦 Gate, 🔧 Fix, 📋 Report)
- **Status indicator** (✅ Completed, ⚠️ Issues Found)
- **Collapsible sections** (`<details>` tags for detailed content)
- **What's Next** (what phase happens next)

### Example Output

```markdown
## 🔍 Pre-Flight: Context Gathering Complete

✅ **Status**: Phase completed successfully

### Summary
- **Issue**: #42 - SfButton crash on iOS when disabled
- **Platforms Affected**: iOS, macOS
- **Files Changed**: 2 source files, 1 test file

### Key Findings
- Crash occurs when setting IsEnabled=false during animation
- Existing test coverage is missing for disabled state transitions

### Next Steps
→ **Phase 2: Gate** - Verifying tests catch the bug

---
*Posted by Bug Fix Agent @ 2026-03-03 14:23:45 UTC*
```

## Output Directory Structure

```
CustomAgentLogsTmp/PRState/{PRNumber}/
├── pr-XXXXX.md                              # State file
├── try-fix/
│   ├── attempt-1/                           # Try-fix attempts
│   └── attempt-2/
├── verify-tests-fail/
│   └── verification-report.md               # Verification results
└── pr-finalize/
    └── pr-finalize-summary.md               # Finalization review
```

## Technical Details

- Comments identified by HTML marker `<!-- AI Summary -->`
- Existing comments are updated (not duplicated) when posting again
- Uses `gh api` for create/update operations
- Scripts require GitHub CLI authentication: `gh auth status`
