<#
.SYNOPSIS
    Posts or updates the AI Summary comment on a GitHub PR.

.PARAMETER PRNumber
    Pull request number

.PARAMETER StateFile
    Path to state file (PRNumber auto-extracted from pr-XXXXX.md naming)

.PARAMETER Content
    Full state file content (legacy)

.PARAMETER DryRun
    Preview changes instead of posting

.EXAMPLE
    pwsh .github/skills/ai-summary-comment/scripts/post-ai-summary-comment.ps1 -PRNumber 123
#>

param(
    [int]$PRNumber = 0,
    [string]$StateFile = "",
    [string]$Content = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"
$marker = "<!-- AI Summary -->"
$sectionStart = "<!-- SECTION:PR-REVIEW -->"
$sectionEnd = "<!-- /SECTION:PR-REVIEW -->"

# Resolve PRNumber and content
if ($StateFile -and (Test-Path $StateFile)) {
    $Content = Get-Content $StateFile -Raw
    if ($StateFile -match 'pr-(\d+)\.md') {
        $PRNumber = [int]$Matches[1]
    }
} elseif ($PRNumber -gt 0 -and -not $Content) {
    $autoPath = "CustomAgentLogsTmp/PRState/pr-$PRNumber.md"
    if (Test-Path $autoPath) {
        $Content = Get-Content $autoPath -Raw
        Write-Host "Auto-loaded state file: $autoPath"
    } else {
        Write-Host "❌ No state file found at $autoPath"
        exit 1
    }
}

if ($PRNumber -eq 0) {
    Write-Host "❌ PRNumber is required."
    exit 1
}

# Build section content
$sectionContent = @"
$sectionStart
## 🤖 PR Review Summary

$Content

---
*Updated by Bug Fix Agent @ $(Get-Date -Format "yyyy-MM-dd HH:mm:ss") UTC*
$sectionEnd
"@

# Build full comment body
$commentBody = @"
$marker

## 🤖 AI Summary

$sectionContent
"@

if ($DryRun) {
    $previewDir = "CustomAgentLogsTmp/PRState/$PRNumber"
    New-Item -ItemType Directory -Path $previewDir -Force | Out-Null
    $previewFile = "$previewDir/ai-summary-comment-preview.md"
    $commentBody | Set-Content $previewFile
    Write-Host "✅ [DryRun] Preview saved to: $previewFile"
    Write-Host ""
    Write-Host $commentBody
    exit 0
}

# Check for existing comment
$existingComment = gh api "repos/$repo/issues/$PRNumber/comments" --jq ".[] | select(.body | contains(`"$marker`")) | .id" 2>$null | Select-Object -First 1

if ($existingComment) {
    # Update existing comment
    $escapedBody = $commentBody | ConvertTo-Json
    gh api "repos/$repo/issues/comments/$existingComment" -X PATCH -f body="$commentBody" 2>$null
    Write-Host "✅ Updated existing AI Summary comment on PR #$PRNumber"
} else {
    # Create new comment
    gh api "repos/$repo/issues/$PRNumber/comments" -f body="$commentBody" 2>$null
    Write-Host "✅ Created AI Summary comment on PR #$PRNumber"
}
