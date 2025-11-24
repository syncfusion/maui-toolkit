# Copilot Instructions for Syncfusion® Toolkit for .NET MAUI

> Purpose: Provide structured, actionable guidance for AI-assisted contributions (bug fixes, features, tests, maintenance) in this repository while preserving project quality, coding standards, and multi-target stability.

---
## 1. Repository Overview
This repo contains the Syncfusion .NET MAUI Toolkit (multi-targeted library) plus:
- `maui/src/` — Main toolkit library (`Syncfusion.Maui.Toolkit.csproj`).
- `maui/tests/` — xUnit-based unit tests (`Syncfusion.Maui.Toolkit.UnitTest.csproj`).
- `maui/samples/Gallery/` — Full control showcase; verify visual & API consistency.
- `maui/samples/Sandbox/` — Minimal app for fast reproduction cases.
- `targets/` — Build customization (`MultiTargeting.targets`) controlling inclusion/exclusion of platform-specific files.
- Root build infra: `Directory.Build.props`, `Directory.Build.targets`, `.editorconfig`.
- CI pipeline: `pipelines/build.yml` (automatic trigger on PRs).

The toolkit targets cross-platform frameworks: `net8.0`, `net9.0`, Android, iOS, MacCatalyst, Windows (conditional), plus packaging metadata and XML docs.

---
## 2. Technology & Targeting
- Multi-target frameworks declared in `maui/src/Syncfusion.Maui.Toolkit.csproj`.
- Platform-specific source segregation via filename suffixes: `.Android.cs`, `.iOS.cs` (**standard**; all new files **must** use `.iOS.cs`), `.MaciOS.cs`, `.Mac.cs`, `.Windows.cs`, `.Standard.cs`.
- `MultiTargeting.targets` removes non-applicable files from compilation per target at build time and sets `DefineConstants` (`MONOANDROID`, `IOS`, `MACCATALYST`, `WINDOWS`).
- Release build variant `Release-Xml` enforces warnings-as-errors + optimization.
---
## 3. Local Development Setup
Prerequisites:
- .NET 9 SDK (and optionally .NET 8 for backward compatibility).
- MAUI workloads: `dotnet workload install maui`.
- For iOS/MacCatalyst builds: Xcode (latest stable) and macOS.
- For Windows-specific targets: Windows 10+ and Visual Studio 17.10+.

Initial build:
```bash
# Restore tools & build solution
dotnet tool restore
dotnet build ./Syncfusion.Maui.Toolkit.sln -c Debug
```
Run tests:
```bash
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj
```
Run samples (example targeting net9 Android):
```bash
dotnet build maui/samples/Gallery/Syncfusion.Maui.ControlsGallery.csproj -f net9.0-android -c Debug
```
Use an IDE (VS / VS Code + MAUI Dev Kit) to deploy and interact with UI.

---
## 4. Build & Configuration Notes
- Central analyzer settings: `Directory.Build.props` (enables nullable, analyzers, code style enforcement).
- Launch profile support: `Directory.Build.targets` sets single-project Maui capabilities.
- Warning thresholds tightened in `Release-Xml` configuration; treat them early to avoid PR build failures.
- CI (`pipelines/build.yml`) builds using the standard `Release` configuration and runs unit tests with HTML logger artifact. The `Release-Xml` configuration is used for packaging and enforces warnings-as-errors; test it locally before submitting PRs.

