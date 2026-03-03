<#
.SYNOPSIS
    Posts or updates try-fix attempt results in the AI Summary comment.

.PARAMETER IssueNumber
    Issue or PR number

.PARAMETER TryFixDir
    Path to specific try-fix attempt directory (auto-loads parameters)

.PARAMETER DryRun
    Preview changes instead of posting

.EXAMPLE
    pwsh .github/skills/ai-summary-comment/scripts/post-try-fix-comment.ps1 -IssueNumber 123
    pwsh .github/skills/ai-summary-comment/scripts/post-try-fix-comment.ps1 -TryFixDir CustomAgentLogsTmp/PRState/123/try-fix/attempt-1
#>

param(
    [int]$IssueNumber = 0,
    [string]$TryFixDir = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"
$marker = "<!-- AI Summary -->"
$sectionStart = "<!-- SECTION:TRY-FIX -->"
$sectionEnd = "<!-- /SECTION:TRY-FIX -->"

# Auto-discover attempts
if ($TryFixDir -and (Test-Path $TryFixDir)) {
    if ($TryFixDir -match '(\d+)[/\\]try-fix') {
        $IssueNumber = [int]$Matches[1]
    }
}

if ($IssueNumber -eq 0) {
    Write-Host "❌ IssueNumber is required."
    exit 1
}

$tryFixBase = "CustomAgentLogsTmp/PRState/$IssueNumber/try-fix"
$attempts = Get-ChildItem "$tryFixBase/attempt-*" -Directory -ErrorAction SilentlyContinue | Sort-Object Name

if ($attempts.Count -eq 0) {
    Write-Host "❌ No try-fix attempts found in $tryFixBase"
    exit 1
}

# Build section content from all attempts
$attemptSections = @()
foreach ($attempt in $attempts) {
    $num = if ($attempt.Name -match 'attempt-(\d+)') { $Matches[1] } else { "?" }
    $approach = if (Test-Path "$($attempt.FullName)/approach.md") { Get-Content "$($attempt.FullName)/approach.md" -Raw } else { "No approach documented" }
    $result = if (Test-Path "$($attempt.FullName)/result.txt") { (Get-Content "$($attempt.FullName)/result.txt" -Raw).Trim() } else { "Unknown" }
    $analysis = if (Test-Path "$($attempt.FullName)/analysis.md") { Get-Content "$($attempt.FullName)/analysis.md" -Raw } else { "" }

    $statusIcon = switch ($result) {
        "Pass" { "✅" }
        "Fail" { "❌" }
        "Blocked" { "⚠️" }
        default { "❓" }
    }

    $attemptSections += @"

<details>
<summary><strong>🔧 Attempt #$num</strong> $statusIcon $result</summary>

$approach

**Result:** $statusIcon $result

$(if ($analysis) { "**Analysis:**`n$analysis" })

</details>
"@
}

$sectionContent = @"
$sectionStart
## 🔧 Try-Fix Attempts for Issue #$IssueNumber

$($attemptSections -join "`n`n---`n")

---
*Updated by Bug Fix Agent @ $(Get-Date -Format "yyyy-MM-dd HH:mm:ss") UTC*
$sectionEnd
"@

if ($DryRun) {
    $previewDir = "CustomAgentLogsTmp/PRState/$IssueNumber"
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

# Find and update existing comment or create section in existing AI Summary
$existingComment = gh api "repos/$repo/issues/$IssueNumber/comments" --jq ".[] | select(.body | contains(`"$marker`")) | {id: .id, body: .body}" 2>$null | ConvertFrom-Json

if ($existingComment) {
    $body = $existingComment.body
    if ($body -match [regex]::Escape($sectionStart)) {
        $body = $body -replace "(?s)$([regex]::Escape($sectionStart)).*?$([regex]::Escape($sectionEnd))", $sectionContent
    } else {
        $body += "`n`n$sectionContent"
    }
    gh api "repos/$repo/issues/comments/$($existingComment.id)" -X PATCH -f body="$body" 2>$null
    Write-Host "✅ Updated TRY-FIX section in AI Summary comment"
} else {
    $newBody = "$marker`n`n## 🤖 AI Summary`n`n$sectionContent"
    gh api "repos/$repo/issues/$IssueNumber/comments" -f body="$newBody" 2>$null
    Write-Host "✅ Created AI Summary comment with TRY-FIX section"
}
