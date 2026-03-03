---
name: verify-tests-fail-without-fix
description: Two-phase test verification - confirms tests FAIL without the fix (proving they catch the bug) and PASS with the fix (proving it works). Use during Gate phase of bug-fix workflow.
compatibility: Requires PowerShell 7+, git, .NET SDK
---

# Verify Tests Fail Without Fix

Two-phase verification that confirms tests correctly detect a bug fix:
1. **Without fix** → Tests should FAIL (bug is reproduced)
2. **With fix** → Tests should PASS (bug is resolved)

## When to Use

- Gate phase of bug-fix workflow
- After writing new tests to verify they catch the bug
- Before merging a fix to confirm test quality

## Inputs

| Input | Required | Description |
|-------|----------|-------------|
| TestFilter | Yes | xUnit test filter (e.g., `FullyQualifiedName~Issue12345` or `FullyQualifiedName~SfButtonUnitTests.BorderColor`) |
| RequireFullVerification | No | If true, requires BOTH phases to pass (default: true) |

## Outputs

| Field | Description |
|-------|-------------|
| `status` | `Passed` or `Failed` |
| `fail-without-fix` | Did tests FAIL without the fix? (expected: Yes) |
| `pass-with-fix` | Did tests PASS with the fix? (expected: Yes) |
| `platform` | Platform tested on |

## Output Structure

```
CustomAgentLogsTmp/PRState/{PRNumber}/verify-tests-fail/
├── verification-report.md    # Full verification report
├── verification-log.txt      # Detailed log
├── test-without-fix.log      # Test output without fix
└── test-with-fix.log         # Test output with fix
```

## Workflow

### Step 1: Check if Tests Exist

**If PR exists:**
```bash
git diff origin/main HEAD --name-only | grep -E "Test"
```

**If issue only:**
```bash
find maui/tests -name "*XXXXX*" -type f 2>/dev/null
```

**If tests exist** → Proceed to verification.

**If NO tests exist** → Let the user know that tests are missing. They can use the `write-tests-agent` to help create them.

### Step 2: Run Verification

🚨 **MUST invoke the verification script:**

```bash
pwsh .github/skills/verify-tests-fail-without-fix/scripts/verify-tests-fail.ps1 \
  -TestFilter "FullyQualifiedName~<filter>" \
  -RequireFullVerification
```

### Expected Output

```
╔═══════════════════════════════════════════════════════════╗
║              VERIFICATION PASSED ✅                       ║
╠═══════════════════════════════════════════════════════════╣
║  - FAIL without fix (as expected)                         ║
║  - PASS with fix (as expected)                            ║
╚═══════════════════════════════════════════════════════════╝
```

### If Tests Don't Behave as Expected

**If tests PASS without fix** → Tests don't catch the bug. Let the user know the tests need to be fixed. They can use the `write-tests-agent` for help.

**If tests FAIL with fix** → The fix doesn't work. The fix needs to be revised.

## Completion Criteria

- [ ] Tests identified
- [ ] Verification script executed
- [ ] Result documented (PASSED or FAILED)
- [ ] Report saved to output directory
