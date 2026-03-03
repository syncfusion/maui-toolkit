<#
.SYNOPSIS
    Initializes a triage session by fetching current milestones and labels.

.DESCRIPTION
    Queries the syncfusion/maui-toolkit repository for active milestones
    and displays them for reference during triage.

.EXAMPLE
    pwsh .github/skills/issue-triage/scripts/init-triage-session.ps1
#>

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"

# Check gh CLI
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "❌ GitHub CLI (gh) not found."
    Write-Host "   Install: brew install gh"
    Write-Host "   Auth:    gh auth login"
    exit 1
}

# Check authentication
$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ GitHub CLI not authenticated."
    Write-Host "   Run: gh auth login"
    exit 1
}

Write-Host "=== Triage Session Initialization ==="
Write-Host "Repository: $repo"
Write-Host ""

# Fetch milestones
Write-Host "Fetching milestones..."
$milestones = gh api "repos/$repo/milestones?state=open&per_page=100" --jq '.[] | "\(.title) [\(.open_issues) open]"' 2>$null

if ($milestones) {
    Write-Host ""
    Write-Host "Active Milestones:"
    $milestones | ForEach-Object { Write-Host "  - $_" }
} else {
    Write-Host "  No open milestones found."
}

Write-Host ""

# Fetch label categories
Write-Host "Common Labels:"
$labels = gh api "repos/$repo/labels?per_page=100" --jq '.[].name' 2>$null
if ($labels) {
    $bugLabels = $labels | Where-Object { $_ -match "^t/" -or $_ -match "bug" }
    $priorityLabels = $labels | Where-Object { $_ -match "^p/" }
    $platformLabels = $labels | Where-Object { $_ -match "platform|ios|android|windows|mac" }

    if ($bugLabels) {
        Write-Host "  Type: $($bugLabels -join ', ')"
    }
    if ($priorityLabels) {
        Write-Host "  Priority: $($priorityLabels -join ', ')"
    }
    if ($platformLabels) {
        Write-Host "  Platform: $($platformLabels -join ', ')"
    }
}

# Create session tracking
$sessionDir = "CustomAgentLogsTmp/Triage"
New-Item -ItemType Directory -Path $sessionDir -Force | Out-Null

$sessionFile = "$sessionDir/triage-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
@{
    StartTime = (Get-Date -Format "o")
    Repository = $repo
    Stats = @{
        Triaged = 0
        Skipped = 0
    }
} | ConvertTo-Json | Set-Content $sessionFile

Write-Host ""
Write-Host "✅ Session initialized: $sessionFile"
Write-Host ""
Write-Host "Next: Run query-issues.ps1 to load issues for triage."
