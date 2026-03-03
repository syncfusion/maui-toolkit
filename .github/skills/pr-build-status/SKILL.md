---
name: pr-build-status
description: "Retrieve CI build status for GitHub Pull Requests, including GitHub Actions workflow results and error details."
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires GitHub CLI (gh) authenticated with access to syncfusion/maui-toolkit repository.
---

# PR Build Status Skill

Retrieve CI build information for GitHub Pull Requests, including workflow status, failed jobs, and error details.

## When to Use

- User asks about CI/CD status for a PR
- User asks about failed checks or builds
- User asks "what's failing on PR #XXXXX"
- User wants to see test results

## Scripts

All scripts are in `.github/skills/pr-build-status/scripts/`

### 1. Get PR Check Status

```bash
pwsh .github/skills/pr-build-status/scripts/Get-PrBuildStatus.ps1 -PrNumber <PR_NUMBER>
```

Shows all GitHub Actions check runs for the PR, including:
- Workflow name and status
- Conclusion (success, failure, neutral, etc.)
- Duration and timestamps

### 2. Get Failed Check Details

```bash
pwsh .github/skills/pr-build-status/scripts/Get-PrBuildStatus.ps1 -PrNumber <PR_NUMBER> -FailedOnly
```

Shows only failed checks with error details.

## Script Parameters

### Get-PrBuildStatus.ps1

| Parameter | Required | Description |
|-----------|----------|-------------|
| `-PrNumber` | Yes | Pull request number |
| `-FailedOnly` | No | Show only failed checks |
| `-Detailed` | No | Include full log output for failed checks |

## Workflow

### Investigating Build Failures

1. Get check status: `Get-PrBuildStatus.ps1 -PrNumber XXXXX`
2. For failed checks, get details: `Get-PrBuildStatus.ps1 -PrNumber XXXXX -FailedOnly -Detailed`
3. Review error messages and logs
4. Determine if failure is:
   - **Build error** → Fix code (compile errors, missing references)
   - **Test failure** → Test is failing (check test output)
   - **Infrastructure** → Retry or report

## Common Failure Patterns

| Pattern | Meaning |
|---------|---------|
| Build error in src/ | Code compilation failure |
| Test failure in tests/ | Unit test assertion failed |
| CodeQL alert | Security or quality issue detected |
| Timeout | Build or test took too long |

## Prerequisites

- `gh` (GitHub CLI) - authenticated with `syncfusion/maui-toolkit` access
