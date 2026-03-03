# Syncfusion MAUI Toolkit - Copilot Instructions

## Repository Overview

This is the **Syncfusion MAUI Toolkit** — an open-source collection of .NET MAUI controls. The codebase contains:
- **Source code**: `maui/src/` — Controls organized by folder (Accordion, BottomSheet, Button, Calendar, Cards, Carousel, Charts, Chip, Core, EffectsView, Expander, NavigationDrawer, NumericEntry, OtpInput, Picker, Popup, ProgressBar, PullToRefresh, SegmentedControl, Shimmer, SparkCharts, SunburstChart, TabView, TextInputLayout)
- **Unit tests**: `maui/tests/Syncfusion.Maui.Toolkit.UnitTest/` — xUnit tests organized by category (Buttons, Calendar, Cards, Chart, Editors, Layout, Miscellaneous, Navigation, Notification, Picker, ProgressBar, SparkChart, Sunburst)
- **Samples**: `maui/samples/Gallery/` — Control gallery app, `maui/samples/Sandbox/` — Quick testing sandbox

## Build & Test Commands

```bash
# Build the toolkit library
dotnet build maui/src/Syncfusion.Maui.Toolkit.csproj

# Run all unit tests
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj

# Run specific tests
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj --filter "FullyQualifiedName~SfButtonUnitTests"
```

## Target Frameworks

- Library: `net9.0`, `net10.0` + platform-specific (`-android`, `-ios`, `-maccatalyst`, `-windows`)
- Unit tests: `net10.0` (platform-agnostic)

## Testing Conventions

- **Framework**: xUnit with `[Fact]` and `[Theory]` + `[InlineData]`
- **Base class**: `BaseUnitTest` (provides reflection helpers: `SetPrivateField`, `GetPrivateField`, `InvokePrivateMethod`, etc.)
- **Namespace**: `Syncfusion.Maui.Toolkit.UnitTest` (some categories use sub-namespaces: `.Charts`, `.SparkCharts`, `.Sunburst` — always match existing files in the folder)
- **Naming convention**: `{PropertyOrMethod}_{ActionOrReturn}_{Context}` (e.g., `BorderColor_GetAndSet_UsingColor`)
- **Organization**: `#region` blocks for Public Properties, Internal Properties, Methods, Events, Combinations

## Agents & Skills

This repository uses GitHub Copilot agents and skills for automated bug fixing:

### Agents (`.github/agents/`)
- **bug-fix.md** — End-to-end bug fix workflow (Pre-Flight → Gate → Fix → Report)
- **write-tests-agent.md** — Determines test type and invokes appropriate skill
- **learn-from-pr.md** — Extracts lessons from completed PRs and applies improvements

### Skills (`.github/skills/`)
- **try-fix/** — Attempts ONE alternative fix, tests it, reports results
- **verify-tests-fail-without-fix/** — Two-phase test verification (FAIL without fix, PASS with fix)
- **write-unit-tests/** — Creates xUnit tests following existing patterns
- **issue-triage/** — Queries and triages open GitHub issues
- **pr-finalize/** — Verifies PR title/description and performs code review
- **ai-summary-comment/** — Posts/updates structured progress comments on PRs
- **learn-from-pr/** — Analyzes completed PRs for lessons learned
- **pr-build-status/** — Retrieves CI build status for PRs

## Code Style

- C# with nullable reference types enabled
- Platform-specific code uses filename convention: `ClassName.Platform.cs` (e.g., `SfBottomSheet.iOS.cs`)
- XML documentation comments on public APIs
- `AllowUnsafeBlocks` is enabled for the main library

## Common Patterns

- Controls inherit from MAUI base classes (`ContentView`, `View`, etc.)
- Bindable properties use `BindableProperty.Create()` pattern
- Platform-specific behavior via handlers and `#if` directives or partial classes
- Resources and themes in `maui/src/Themes/` and `maui/src/Resources/`
