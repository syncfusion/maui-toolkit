---
name: write-unit-tests
description: Creates xUnit tests for bug fixes in the Syncfusion.Maui.Toolkit.UnitTest project. Follows existing test patterns - BaseUnitTest base class, region-organized structure, Arrange-Act-Assert pattern.
metadata:
  author: maui-toolkit
  version: "1.0"
compatibility: Requires .NET SDK, PowerShell 7+
---

# Write Unit Tests Skill

Creates xUnit unit tests for a given issue in the `Syncfusion.Maui.Toolkit.UnitTest` project, following established patterns and conventions.

## When to Use

- "Write tests for issue #XXXXX"
- "Add test coverage for [control]"
- "Create regression test for this bug"
- Before running verify-tests-fail-without-fix (tests need to exist first)

## Inputs

| Input | Required | Description |
|-------|----------|-------------|
| Issue number | Yes | GitHub issue number |
| Issue description | Yes | Description of the bug to test |
| Control name | Yes | Which control is affected (e.g., SfButton, SfAccordion, SfCalendar) |
| Platforms affected | No | iOS, Android, Windows, macOS |

## Outputs

| Field | Description |
|-------|-------------|
| Test file path | Where the test was created/modified |
| Test method name | Name of the test method |
| Verification status | Whether the test compiles and runs |

## Test Project Structure

```
maui/tests/Syncfusion.Maui.Toolkit.UnitTest/
├── BaseUnitTest.cs                          # Base class with reflection helpers
├── Syncfusion.Maui.Toolkit.UnitTest.csproj  # xUnit + .NET 10
├── Buttons/                                 # SfButton, SfChipGroup, SfChips, SfSegmentedControl
├── Calendar/                                # SfCalendar views, methods, subclasses
├── Cards/                                   # SfCardLayout, SfCardView
├── Chart/                                   # SfCartesianChart, SfCircularChart, etc.
├── Editors/                                 # SfNumericEntry, SfNumericUpDown, SfOtpInput
├── Layout/                                  # SfAccordion, SfCarousel, SfExpander, SfPopup, SfTextInputLayout
├── Miscellaneous/                           # SfEffectsView, SfShimmer
├── Navigation/                              # SfBottomSheet, SfNavigationDrawer, SfTabView
├── Notification/                            # SfPullToRefresh
├── Picker/                                  # Date, DateTime, Time pickers
├── ProgressBar/                             # Linear, Circular progress bars
├── SparkChart/                              # Spark chart variants
└── Sunburst/                                # SfSunburstChart
```

## Conventions (MANDATORY)

### File Naming
- Test file: `Sf{ControlName}UnitTests.cs` (or `{ControlName}UnitTests.cs`)
- Place in the appropriate category folder (Buttons, Navigation, Layout, etc.)
- If a test file already exists for the control, **ADD to it** — don't create a new file

### Namespace
- Most test files use `Syncfusion.Maui.Toolkit.UnitTest`
- Some categories use sub-namespaces: `Syncfusion.Maui.Toolkit.UnitTest.Charts`, `Syncfusion.Maui.Toolkit.UnitTest.SparkCharts`, `Syncfusion.Maui.Toolkit.UnitTest.Sunburst`
- **Always match the namespace used by existing files in the same folder**

### Class Structure
```csharp
using Syncfusion.Maui.Toolkit.ControlNamespace;

namespace Syncfusion.Maui.Toolkit.UnitTest // or sub-namespace — match existing files in the folder
{
    public class SfControlNameUnitTests : BaseUnitTest
    {
        #region Public Properties
        // Tests for public bindable properties
        #endregion

        #region Internal Properties
        // Tests for internal properties using reflection helpers
        #endregion

        #region Methods
        // Tests for public and private methods
        #endregion

        #region Events
        // Tests for event handlers
        #endregion

        #region Combinations
        // Tests for property combinations and interactions
        #endregion
    }
}
```

### Test Method Naming
Three-part convention: `{PropertyOrMethod}_{ActionOrReturn}_{Context}`

Examples:
- `BorderColor_GetAndSet_UsingColor`
- `Constructor_InitializesDefaultsCorrectly`
- `UpdateLayout_ReturnsCorrectSize_WhenCalled`
- `ItemTapped_TriggersEvent_WhenCalled`

### Test Patterns

**Simple property test ([Fact]):**
```csharp
[Fact]
public void IsEnabled_GetAndSet_ReturnsExpected()
{
    // Arrange
    var control = new SfControl();

    // Act
    control.IsEnabled = false;

    // Assert
    Assert.False(control.IsEnabled);
}
```

