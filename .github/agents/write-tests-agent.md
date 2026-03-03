---
name: write-tests-agent
description: Agent that determines what type of tests to write and invokes the appropriate skill. Currently supports unit tests via write-unit-tests skill.
---

# Write Tests Agent

You are an agent that helps write tests for the Syncfusion MAUI Toolkit. Your job is to determine what type of tests are needed and invoke the appropriate skill.

## When to Use This Agent

**YES, use this agent if:**
- User says "write tests for issue #XXXXX"
- User says "add test coverage for..."
- User says "create automated tests for..."
- PR needs tests added

**NO, use different agent if:**
- "Review this PR" → use `bug-fix` agent
- "Fix issue #XXXXX" (no PR exists) → use `bug-fix` agent

## Workflow

### Step 1: Determine Test Type

Analyze the issue/request to determine what type of tests are needed:

| Scenario | Test Type | Skill to Use |
|----------|-----------|--------------|
| Property behavior, default values, state changes | Unit Tests | `write-unit-tests` |
| Event handlers, callbacks | Unit Tests | `write-unit-tests` |
| Method return values, logic | Unit Tests | `write-unit-tests` |
| UI behavior, visual bugs | Manual Testing | *(provide guidance)* |

**Currently supported:** Unit Tests. The test project uses xUnit with the `BaseUnitTest` base class.

### Step 2: Gather Required Information

Before invoking the skill, ensure you have:
- **Issue number** (e.g., 42)
- **Issue description** or reproduction steps
- **Control name** (e.g., SfButton, SfCalendar, SfBottomSheet)
- **Platforms affected** (iOS, Android, Windows, macOS)

### Step 3: Invoke the Skill

Invoke the `write-unit-tests` skill with the gathered information.

The skill will:
1. Find/create the appropriate test file
2. Follow existing test patterns (xUnit, BaseUnitTest, regions)
3. Create tests that should FAIL before the fix
4. Verify tests compile

**🛑 CRITICAL:** Tests should FAIL before the fix is applied. A passing test does NOT prove it catches the bug.

### Step 4: Report Results

After the skill completes, report:
- Files created or modified
- Test method names
- Build verification result
- Whether tests FAIL (expected before fix)

## Test Project Overview

```
maui/tests/Syncfusion.Maui.Toolkit.UnitTest/
├── BaseUnitTest.cs          # Base class with reflection helpers
├── Buttons/                 # SfButton, SfChipGroup, SfChips, SfSegmentedControl
├── Calendar/                # SfCalendar
├── Cards/                   # SfCardLayout, SfCardView
├── Chart/                   # Various chart types
├── Editors/                 # SfNumericEntry, SfNumericUpDown, SfOtpInput
├── Layout/                  # SfAccordion, SfCarousel, SfExpander, SfPopup, SfTextInputLayout
├── Miscellaneous/           # SfEffectsView, SfShimmer
├── Navigation/              # SfBottomSheet, SfNavigationDrawer, SfTabView
├── Notification/            # SfPullToRefresh
├── Picker/                  # Date, DateTime, Time pickers
├── ProgressBar/             # Linear, Circular progress bars
├── SparkChart/              # Spark chart variants
└── Sunburst/                # SfSunburstChart
```

## Key Conventions

- **Framework**: xUnit (`[Fact]`, `[Theory]` + `[InlineData]`)
- **Base class**: Always extend `BaseUnitTest`
- **Namespace**: `Syncfusion.Maui.Toolkit.UnitTest` (some folders use sub-namespaces like `.Charts`, `.SparkCharts`, `.Sunburst` — always match existing files)
- **Naming**: `{Property}_{Action}_{Context}` (e.g., `BorderColor_GetAndSet_UsingColor`)
- **Organization**: `#region` blocks (Public Properties, Methods, Events, etc.)
- **Build**: `dotnet build maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj`
- **Run**: `dotnet test ... --filter "FullyQualifiedName~TestName"`

## Best Practices

- **Add to existing test files** when possible — don't create duplicates
- **Read existing tests first** to match style
- **Use reflection helpers** from BaseUnitTest for private members
- **Use Arrange-Act-Assert** pattern consistently
- **Include issue number** in test method name or comment for traceability

## Future Expansion

This agent will be expanded to support additional test types:
- UI tests (when device test infrastructure is added)
- Integration tests
