<#
.SYNOPSIS
    Retrieves CI build/check status for a GitHub Pull Request.

.DESCRIPTION
    Queries GitHub Actions check runs for a PR and displays their status,
    with optional filtering for failed checks and detailed log output.

.PARAMETER PrNumber
    The pull request number to check.

.PARAMETER FailedOnly
    Show only failed checks.

.PARAMETER Detailed
    Include log output for failed checks.

.EXAMPLE
    pwsh .github/skills/pr-build-status/scripts/Get-PrBuildStatus.ps1 -PrNumber 42
    pwsh .github/skills/pr-build-status/scripts/Get-PrBuildStatus.ps1 -PrNumber 42 -FailedOnly -Detailed
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$PrNumber,

    [switch]$FailedOnly,
    [switch]$Detailed
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"

# Check gh CLI
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "❌ GitHub CLI (gh) not found."
    Write-Host "   Install: brew install gh"
    exit 1
}

Write-Host "=== PR #$PrNumber Build Status ==="
Write-Host "Repository: $repo"
Write-Host ""

# Get PR head SHA
$prInfo = gh pr view $PrNumber --repo $repo --json headRefOid,statusCheckRollup,title 2>$null | ConvertFrom-Json

if (-not $prInfo) {
    Write-Host "❌ Could not fetch PR #$PrNumber"
    exit 1
}

Write-Host "PR: $($prInfo.title)"
Write-Host "HEAD: $($prInfo.headRefOid.Substring(0, 8))"
Write-Host ""

# Get check runs
$checks = $prInfo.statusCheckRollup

if (-not $checks -or $checks.Count -eq 0) {
    Write-Host "No checks found for this PR."
    exit 0
}

# Filter if needed
if ($FailedOnly) {
    $checks = $checks | Where-Object {
        $_.conclusion -eq "FAILURE" -or $_.conclusion -eq "failure" -or
        $_.status -eq "FAILURE" -or $_.state -eq "FAILURE"
    }
    if ($checks.Count -eq 0) {
        Write-Host "✅ No failed checks!"
        exit 0
    }
}

# Display results
Write-Host ("{0,-40} {1,-12} {2,-15}" -f "Check", "Status", "Conclusion")
Write-Host ("{0,-40} {1,-12} {2,-15}" -f "-----", "------", "----------")

foreach ($check in $checks) {
    $name = $check.name
    if (-not $name) { $name = $check.context }
    if ($name -and $name.Length -gt 37) { $name = $name.Substring(0, 37) + "..." }

    $status = if ($check.status) { $check.status } elseif ($check.state) { $check.state } else { "unknown" }
    $conclusion = if ($check.conclusion) { $check.conclusion } else { "pending" }

    $icon = switch ($conclusion.ToLower()) {
        "success" { "✅" }
        "failure" { "❌" }
        "neutral" { "⚪" }
        "pending" { "⏳" }
        default { "❓" }
    }

    Write-Host ("{0,-40} {1,-12} {2} {3}" -f $name, $status, $icon, $conclusion)
}

# Summary
$total = $checks.Count
$failed = ($checks | Where-Object { $_.conclusion -eq "FAILURE" -or $_.conclusion -eq "failure" }).Count
$passed = ($checks | Where-Object { $_.conclusion -eq "SUCCESS" -or $_.conclusion -eq "success" }).Count
$pending = $total - $failed - $passed

Write-Host ""
Write-Host "Summary: $passed passed, $failed failed, $pending pending (total: $total)"

if ($failed -gt 0) {
    Write-Host ""
    Write-Host "⚠️ There are $failed failed check(s). Use -FailedOnly -Detailed for more info."
}

# Detailed output for failed checks
if ($Detailed -and $FailedOnly) {
    Write-Host ""
    Write-Host "=== Detailed Failure Information ==="
    foreach ($check in $checks) {
        $name = if ($check.name) { $check.name } else { $check.context }
        Write-Host ""
        Write-Host "--- $name ---"
        if ($check.detailsUrl) {
            Write-Host "Details: $($check.detailsUrl)"
        }
        if ($check.targetUrl) {
            Write-Host "URL: $($check.targetUrl)"
        }
    }
}
