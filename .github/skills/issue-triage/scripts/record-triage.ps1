<#
.SYNOPSIS
    Records a triage decision for tracking.

.PARAMETER IssueNumber
    The issue number that was triaged.

.PARAMETER Milestone
    The milestone assigned.

.PARAMETER Labels
    Additional labels applied (comma-separated).

.EXAMPLE
    pwsh .github/skills/issue-triage/scripts/record-triage.ps1 -IssueNumber 123 -Milestone "Backlog"
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$IssueNumber,

    [string]$Milestone = "",
    [string]$Labels = "",
    [string]$Action = "triaged"
)

$sessionDir = "CustomAgentLogsTmp/Triage"
$sessionFiles = Get-ChildItem "$sessionDir/triage-*.json" -ErrorAction SilentlyContinue | Sort-Object Name -Descending

if ($sessionFiles.Count -eq 0) {
    Write-Host "⚠️ No active triage session. Run init-triage-session.ps1 first."
    exit 0
}

$sessionFile = $sessionFiles[0].FullName
$session = Get-Content $sessionFile | ConvertFrom-Json

# Update stats
if ($Action -eq "skipped") {
    $session.Stats.Skipped++
} else {
    $session.Stats.Triaged++
}

$session | ConvertTo-Json -Depth 3 | Set-Content $sessionFile

Write-Host "✅ Recorded: Issue #$IssueNumber → $Action$(if ($Milestone) { " (Milestone: $Milestone)" })$(if ($Labels) { " (Labels: $Labels)" })"
Write-Host "   Session stats: $($session.Stats.Triaged) triaged, $($session.Stats.Skipped) skipped"
