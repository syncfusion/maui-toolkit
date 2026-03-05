---
description: "Guidance for GitHub Copilot when working on the Syncfusion Toolkit for .NET MAUI repository."
---

# GitHub Copilot Development Environment Instructions

This document provides specific guidance for GitHub Copilot when working on the Syncfusion Toolkit for .NET MAUI repository. It serves as context for understanding the project structure, development workflow, and best practices.

## Code Review Instructions

When performing a code review on PRs that change functional code, verify that the PR title and description accurately match the actual implementation and reference the correct issue number(s) in the `Fixes #` field.

## Repository Overview

**Syncfusion Toolkit for .NET MAUI** is an open-source UI controls library built on top of .NET MAUI, providing rich, cross-platform UI controls and components for Android, iOS, macOS, and Windows applications.

### Key Technologies

- **.NET MAUI** — Cross-platform UI framework (Android, iOS, macOS, Windows)
- **.NET SDK** — Version `10.0.x` (see `pipelines/build.yml`)
- **MSBuild** — Solution: `Syncfusion.Maui.Toolkit.sln` at repository root
- **xUnit** — Unit testing framework (`Syncfusion.Maui.Toolkit.UnitTest`)
- **C# with nullable reference types** enabled globally (`Directory.Build.props`)

### Repository Source

- **GitHub**: `https://github.com/syncfusion/maui-toolkit`
- **Primary branch**: `main` (all PRs target `main`)

---

## Development Environment Setup

### Prerequisites

- **.NET 10 SDK** — `dotnet --version` should return `10.x`
- **Visual Studio 17.10+** (Windows) or **VS Code + .NET MAUI Dev Kit** (macOS)
- **Android SDK** — Install via Visual Studio or Android SDK Manager
- **OpenJDK 17** — Required for Android targets
- **Xcode** (macOS only) — Required for iOS and MacCatalyst

### Initial Setup

```bash
# Restore tools and build
dotnet tool restore
dotnet build ./Syncfusion.Maui.Toolkit.sln
```

### Platform Requirements

| Platform | Host | Requirements |
|----------|------|-------------|
| Android | Windows / macOS | OpenJDK 17 + Android SDK |
| iOS / MacCatalyst | macOS | Xcode (current stable) |
| Windows | Windows | Windows SDK |

---

## Project Structure

### Repository Root

```
maui-toolkit/
├── maui/
│   ├── src/           ← All control source code
│   ├── tests/         ← Unit tests
│   └── samples/       ← Sample gallery and sandbox
├── samples/           ← Top-level sample apps
├── pipelines/         ← Azure DevOps CI pipelines
├── targets/           ← MSBuild targets
├── .github/           ← GitHub workflows, templates, agents
├── Directory.Build.props
└── Syncfusion.Maui.Toolkit.sln
```

### Source Controls (`maui/src/`)

Each control lives in its own folder under `maui/src/`:

| Control | Path |
|---------|------|
| Charts (Cartesian, Circular, Funnel, Pyramid) | `maui/src/Charts/` |
| Spark Charts | `maui/src/SparkCharts/` |
| Sunburst Chart | `maui/src/SunburstChart/` |
| Calendar | `maui/src/Calendar/` |
| Popup | `maui/src/Popup/` |
| Bottom Sheet | `maui/src/BottomSheet/` |
| Tab View | `maui/src/TabView/` |
| Accordion / Expander | `maui/src/Accordion/`, `maui/src/Expander/` |
| Navigation Drawer | `maui/src/NavigationDrawer/` |
| Carousel | `maui/src/Carousel/` |
| Cards | `maui/src/Cards/` |
| Chip | `maui/src/Chip/` |
| Segmented Control | `maui/src/SegmentedControl/` |
| Button | `maui/src/Button/` |
| Numeric Entry | `maui/src/NumericEntry/` |
| OTP Input | `maui/src/OtpInput/` |
| Text Input Layout | `maui/src/TextInputLayout/` |
| Picker | `maui/src/Picker/` |
| Pull to Refresh | `maui/src/PullToRefresh/` |
| Progress Bar | `maui/src/ProgressBar/` |
| Shimmer | `maui/src/Shimmer/` |
| Effects View | `maui/src/EffectsView/` |
| Core (shared utilities) | `maui/src/Core/` |

### Core Infrastructure (`maui/src/Core/`)

Shared utilities used across all controls:

- `Animation/` — Animation helpers
- `BaseView/` — Base view classes (`SfView`)
- `ButtonBase/` — Button base implementation
- `DrawableView/` — Custom drawable canvas view
- `EntryView/` — Entry-based view utilities
- `Extensions/` — Extension methods
- `FontElement/` — Font-related bindable properties
- `GestureDetector/` — Cross-platform gesture support
- `Helper/` — General helper utilities
- `KeyboardDetector/` — Keyboard event handling
- `Legend/` — Shared legend for chart controls
- `Localization/` — Localization resources
- `PickerView/` — Base picker implementation
- `TabView/` — Tab view base
- `TextMeasurer/` — Text size measurement
- `TextStyle/` — Text styling utilities
- `Theme/` — Light/dark theme support
- `Tooltip/` — Tooltip implementation
- `TouchDetector/` — Touch event detection
- `ViewExtensions/` — View helper extensions
- `WindowOverlay/` — Window overlay support

### Platform-Specific Code

Platform-specific files use folder and naming conventions:

