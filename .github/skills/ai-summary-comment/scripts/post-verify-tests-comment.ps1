<#
.SYNOPSIS
    Posts or updates test verification results in the AI Summary comment.

.PARAMETER PRNumber
    Pull request number

.PARAMETER ReportFile
    Path to verification report (auto-discovered if not provided)

.PARAMETER DryRun
    Preview changes instead of posting

.EXAMPLE
    pwsh .github/skills/ai-summary-comment/scripts/post-verify-tests-comment.ps1 -PRNumber 123
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$PRNumber,

    [string]$ReportFile = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"
$marker = "<!-- AI Summary -->"
$sectionStart = "<!-- SECTION:VERIFY-TESTS -->"
$sectionEnd = "<!-- /SECTION:VERIFY-TESTS -->"

# Auto-discover report
if (-not $ReportFile) {
    $ReportFile = "CustomAgentLogsTmp/PRState/$PRNumber/verify-tests-fail/verification-report.md"
}

if (-not (Test-Path $ReportFile)) {
    Write-Host "❌ Verification report not found: $ReportFile"
    exit 1
}

$reportContent = Get-Content $ReportFile -Raw

# Detect status
$status = if ($reportContent -match "Status:\*\*\s*(Passed|Failed)") { $Matches[1] } else { "Unknown" }
$statusIcon = if ($status -eq "Passed") { "✅" } else { "❌" }

$sectionContent = @"
$sectionStart
## 🧪 Test Verification $statusIcon

$reportContent

---
*Verified by Bug Fix Agent @ $(Get-Date -Format "yyyy-MM-dd HH:mm:ss") UTC*
$sectionEnd
"@

if ($DryRun) {
    $previewDir = "CustomAgentLogsTmp/PRState/$PRNumber"
    New-Item -ItemType Directory -Path $previewDir -Force | Out-Null
    $previewFile = "$previewDir/ai-summary-comment-preview.md"

    if (Test-Path $previewFile) {
        $existing = Get-Content $previewFile -Raw
        if ($existing -match [regex]::Escape($sectionStart)) {
            $existing = $existing -replace "(?s)$([regex]::Escape($sectionStart)).*?$([regex]::Escape($sectionEnd))", $sectionContent
        } else {
            $existing += "`n`n$sectionContent"
        }
        $existing | Set-Content $previewFile
    } else {
        $sectionContent | Set-Content $previewFile
    }
    Write-Host "✅ [DryRun] Preview updated: $previewFile"
    exit 0
}

# Find and update existing comment
$existingComment = gh api "repos/$repo/issues/$PRNumber/comments" --jq ".[] | select(.body | contains(`"$marker`")) | {id: .id, body: .body}" 2>$null | ConvertFrom-Json

if ($existingComment) {
    $body = $existingComment.body
    if ($body -match [regex]::Escape($sectionStart)) {
        $body = $body -replace "(?s)$([regex]::Escape($sectionStart)).*?$([regex]::Escape($sectionEnd))", $sectionContent
    } else {
        $body += "`n`n$sectionContent"
    }
    gh api "repos/$repo/issues/comments/$($existingComment.id)" -X PATCH -f body="$body" 2>$null
    Write-Host "✅ Updated VERIFY-TESTS section in AI Summary comment"
} else {
    $newBody = "$marker`n`n## 🤖 AI Summary`n`n$sectionContent"
    gh api "repos/$repo/issues/$PRNumber/comments" -f body="$newBody" 2>$null
    Write-Host "✅ Created AI Summary comment with VERIFY-TESTS section"
}
