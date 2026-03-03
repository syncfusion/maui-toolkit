<#
.SYNOPSIS
    Verifies that tests FAIL without the fix and PASS with the fix.

.DESCRIPTION
    Two-phase verification:
    Phase 1: Revert fix files → run tests → expect FAIL (bug reproduced)
    Phase 2: Restore fix files → run tests → expect PASS (bug resolved)

.PARAMETER TestFilter
    xUnit test filter expression (e.g., "FullyQualifiedName~Issue12345")

.PARAMETER BaseBranch
    Base branch to compare against (default: main)

.PARAMETER RequireFullVerification
    Require both phases to pass (default: true)

.PARAMETER OutputDir
    Directory for output files (default: auto-detected from PR number)

.EXAMPLE
    pwsh .github/skills/verify-tests-fail-without-fix/scripts/verify-tests-fail.ps1 -TestFilter "FullyQualifiedName~Issue12345"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$TestFilter,

    [string]$BaseBranch = "main",
    [switch]$RequireFullVerification = $true,
    [string]$OutputDir = ""
)

$ErrorActionPreference = "Stop"
$startTime = Get-Date

# --- Setup ---
$testProject = "maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj"

if (-not (Test-Path $testProject)) {
    Write-Host "❌ Test project not found: $testProject"
    Write-Host "   Make sure you're running from the repository root."
    exit 1
}

# Determine output directory
if (-not $OutputDir) {
    # Try to extract PR/issue number from branch name
    $branchName = git branch --show-current 2>$null
    $prNumber = ""
    if ($branchName -match '(\d+)') {
        $prNumber = $Matches[1]
    }
    if ($prNumber) {
        $OutputDir = "CustomAgentLogsTmp/PRState/$prNumber/verify-tests-fail"
    } else {
        $OutputDir = "CustomAgentLogsTmp/verify-tests-fail"
    }
}

New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
Write-Host "Output directory: $OutputDir"

# Determine merge base
$mergeBase = git merge-base HEAD "origin/$BaseBranch" 2>$null
if (-not $mergeBase) {
    Write-Host "❌ Could not determine merge-base with origin/$BaseBranch"
    exit 1
}

# Get fix files (non-test files changed in PR)
$changedFiles = git diff --name-only $mergeBase HEAD 2>$null
$testPatterns = @("*/tests/*", "*/UnitTest/*", "*Test.cs", "*Tests.cs")
$fixFiles = @()
foreach ($file in $changedFiles) {
    $isTest = $false
    foreach ($pattern in $testPatterns) {
        if ($file -like $pattern) { $isTest = $true; break }
    }
    if (-not $isTest) { $fixFiles += $file }
}

if ($fixFiles.Count -eq 0) {
    Write-Host "❌ No fix files found to revert."
    exit 1
}

Write-Host ""
Write-Host "Fix files to toggle:"
$fixFiles | ForEach-Object { Write-Host "  📝 $_" }
Write-Host ""
Write-Host "Test filter: $TestFilter"
Write-Host ""

# --- Phase 1: Test WITHOUT fix (expect FAIL) ---
Write-Host "╔═══════════════════════════════════════════════════════════╗"
Write-Host "║  Phase 1: Testing WITHOUT fix (expect FAIL)              ║"
Write-Host "╚═══════════════════════════════════════════════════════════╝"
Write-Host ""

# Revert fix files to merge-base
foreach ($file in $fixFiles) {
    $existsAtBase = git show "${mergeBase}:${file}" 2>$null
    if ($LASTEXITCODE -eq 0) {
        git checkout $mergeBase -- $file 2>$null
        Write-Host "  Reverted: $file"
    } else {
        if (Test-Path $file) {
            Remove-Item $file -Force
            Write-Host "  Removed (new file): $file"
        }
    }
}

Write-Host ""
Write-Host "Running tests without fix..."
$testWithoutFixLog = "$OutputDir/test-without-fix.log"

$testResult1 = $null
try {
    dotnet test $testProject --filter $TestFilter --no-restore -v normal 2>&1 | Tee-Object -FilePath $testWithoutFixLog
    $testResult1 = $LASTEXITCODE
} catch {
    $testResult1 = 1
}

