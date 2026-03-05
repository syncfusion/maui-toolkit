---
name: multi-model-analysis
description: "Runs 3 AI models in parallel to independently analyze a problem and propose approaches. Compares all proposals against a rubric and selects the best. Can be invoked standalone or called from any agent (issue-resolver, pr-review, etc.)."
---

# Skill: Multi-Model Analysis

Analyze a problem or code change using three AI models in parallel, then compare and select the best approach.

## When to Use This Skill

- ✅ Validate a proposed fix before implementing
- ✅ Review a PR's code changes from multiple perspectives
- ✅ Get a second opinion on an architectural approach
- ✅ Called from another agent to enrich its analysis

---

## Inputs Required

The caller (agent or user) must provide:

| Input | Description |
|-------|-------------|
| **Subject** | What is being analyzed — a proposed fix, a PR diff, a design approach, etc. |
| **Context** | Issue/PR description, root cause (if known), affected files and relevant source snippets |
| **Primary approach** | The default model's or caller's current proposed approach (optional — omit if no prior proposal exists) |

---

## Step 1: Permission Prompt

Before running, show the user:

```
🤖 Multi-Model Analysis available.
Subject: [one-line summary of what will be analyzed]

Three AI models will independently analyze this and propose approaches.
All proposals will be compared and the best selected.

Proceed with Multi-Model Analysis? (yes / no)
```

- **NO** → Return to the caller with the primary approach unchanged.
- **YES** → Continue below.

---

## Step 2: Launch 3 Parallel Agents

Launch all three agents **simultaneously** using the `explore` agent type with this prompt:

```
You are a senior .NET MAUI engineer reviewing code for the Syncfusion Toolkit for .NET MAUI.

Subject: [what is being analyzed]

Context:
[issue/PR description, root cause, affected files]

Source code context:
[relevant source snippets]

[If a primary approach exists:]
Primary approach already proposed:
[paste the primary approach]

Task: Propose your own independent approach. Include:
1. Exact files and methods involved
2. The proposed change (before/after or pseudocode)
3. Why this approach is correct and safe
4. Any risks or edge cases to watch for
[If primary approach exists: 5. Whether you agree or disagree with the primary approach, and why]

Be concise and specific. Do NOT implement — only propose.
```

| Agent | Model |
|-------|-------|
| Agent A | `claude-sonnet-4.6` |
| Agent B | `gpt-5.1` |
| Agent C | `gemini-3-pro-preview` |

---

## Step 3: Evaluate and Select the Best Approach

Compare all proposals (primary + 3 agents) using this rubric:

| Criterion | Weight |
|-----------|--------|
| Correctness — directly addresses the subject | High |
| Minimal change — fewest lines/files affected | High |
| Safety — no regressions, handles edge cases | High |
| Consistency with existing code style | Medium |
| Testability — easy to verify | Medium |

Select the **best approach** — one proposal or a synthesis of multiple.

---

## Step 4: Report the Result

Return a structured summary to the caller:

```markdown
## Multi-Model Analysis Result
| Model | Approach Summary | Selected? |
|-------|-----------------|-----------|
| Primary / Default | [brief summary or "N/A"] | ✅ / ❌ |
| claude-sonnet-4.6 | [brief summary] | ✅ / ❌ |
| gpt-5.1 | [brief summary] | ✅ / ❌ |
| gemini-3-pro-preview | [brief summary] | ✅ / ❌ |

**Selected Approach**: [model name, or "synthesis of X+Y"]
**Reason**: [why this approach was chosen]
**Next Step**: [what the caller should do with this result]
```

If invoked from an agent, the agent records this in its state file and proceeds with the selected approach.
If invoked directly, present the result to the user.

