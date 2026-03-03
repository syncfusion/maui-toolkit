---
name: learn-from-pr
description: Analyzes a completed PR to extract lessons learned from agent behavior. Identifies patterns to reinforce or fix, and generates actionable recommendations for instruction files, skills, and documentation.
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires GitHub CLI (gh)
---

# Learn From PR

Extracts lessons learned from a completed PR to improve repository documentation and agent capabilities.

## Inputs

| Input | Required | Source |
|-------|----------|--------|
| PR number or Issue number | Yes | User provides (e.g., "PR #42" or "issue 42") |
| Session markdown | Optional | `CustomAgentLogsTmp/PRState/pr-XXXXX.md` |

## Outputs

1. **Learning Analysis** - Structured markdown with:
   - What happened (problem, attempts, solution)
   - Fix location analysis (attempted vs actual)
   - Failure modes identified
   - Prioritized recommendations

2. **Actionable Recommendations** - Each with:
   - Category, Priority, Location, Specific Change, Why It Helps

## Completion Criteria

- [ ] Gathered PR diff and metadata
- [ ] Analyzed fix location (attempted vs actual)
- [ ] Identified failure modes
- [ ] Generated at least one concrete recommendation
- [ ] Presented findings to user

## When to Use

- After agent failed to find the right fix
- After agent succeeded but took many attempts
- After agent succeeded quickly (to understand what worked)
- When asked "what can we learn from PR #XXXXX?"

## When NOT to Use

- Before PR is finalized (use `pr-finalize` first)
- For trivial PRs (typo fixes)
- When no agent was involved

---

## Workflow

### Step 1: Gather Data

```bash
gh pr view XXXXX --json title,body,files
gh pr diff XXXXX

# Check for session markdown
ls CustomAgentLogsTmp/PRState/pr-XXXXX.md 2>/dev/null
```

### Step 2: Fix Location Analysis

**Critical question:** Did agent attempts target the same files as the final fix?

```bash
gh pr view XXXXX --json files --jq '.files[].path' | grep -v test
```

| Scenario | Implication |
|----------|-------------|
| Same files | Agent found right location |
| Different files | **Major learning opportunity** |

### Step 3: Analyze Outcome

#### Scenario A: Agent Failed

| Pattern | Indicator |
|---------|-----------|
| **Wrong file entirely** | All attempts in File A, fix in File B |
| **Tunnel vision** | Only looked at file mentioned in error |
| **Trusted issue title** | Issue said "crash in X" so only looked at X |
| **Missing platform knowledge** | Didn't know iOS/Android specifics |
| **Wrong abstraction layer** | Fixed handler when problem was in core |
| **Over-engineered** | Complex fix when simple one existed |

#### Scenario B: Agent Succeeded Slowly

| Pattern | Indicator |
|---------|-----------|
| **Correct file, wrong approach** | Found right file but tried wrong fixes |
| **Needed multiple iterations** | Each attempt got closer |
| **Missing domain knowledge** | Had to learn something documentable |

#### Scenario C: Agent Succeeded Quickly

| Pattern | Indicator |
|---------|-----------|
| **Good search strategy** | Found right file immediately |
| **Understood the pattern** | Recognized similar issues |
| **Documentation helped** | Existing docs pointed to solution |

### Step 4: Find Improvement Locations

| Location | When to Add Here |
|----------|------------------|
| `.github/copilot-instructions.md` | General AI workflow guidance |
| `.github/skills/*/SKILL.md` | Skill needs new step or checklist |
| Code comments | Non-obvious code behavior |

### Step 5: Generate Recommendations

For each recommendation:
1. **Category:** Instruction file / Skill / Code comment
2. **Priority:** High / Medium / Low
3. **Location:** Exact file path
4. **Specific Change:** Exact text to add
5. **Why It Helps:** Which failure mode it prevents

**Pattern-to-Improvement Mapping:**

| Pattern | Likely Improvement |
|---------|-------------------|
| Wrong file entirely | Document component relationships |
| Tunnel vision | "Always search for pattern across codebase" |
| Missing platform knowledge | Platform-specific documentation |
| Wrong abstraction layer | Document layer responsibilities |
| Over-engineered | Skill: "Try simplest fix first" |

### Step 6: Present Findings

Present analysis covering:
- What happened and what made it hard
- Where agent looked vs actual fix location
- Which patterns applied with evidence
- Prioritized recommendations with full details

## Error Handling

| Situation | Action |
|-----------|--------|
| PR not found | Ask user to verify PR number |
| No session markdown | Analyze PR diff only |
| Can't determine failure mode | State "insufficient data" |

## Constraints

- **Analysis only** - Don't apply changes (use learn-from-pr agent for that)
- **Actionable recommendations** - Every recommendation must have specific file path and text
- **Don't duplicate** - Check existing docs before recommending new ones
- **Focus on high-value learning** - Skip trivial observations
