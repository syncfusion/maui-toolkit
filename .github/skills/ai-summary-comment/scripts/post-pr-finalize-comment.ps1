<#
.SYNOPSIS
    Posts a separate PR finalization comment.

.PARAMETER PRNumber
    Pull request number

.PARAMETER TitleStatus
    "Good" or "NeedsUpdate"

.PARAMETER CurrentTitle
    Current PR title (auto-fetched if not provided)

.PARAMETER RecommendedTitle
    Recommended title (required if TitleStatus is NeedsUpdate)

.PARAMETER DescriptionStatus
    "Excellent", "Good", "NeedsUpdate", or "NeedsRewrite"

.PARAMETER DescriptionAssessment
    Assessment of description quality

.PARAMETER CodeReviewStatus
    "Passed", "IssuesFound", or "Skipped"

.PARAMETER CodeReviewFindings
    Markdown content for code review section

.PARAMETER DryRun
    Preview instead of posting

.EXAMPLE
    pwsh .github/skills/ai-summary-comment/scripts/post-pr-finalize-comment.ps1 -PRNumber 123 -TitleStatus "Good" -DescriptionStatus "Good"
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$PRNumber,

    [string]$TitleStatus = "Good",
    [string]$CurrentTitle = "",
    [string]$RecommendedTitle = "",
    [string]$DescriptionStatus = "Good",
    [string]$DescriptionAssessment = "",
    [string]$MissingElements = "",
    [string]$RecommendedDescription = "",
    [string]$CodeReviewStatus = "Skipped",
    [string]$CodeReviewFindings = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"
$repo = "syncfusion/maui-toolkit"
$marker = "<!-- PR-FINALIZE-COMMENT -->"

# Auto-fetch title if not provided
if (-not $CurrentTitle) {
    $CurrentTitle = gh pr view $PRNumber --repo $repo --json title --jq '.title' 2>$null
}

# Build title section
$titleIcon = if ($TitleStatus -eq "Good") { "✅" } else { "⚠️" }
$titleSection = @"
### $titleIcon Title: $TitleStatus
**Current:** "$CurrentTitle"
$(if ($RecommendedTitle) { "**Recommended:** `"$RecommendedTitle`"" })
"@

# Build description section
$descIcon = switch ($DescriptionStatus) {
    "Excellent" { "✅" }
    "Good" { "✅" }
    default { "⚠️" }
}
$descSection = @"
### $descIcon Description: $DescriptionStatus
$(if ($DescriptionAssessment) { $DescriptionAssessment })
$(if ($MissingElements) { "`n**Missing:** $MissingElements" })
"@

# Build code review section
$codeSection = ""
if ($CodeReviewStatus -ne "Skipped") {
    $codeIcon = if ($CodeReviewStatus -eq "Passed") { "✅" } else { "⚠️" }
    $codeSection = @"

### $codeIcon Code Review: $CodeReviewStatus
$(if ($CodeReviewFindings) { $CodeReviewFindings })
"@
}

# Build full comment
$commentBody = @"
$marker

## 📋 PR Finalization Review

$titleSection

$descSection
$codeSection

---
*Reviewed by Bug Fix Agent @ $(Get-Date -Format "yyyy-MM-dd HH:mm:ss") UTC*
"@

if ($DryRun) {
    Write-Host "✅ [DryRun] PR Finalize comment preview:"
    Write-Host ""
    Write-Host $commentBody
    exit 0
}

# Check for existing finalize comment
$existingComment = gh api "repos/$repo/issues/$PRNumber/comments" --jq ".[] | select(.body | contains(`"$marker`")) | .id" 2>$null | Select-Object -First 1

if ($existingComment) {
    gh api "repos/$repo/issues/comments/$existingComment" -X PATCH -f body="$commentBody" 2>$null
    Write-Host "✅ Updated PR Finalize comment on PR #$PRNumber"
} else {
    gh api "repos/$repo/issues/$PRNumber/comments" -f body="$commentBody" 2>$null
    Write-Host "✅ Created PR Finalize comment on PR #$PRNumber"
}