$phase1Pass = ($testResult1 -ne 0)  # We WANT failure here
Write-Host ""
if ($phase1Pass) {
    Write-Host "  ✅ Phase 1: Tests FAILED without fix (as expected)"
} else {
    Write-Host "  ❌ Phase 1: Tests PASSED without fix (unexpected!)"
    Write-Host "     Tests don't catch the bug - they need to be fixed."
}

# --- Phase 2: Test WITH fix (expect PASS) ---
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════╗"
Write-Host "║  Phase 2: Testing WITH fix (expect PASS)                 ║"
Write-Host "╚═══════════════════════════════════════════════════════════╝"
Write-Host ""

# Restore fix files
foreach ($file in $fixFiles) {
    git checkout HEAD -- $file 2>$null
    Write-Host "  Restored: $file"
}

Write-Host ""
Write-Host "Running tests with fix..."
$testWithFixLog = "$OutputDir/test-with-fix.log"

$testResult2 = $null
try {
    dotnet test $testProject --filter $TestFilter --no-restore -v normal 2>&1 | Tee-Object -FilePath $testWithFixLog
    $testResult2 = $LASTEXITCODE
} catch {
    $testResult2 = 1
}

$phase2Pass = ($testResult2 -eq 0)  # We WANT success here
Write-Host ""
if ($phase2Pass) {
    Write-Host "  ✅ Phase 2: Tests PASSED with fix (as expected)"
} else {
    Write-Host "  ❌ Phase 2: Tests FAILED with fix (unexpected!)"
    Write-Host "     The fix doesn't resolve the issue."
}

# --- Final Result ---
$endTime = Get-Date
$duration = $endTime - $startTime
$overallPass = $phase1Pass -and $phase2Pass

Write-Host ""
if ($overallPass) {
    Write-Host "╔═══════════════════════════════════════════════════════════╗"
    Write-Host "║              VERIFICATION PASSED ✅                       ║"
    Write-Host "╠═══════════════════════════════════════════════════════════╣"
    Write-Host "║  - FAIL without fix (as expected)                         ║"
    Write-Host "║  - PASS with fix (as expected)                            ║"
    Write-Host "╚═══════════════════════════════════════════════════════════╝"
} else {
    Write-Host "╔═══════════════════════════════════════════════════════════╗"
    Write-Host "║              VERIFICATION FAILED ❌                       ║"
    Write-Host "╠═══════════════════════════════════════════════════════════╣"
    if (-not $phase1Pass) {
        Write-Host "║  - Tests PASSED without fix (should FAIL)                 ║"
    } else {
        Write-Host "║  - FAIL without fix ✅                                    ║"
    }
    if (-not $phase2Pass) {
        Write-Host "║  - Tests FAILED with fix (should PASS)                    ║"
    } else {
        Write-Host "║  - PASS with fix ✅                                       ║"
    }
    Write-Host "╚═══════════════════════════════════════════════════════════╝"
}

Write-Host ""
Write-Host "Duration: $($duration.TotalSeconds.ToString('F1'))s"

# --- Generate Report ---
$status = if ($overallPass) { "Passed" } else { "Failed" }
$report = @"
# Verification Report

**Status:** $status
**Test Filter:** $TestFilter
**Duration:** $($duration.TotalSeconds.ToString('F1'))s
**Date:** $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")

## Results

| Phase | Expected | Actual | Status |
|-------|----------|--------|--------|
| Without fix | FAIL | $(if ($phase1Pass) { "FAIL" } else { "PASS" }) | $(if ($phase1Pass) { "✅" } else { "❌" }) |
| With fix | PASS | $(if ($phase2Pass) { "PASS" } else { "FAIL" }) | $(if ($phase2Pass) { "✅" } else { "❌" }) |

## Fix Files Tested

$($fixFiles | ForEach-Object { "- ``$_``" } | Out-String)
"@

$report | Set-Content "$OutputDir/verification-report.md"
Write-Host "Report saved to: $OutputDir/verification-report.md"

if (-not $overallPass -and $RequireFullVerification) {
    exit 1
}
