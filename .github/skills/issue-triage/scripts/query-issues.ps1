<#
.SYNOPSIS
    Queries open issues from syncfusion/maui-toolkit for triage.

.DESCRIPTION
    Fetches open issues that need triage decisions (no milestone, etc.)
    with optional filtering by platform and label.

.PARAMETER Platform
    Filter by platform (android, ios, windows, maccatalyst, all)

.PARAMETER Label
    Filter by specific label

.PARAMETER Limit
    Maximum number of issues to fetch (default: 50)

.PARAMETER Skip
    Skip first N issues for pagination (default: 0)

.PARAMETER OutputFormat
    Output format: table, json, or triage (default: table)

.EXAMPLE
    pwsh .github/skills/issue-triage/scripts/query-issues.ps1 -Limit 20
    pwsh .github/skills/issue-triage/scripts/query-issues.ps1 -Platform android -OutputFormat triage
#>

param(
    [ValidateSet("android", "ios", "windows", "maccatalyst", "all")]
    [string]$Platform = "all",

    [string]$Label = "",
    [int]$Limit = 50,
    [int]$Skip = 0,

    [ValidateSet("table", "json", "triage")]
    [string]$OutputFormat = "table"
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"

# Build search query
$searchQuery = "repo:$repo is:issue is:open no:milestone"

# Add label filter
if ($Label) {
    $searchQuery += " label:`"$Label`""
}

# Add platform filter (search in body since platform is in issue template)
if ($Platform -ne "all") {
    $platformMap = @{
        "android" = "Android"
        "ios" = "iOS"
        "windows" = "Windows"
        "maccatalyst" = "macOS"
    }
    $searchQuery += " `"$($platformMap[$Platform])`""
}

Write-Host "Query: $searchQuery"
Write-Host "Limit: $Limit | Skip: $Skip"
Write-Host ""

# Execute search
$jsonResult = gh api "search/issues?q=$([Uri]::EscapeDataString($searchQuery))&per_page=$Limit&page=$([Math]::Floor($Skip / [Math]::Max($Limit, 1)) + 1)&sort=created&order=desc" 2>$null

if (-not $jsonResult) {
    Write-Host "❌ Failed to query issues."
    exit 1
}

$parsed = $jsonResult | ConvertFrom-Json
$issues = $parsed.items
$totalCount = $parsed.total_count

Write-Host "Found $totalCount total issues (showing $($issues.Count))"
Write-Host ""

if ($issues.Count -eq 0) {
    Write-Host "No issues match the criteria."
    exit 0
}

switch ($OutputFormat) {
    "json" {
        $issues | ConvertTo-Json -Depth 5
    }
    "triage" {
        foreach ($issue in $issues) {
            $labels = ($issue.labels | ForEach-Object { $_.name }) -join ", "
            Write-Host "---"
            Write-Host "## Issue #$($issue.number)"
            Write-Host "**$($issue.title)**"
            Write-Host "🔗 $($issue.html_url)"
            Write-Host ""
            Write-Host "| Field | Value |"
            Write-Host "|-------|-------|"
            Write-Host "| Author | $($issue.user.login) |"
            Write-Host "| Labels | $labels |"
            Write-Host "| Comments | $($issue.comments) |"
            Write-Host "| Created | $($issue.created_at) |"
            Write-Host ""
        }
    }
    default {
        # Table format
        Write-Host ("{0,-8} {1,-60} {2,-20} {3,-10}" -f "Number", "Title", "Labels", "Comments")
        Write-Host ("{0,-8} {1,-60} {2,-20} {3,-10}" -f "------", "-----", "------", "--------")
        foreach ($issue in $issues) {
            $title = if ($issue.title.Length -gt 57) { $issue.title.Substring(0, 57) + "..." } else { $issue.title }
            $labels = ($issue.labels | ForEach-Object { $_.name }) -join ", "
            if ($labels.Length -gt 17) { $labels = $labels.Substring(0, 17) + "..." }
            Write-Host ("{0,-8} {1,-60} {2,-20} {3,-10}" -f $issue.number, $title, $labels, $issue.comments)
        }
    }
}
