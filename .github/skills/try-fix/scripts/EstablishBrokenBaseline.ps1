<#
.SYNOPSIS
    Establishes a broken baseline by reverting fix files to their merge-base state,
    preserving test files. Used by try-fix skill to start from a known broken state.

.DESCRIPTION
    Auto-detects fix files (non-test source files changed in the PR) and reverts them
    to the merge-base state with the specified base branch. Test files are preserved.

.PARAMETER BaseBranch
    The base branch to compare against (default: main)

.PARAMETER Restore
    Restore files to their current branch state (undo baseline)

.PARAMETER DryRun
    Show what would be reverted without making changes

.EXAMPLE
    pwsh .github/skills/try-fix/scripts/EstablishBrokenBaseline.ps1
    pwsh .github/skills/try-fix/scripts/EstablishBrokenBaseline.ps1 -Restore
    pwsh .github/skills/try-fix/scripts/EstablishBrokenBaseline.ps1 -DryRun
#>

param(
    [string]$BaseBranch = "main",
    [switch]$Restore,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

# Determine merge base
$mergeBase = git merge-base HEAD "origin/$BaseBranch" 2>$null
if (-not $mergeBase) {
    Write-Host "❌ Could not determine merge-base with origin/$BaseBranch"
    Write-Host "   Make sure you are on a PR branch, not $BaseBranch itself."
    exit 1
}

Write-Host "Merge base: $mergeBase"
Write-Host "Base branch: origin/$BaseBranch"

# Get all changed files
$changedFiles = git diff --name-only $mergeBase HEAD 2>$null
if (-not $changedFiles) {
    Write-Host "❌ No fix files detected. Are you on the correct PR branch?"
    exit 1
}

# Classify files into fix files and test files
$testPatterns = @(
    "*/tests/*",
    "*/UnitTest/*",
    "*Test.cs",
    "*Tests.cs",
    "*Test.csproj"
)

$fixFiles = @()
$testFiles = @()

foreach ($file in $changedFiles) {
    $isTest = $false
    foreach ($pattern in $testPatterns) {
        if ($file -like $pattern) {
            $isTest = $true
            break
        }
    }
    if ($isTest) {
        $testFiles += $file
    } else {
        $fixFiles += $file
    }
}

if ($fixFiles.Count -eq 0) {
    Write-Host "❌ No fix files detected (only test files changed)."
    Write-Host "   Changed files:"
    $changedFiles | ForEach-Object { Write-Host "     $_" }
    exit 1
}

Write-Host ""
Write-Host "=== File Classification ==="
Write-Host "Fix files ($($fixFiles.Count)):"
$fixFiles | ForEach-Object { Write-Host "  📝 $_" }
Write-Host ""
Write-Host "Test files ($($testFiles.Count)) - PRESERVED:"
$testFiles | ForEach-Object { Write-Host "  🧪 $_" }
Write-Host ""

if ($Restore) {
    Write-Host "=== Restoring files to branch state ==="
    foreach ($file in $fixFiles) {
        if ($DryRun) {
            Write-Host "  [DryRun] Would restore: $file"
        } else {
            git checkout HEAD -- $file 2>$null
            Write-Host "  ✅ Restored: $file"
        }
    }
    Write-Host ""
    Write-Host "Baseline restored ✅"
    exit 0
}

# Revert fix files to merge-base state
Write-Host "=== Reverting fix files to merge-base state ==="
foreach ($file in $fixFiles) {
    if ($DryRun) {
        Write-Host "  [DryRun] Would revert: $file"
    } else {
        # Check if file existed at merge-base
        $existsAtBase = git show "${mergeBase}:${file}" 2>$null
        if ($LASTEXITCODE -eq 0) {
            git checkout $mergeBase -- $file 2>$null
            Write-Host "  ✅ Reverted: $file"
        } else {
            # File was added in PR - remove it
            if (Test-Path $file) {
                Remove-Item $file -Force
                Write-Host "  🗑️ Removed (new file): $file"
            }
        }
    }
}

Write-Host ""
Write-Host "╔═══════════════════════════════════════════╗"
Write-Host "║     Baseline established ✅               ║"
Write-Host "║     Fix files reverted to merge-base      ║"
Write-Host "║     Test files preserved                  ║"
Write-Host "╚═══════════════════════════════════════════╝"