**Subfolder convention:**
- `Android/` — Android-only implementations
- `iOS/` — iOS-only implementations
- `MacCatalyst/` — MacCatalyst-only implementations
- `Windows/` — Windows-only implementations

**File extension convention:**
- `.android.cs` — Android TFM only
- `.ios.cs` — iOS and MacCatalyst TFMs
- `.maccatalyst.cs` — MacCatalyst TFM only (does NOT compile for iOS)
- `.windows.cs` — Windows TFM only

> ⚠️ `.ios.cs` files compile for both iOS **and** MacCatalyst. If you have both `.ios.cs` and `.maccatalyst.cs` for the same file, both will compile for MacCatalyst.

### Tests (`maui/tests/`)

All unit tests live under:

```
maui/tests/Syncfusion.Maui.Toolkit.UnitTest/
├── Calendar/
├── Buttons/
├── Picker/
├── Chart/
│   ├── Features/
│   └── DefaultTests/
├── Cards/
├── Editors/
├── Layout/
├── Navigation/
├── Notification/
├── ProgressBar/
├── SparkChart/
└── Miscellaneous/
```

Run all unit tests:
```bash
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/
```

### Sample Projects (`maui/samples/`)

| Project | Purpose |
|---------|---------|
| `Gallery/` | Full control gallery demonstrating all features |
| `Sandbox/` | Empty project for quick testing and issue reproduction |

---

## Development Workflow

### Building

```bash
# Build entire solution
dotnet build ./Syncfusion.Maui.Toolkit.sln

# Build specific control project
dotnet build maui/src/Syncfusion.Maui.Toolkit.csproj
```

### Running Tests

```bash
# Run all unit tests
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/

# Run tests for a specific control
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/ --filter "FullyQualifiedName~Chart"

# If MauiVersion is required (project references $(MauiVersion)), pass it explicitly:
# dotnet build ./Syncfusion.Maui.Toolkit.sln -p:MauiVersion=<value>
# dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/ -p:MauiVersion=<value>
```

### Code Style

Follow the [.NET Foundation Coding Style](https://github.com/dotnet/runtime/blob/master/docs/coding-guidelines/coding-style.md) with these project-specific exceptions:

- **Default accessibility**: Do NOT use the `private` keyword (it is the default in C#)
- **Indentation**: Use **hard tabs** (not spaces)
- **Nullable**: Always respect nullable reference type annotations (`enable` globally)

### CI Pipelines (Azure DevOps)

| Pipeline | File | Purpose |
|----------|------|---------|
| PR Build | `pipelines/build.yml` | Full PR validation on Windows |

The CI pipeline:
1. Installs .NET 10 SDK
2. Installs .NET MAUI workloads
3. Builds the solution
4. Runs unit tests

---

## Contribution Guidelines

### Git Workflow

> 🚨 **CRITICAL Git Rules for Copilot:**

1. **NEVER commit directly to `main`** — Always create a feature branch.
2. **When amending an existing PR**, check out the PR's branch directly (`gh pr checkout XXXXX`). Do NOT create a new branch off a PR branch.
3. **Do NOT rebase, squash, or force-push** unless explicitly requested.

**Safe Git Workflow:**
```bash
# Create a feature branch
git checkout -b fix/issue-XXXXX-short-description

# Commit with a descriptive message
git add .
git commit -m "Fix: Short description of the change

Fixes #XXXXX"

# Push to remote
git push -u origin fix/issue-XXXXX-short-description
```

**When amending an existing PR:**
```bash
gh pr checkout XXXXX
git add .
git commit -m "Fix: Description of additional change"
# STOP and ask the user before pushing
```

### Branching

- `main` — All bug fixes and new features target `main`

### Pull Request Requirements

Every PR description must follow `.github/PULL_REQUEST_TEMPLATE.md`:

```markdown
### Root Cause of the Issue
<!-- Description of root cause -->

### Description of Change
<!-- What was changed and why -->

### Issues Fixed
Fixes #XXXXX

### Screenshots
#### Before:
#### After:
```

- **Always check** "Allow edits from maintainers"
- At least **two Syncfusion team reviewers** must approve before merge
- All PRs target **`main`** branch

### Issue References

- Bug reports: `.github/ISSUE_TEMPLATE/bug-report.yml`
- Feature requests: `.github/ISSUE_TEMPLATE/feature-request.yml`
- Browse backlog: `https://github.com/syncfusion/maui-toolkit/issues?q=is%3Aopen+is%3Aissue+milestone%3ABacklog`

### Public API Changes

- Update XML documentation comments for all public APIs
- Follow existing documentation patterns in the control's source files
- Add or update `<summary>`, `<param>`, `<returns>`, and `<remarks>` tags

---

## Custom Agents

### Available Custom Agents

1. **issue-resolver** — End-to-end agent for fixing bugs and implementing features from GitHub issues or user prompts
   - **Use when**: "Fix issue #XXXXX", "Implement feature from #XXXXX", "Resolve bug #XXXXX"
   - **Phases**: Pre-flight → Fix → Report
   - **Location**: `.github/agents/issue-resolver.md`

### Delegation Policy

When a user request matches an agent's trigger phrases, **immediately delegate** to the appropriate agent. Do not ask for permission.

**Examples:**
- "Fix issue #123" → Delegate to **issue-resolver** agent
- "Implement feature #456" → Delegate to **issue-resolver** agent
- "What does issue #789 describe?" → Answer directly (informational query)