---
## 5. Code Style & Conventions
Derived from `.editorconfig` and CONTRIBUTING:
- Indentation: Tabs for C# (`indent_style = tab`). 2 spaces for XML/XAML build files.
- Accessibility modifiers: Avoid explicit `private` (default); do not add where unnecessary.
- Field naming: `_camelCase` for private/internal; PascalCase for static/const.
- Prefer `var` only when type is obvious (editorconfig rules enforce this). Avoid `this.` qualification unless required.
- Use XML doc comments on public APIs (important for NuGet XML docs packaging).
- Avoid expression-bodied members except for properties, indexers, simple accessors (per config).
- All braces on new lines (`Allman` style for C# via new line rules).
- Nullability enabled: address warnings promptly; do not suppress with `!` unless logically safe.

---
## 6. Control Architecture Guidelines
Patterns & Principles:
- Partial classes: Split logically (core API, methods, properties) to reduce merge churn. Avoid scattering unrelated functionality across many partials.
- Event Args & Models: Place dedicated `EventArgs` or model classes in `EventArgs/` or `Model/` folders where present; reuse existing types when semantics match.
- Styling & Themes: XAML resources under `Themes/` or control-specific resource dictionaries. 
  **Style file naming:** The codebase contains both `Sf<ControlName>Styles.xaml` (plural) and `Sf<ControlName>Style.xaml` (singular) patterns. **For new controls, use the plural form `Sf<ControlName>Styles.xaml`** to align with current best practices and to indicate the file may contain multiple style resources. The singular form exists for legacy reasons; only use it when updating or maintaining existing files that already use that pattern. Prefer explicit keys for each major visual state.
- Windows / Platform Resources: If a XAML file is excluded (`<MauiXaml Remove>`), verify add-back via `<Page Include>` or `<MauiXaml Update>` when reintroducing. Keep platform styling minimal and consistent.
- Initialization: Keep constructors lightweight; defer heavy setup until handler attachment (`HandlerChanged`) or `Loaded` event.
- Bindable Properties: Provide clear defaults; document side-effects (e.g., layout invalidation) in XML remarks.
- Layout & Measure: Cache measurement where stable; invalidate only when dependent properties change.
- Avoid synchronous heavy work in property setters; schedule via `Dispatcher` if UI thread required.

---
## 7. Platform-Specific Implementation
Guidelines:
- Prefer suffixed files (`*.Android.cs`, `*.iOS.cs`, `*.MaciOS.cs`, `*.Mac.cs`, `*.Windows.cs`) over coarse `#if` regions in shared code. **Important:** The casing of these suffixes (e.g., `.iOS.cs`) must match exactly as shown—`MultiTargeting.targets` only excludes files with the exact casing. Do not use lowercase variants like `.ios.cs`, or the file will not be excluded from non-iOS builds.
- Public API surface must remain uniform; platform differences implemented internally.
- Use `DefineConstants` (`MONOANDROID`, `IOS`, `MACCATALYST`) exclusively for micro-conditions; if logic grows, refactor to a dedicated file.
- Add stub or no-op implementations for unsupported platforms to retain consistent behavior & avoid runtime null references.
- Observe OS version constraints (`SupportedOSPlatformVersion`); if using APIs newer than minimum, guard with `OperatingSystem.IsXXXVersionAtLeast`.
- AOT Considerations: `IsAotCompatible` should be set for net9 targets (future constraint); avoid runtime code generation, reflection emit, or dynamic IL.
- Unsafe Code: Project allows `<AllowUnsafeBlocks>true>`; only use unsafe constructs for measurable performance gains and document rationale in comments adjacent to usage (avoid broad unsafe blocks).

---
## 8. Bug Fix Workflow
1. Triage: Confirm issue exists (link GitHub issue). Collect repro details (target framework, platform, control, property combinations).
2. Reproduction: Prefer `maui/samples/Sandbox` for isolated scenario; or add temporary sample page in Gallery (remove before merge).
3. Unit Test: Add/modify test in `maui/tests/...` verifying logic (state transitions, event firing, layout calculations). For purely visual issues, approximate with property assertions or measure internal state.
4. Cross-Target Build: Build library for all target frameworks locally (`dotnet build maui/src/Syncfusion.Maui.Toolkit.csproj -c Debug`).
5. Platform Check: Deploy on at least 2 platforms impacted (e.g., Android + iOS) when change touches renderer/handler or platform-specific file.
6. Performance/Regression Scan: Avoid introducing layout loops or frequent invalidations. Check for `Measure`/`Invalidate` call frequency.
7. Docs: Update XML summary/comments if behavior changed; ensure remarks describe edge cases.
8. Checklist (Section 13) before PR.
9. PR: Reference issue, minimal diff, clear description of root cause & resolution.

Root Cause Guidance:
- Prefer addressing underlying state mis-synchronization vs patching rendering side-effects.
- Trace event sequences (Loaded, HandlerChanged, SizeChanged) for lifecycle-related bugs.

---
## 9. Feature Implementation Workflow
1. Proposal Validation: Ensure an approved issue/spec exists.
2. API Design: Draft public properties/events; ensure consistent naming (`Sf` prefix reserved only for top-level control classes).
3. Themability: Add style keys & default theme resources.
4. Multi-Target Support: Introduce platform-specific files only where native APIs required.
5. Unit Tests: Add coverage for new logic; include negative cases & nullability checks.
6. Sample: Add representative usage to Gallery (concise; highlight differentiators). Avoid large demo data inside control code.
7. Documentation: Provide XML docs + update README or external docs if needed.
8. Performance: Profile baseline vs new feature (avoid large allocations in hot paths; reuse brushes, pens, buffers where possible).
9. PR Submission & Review.

---
## 10. Testing Strategy
Test Types:
- Unit (xUnit): Logic, state transitions, event ordering, property side-effects.
- Manual Platform Smoke: Deploy Gallery/Sandbox for changed controls across Android, iOS, Windows, MacCatalyst.
- Coverage: Use coverlet collector already referenced.

Coverage Command:
```bash
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/ --collect:"XPlat Code Coverage" --logger:"trx" --results-directory TestResults
```
Optional threshold example (future enhancement): integrate `-- DataCollectionRunSettings.IncludeNativeLibraries` or coverlet MSBuild with thresholds.

Writing Tests:
- Follow Arrange / Act / Assert; single assertion focus unless logically grouped.
- Prefer `[Theory]` with `InlineData` for property matrix coverage.
- Avoid relying on UI thread—keep tests pure. For dispatcher required logic, abstract into testable service or helper.
- Assert event order by capturing sequence into a list.

Recommended Control Test Categories:
- Defaults: Verify initial values & non-null essentials.
- Transition: Expand/collapse, selection changes, navigation index updates.
- Binding Reactivity: Collection changes reflect in internal state.
- Event Semantics: ValueChanged occurs before Completed if documented.
- Edge Conditions: Empty data, null items, boundary numeric ranges, culture-specific formatting.

Future (Visual/UI): Consider screenshot diff harness or MAUI UITest for rendering correctness.

---
## 11. Performance & Memory Guidelines
- Minimize allocation in render/layout loops; cache `Size` computations.
- Pool or reuse brushes, colors, pens, handlers where possible.
- Replace LINQ in hot paths with explicit loops; measure with profiling before refactor.
- Defer heavy initialization until handler attached or `Loaded` to avoid premature platform handle creation.
- Unsubscribe events in disposal/`HandlerChanging` to prevent leaks; favor weak event patterns only if churn high.
- Avoid `Task.Run` for UI-bound operations; use dispatcher/async await patterns.
- Consider future virtualization for large item-based controls (Accordion, Carousel) if performance hotspots identified.

---
## 12. Accessibility & Localization
Accessibility:
- Ensure controls expose automation properties (`SemanticProperties.Description` where appropriate through bindings).
- Keyboard navigation (Windows) — test focus movement after adding interactive elements.
- Color contrast: Default styles should meet WCAG AA; avoid low-opacity essential text.

Localization:
- Do not hard-code user-facing strings inside controls; expose properties or use resource dictionaries for eventual localization.
- Culture-sensitive formatting (numeric/date controls): Use current culture; allow override.

---
## 13. Quality Assurance Checklist (Pre-PR)
- [ ] Multi-target build succeeds (all declared TFMs).
- [ ] `Release-Xml` build passes with zero warnings.
- [ ] New/updated unit tests added & passing.
- [ ] XML docs updated for all changed public APIs.
- [ ] No silent breaking changes (or migration notes included).
- [ ] Platform parity: Implemented or stubbed for each target.
- [ ] Performance regression checked (manual or profiling sample scenario).
- [ ] No memory leaks (events/timers unsubscribed, no static references to disposable instances).
- [ ] Samples reflect changes (Gallery/Sandbox updates if needed).
- [ ] Themes/styles aligned; no orphaned resource keys.
- [ ] Accessibility & semantics validated (focus, descriptions).
- [ ] PR description references issue & explains root cause/fix.

---
## 14. Security Considerations
- Validate user-provided bindable properties when they can cause native calls or reflection.
- Avoid exposing raw file system or network operations in controls; delegate to app layer.
- Do not store sensitive data; ephemeral input only.
- Sanitize any text that could reflect into HTML-rendering surfaces (future web-view integration scenarios).

---
## 15. Documentation & Packaging
- XML docs generated in Release builds; ensure summaries concise and actionable.
- Keep README in sync for any new flagship control additions.
- Version bump: Adjust `<Version>` in toolkit csproj when release planning—coordinate with release notes link.

---
## 16. CI Integration Notes
Pipeline Steps (pipelines/build.yml):
1. Install .NET SDK & workloads.
2. Build solution (Release).
3. Run unit tests with HTML output.
4. Publish test artifacts.

Best Practice: Replicate CI locally using same commands (Release configuration) before pushing to reduce iteration cycles.
---
## 17. Release & Versioning
- Semantic versioning recommended (Major.Minor.Patch). Current placeholder `1.0.0`.
- Use `Release-Xml` configuration for packaging + strict warnings.
- Confirm package assets (LICENSE, README, icon) included via csproj `<None Pack="true" ...>` entries.

---
## 18. Common Pitfalls & Troubleshooting
| Symptom | Likely Cause | Action |
|---------|-------------|--------|
| Missing platform behavior | File suffix not included for target | Verify `MultiTargeting.targets` conditions / file naming |
| Handler not invoked | Control loaded before `ConfigureSyncfusionToolkit()` | Ensure toolkit initialization in `MauiProgram` |
| Build warning escalated to error | Analyzer rule severity in `.editorconfig` | Adjust code per rule; do not suppress unless justified |
| Resource not applied (Windows) | XAML removed via `<MauiXaml Remove>` | Verify csproj resource inclusion sections |
| Test failing only on CI | Workload missing locally | Run `dotnet workload list` & reinstall `maui` |

---
## 19. Suggested Future Enhancements (Non-blocking)
- Automated UI tests (MAUI UITest/Appium/Playwright hybrid).
- BenchmarkDotNet performance suite (layout, measure, render hotspots).
- Screenshot diff pipeline for visual regressions.
- Accessibility audit script (semantic tree scan & contrast analyzer).
- Allocation snapshot tooling integrated into debug profile.

---
## 20. AI Assistance Guidance
When generating changes:
- Keep diffs minimal; avoid formatting churn.
- Never introduce breaking public API changes silently.
- Prefer adding tests before refactors.
- For ambiguous behavior, search existing similar control implementations first (follow established patterns).
- Use platform-specific files over large `#if` blocks to maintain clarity.

---
## 21. Quick Command Reference
```bash
# Full multi-target build (debug)
dotnet build maui/src/Syncfusion.Maui.Toolkit.csproj -c Debug

# Strict build
dotnet build maui/src/Syncfusion.Maui.Toolkit.csproj -c Release-Xml

# Run unit tests (with coverage)
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/ --collect:"XPlat Code Coverage"

# Build Gallery sample for Android
dotnet build maui/samples/Gallery/Syncfusion.Maui.ControlsGallery.csproj -f net9.0-android -c Debug
```

---
## 22. Final Notes
Prioritize stability across all platforms; treat cross-target regression risk as highest concern. Favor predictability and low maintenance complexity over clever abstractions. Document intent for any non-trivial architectural change inside PR description.

---