**Parameterized test ([Theory]):**
```csharp
[Theory]
[InlineData(0)]
[InlineData(50)]
[InlineData(100)]
public void Width_GetAndSet_UsingDouble(double value)
{
    var control = new SfControl();
    control.Width = value;
    Assert.Equal(value, control.Width);
}
```

**Private member testing (using BaseUnitTest helpers):**
```csharp
[Fact]
public void InternalState_UpdatesCorrectly_WhenPropertyChanges()
{
    var control = new SfControl();
    SetPrivateField(control, "_internalField", 42);
    var result = GetPrivateField<SfControl>(control, "_internalField");
    Assert.Equal(42, result);
}
```

**Event testing:**
```csharp
[Fact]
public void Changed_TriggersEvent_WhenValueModified()
{
    var control = new SfControl();
    bool eventFired = false;
    control.Changed += (s, e) => eventFired = true;

    control.Value = 10;

    Assert.True(eventFired);
}
```

### BaseUnitTest Helpers Available

| Method | Purpose |
|--------|---------|
| `SetPrivateField(obj, fieldName, value)` | Set non-public instance field |
| `GetPrivateField<T>(obj, fieldName)` | Get non-public instance field |
| `InvokePrivateMethod<T>(obj, methodName, params)` | Invoke non-public method |
| `InvokeRefPrivateMethod<T>(obj, methodName, ref params)` | Invoke with ref params |
| `GetNonPublicProperty<T>(obj, propertyName)` | Get non-public property |
| `SetNonPublicProperty(obj, propertyName, value)` | Set non-public property |
| `InvokeStaticPrivateMethod<T>(obj, methodName, params)` | Invoke private static method |

## Workflow

### Step 1: Determine Test Location

1. Identify the control being tested
2. Find the correct category folder
3. Check if a test file already exists for this control:
   ```bash
   find maui/tests/Syncfusion.Maui.Toolkit.UnitTest -name "*ControlName*" -type f
   ```

### Step 2: Examine Existing Patterns

**ALWAYS read an existing test file in the same folder first:**
```bash
ls maui/tests/Syncfusion.Maui.Toolkit.UnitTest/<Category>/
```

Match the style: regions, naming, assertion patterns, imports.

### Step 3: Write the Test

- Create test that reproduces the bug described in the issue
- Test should **FAIL** before the fix is applied (proving it catches the bug)
- Test should **PASS** after the fix is applied
- Use `[Fact]` for single-case, `[Theory]` + `[InlineData]` for parameterized
- Include issue number in method name or comment for traceability

### Step 4: Verify Test Compiles

```bash
dotnet build maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj --no-restore
```

### Step 5: Run the Test

```bash
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Syncfusion.Maui.Toolkit.UnitTest.csproj \
  --filter "FullyQualifiedName~<TestMethodName>" --no-restore -v normal
```

### Step 6: Report Results

Report:
- File created/modified
- Test method name(s)
- Whether tests compile
- Whether tests FAIL (before fix) — this is the expected outcome!

## Output Structure

```
CustomAgentLogsTmp/PRState/{IssueNumber}/write-tests/
├── attempt-1/
│   ├── description.md    # What the test verifies
│   ├── test-info.json    # {TestFile, TestMethod, Category}
│   ├── result.txt        # "Verified", "Failed", or "Unverified"
│   └── notes.md          # Additional notes
```

### test-info.json Format

```json
{
    "TestFile": "maui/tests/Syncfusion.Maui.Toolkit.UnitTest/Navigation/SfBottomSheetUnitTests.cs",
    "TestMethod": "Issue12345_BottomSheetDoesNotCrash_WhenClosedRapidly",
    "Category": "Navigation"
}
```

## Common Mistakes to Avoid

| Mistake | Correct Approach |
|---------|------------------|
| ❌ Creating a new test file when one exists | ✅ Add to existing test file for the control |
| ❌ Using NUnit attributes ([Test]) | ✅ Use xUnit ([Fact], [Theory]) |
| ❌ Not inheriting BaseUnitTest | ✅ Always extend BaseUnitTest |
| ❌ Writing tests that always pass | ✅ Write tests that FAIL without the fix |
| ❌ Wrong namespace | ✅ Always use `Syncfusion.Maui.Toolkit.UnitTest` |
| ❌ Forgetting region organization | ✅ Place in appropriate #region block |
