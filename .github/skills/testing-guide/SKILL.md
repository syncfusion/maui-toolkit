---
name: testing-guide
description: "Writes or reviews unit tests for Syncfusion Toolkit for .NET MAUI controls. Covers what to test, impossible scenarios, naming conventions, and skipped test documentation. Can be invoked standalone or called from any agent (issue-resolver, test-writer, etc.)."
---

# Skill: Testing Guide

Write or review xUnit tests for a Syncfusion Toolkit for .NET MAUI control change.

## When to Use This Skill

- ✅ Writing new tests after a bug fix or feature
- ✅ Reviewing whether existing tests cover a change adequately
- ✅ Called from another agent to handle the test-writing step

---

## Inputs Required

The caller (agent or user) must provide:

| Input | Description |
|-------|-------------|
| **Control area** | e.g., `Charts`, `Calendar`, `Popup` |
| **What changed** | A summary of the fix or feature that was implemented |
| **Affected files** | Source files that were modified |

---

## Step 1: Locate the Test File

```
maui/tests/Syncfusion.Maui.Toolkit.UnitTest/{ControlArea}/
```

```bash
find maui/tests/Syncfusion.Maui.Toolkit.UnitTest/{ControlArea}/ -name "*.cs" | head -20
```

If no test file exists for the control area, create one following the naming pattern of adjacent test files.

---

## Step 2: What to Test

Add tests for **all scenarios that are possible** given the control's design:

| Change Type | What to Cover |
|-------------|--------------|
| Bug fix | Reproduce the bug — must fail without fix, pass with fix |
| Feature | Validate new behavior end-to-end |
| Edge cases | Boundary values, null inputs where nullable is allowed, default state, repeated calls |

**Naming convention**: `MethodOrProperty_Scenario_ExpectedBehavior`

```csharp
[Fact]
public void SomeProperty_WhenSetToValue_ShouldUpdateBehavior()
{
	// Arrange
	var control = new SomeControl();

	// Act
	control.SomeProperty = expectedValue;

	// Assert
	Assert.Equal(expectedValue, control.InternalState);
}
```

> Use **hard tabs** for indentation. Do NOT use the `private` keyword.

---

## Step 3: What NOT to Test (Impossible Scenarios)

Do NOT write tests for scenarios that are architecturally impossible.

A scenario is **impossible** if:

| Reason | Example |
|--------|---------|
| Type system prevents the state | Testing `null` on a non-nullable `required` property |
| Constructor guarantees initialization | Testing "uninitialized" state on a property set in the constructor |
| Code path can never be reached | A branch guarded by a condition always true/false by design |
| Platform API unavailable in unit test context | Testing a renderer or native handler requiring a live platform runtime |
| Contradicts control invariants | Setting an index out of range when the setter clamps to valid range |

---

## Step 4: Document Skipped Scenarios

For each impossible scenario, add a row to `## Skipped Tests` in the caller's state file (or report directly to the user if invoked standalone):

```markdown
## Skipped Tests
| Scenario | Reason Impossible |
|----------|-------------------|
| `Items_WhenNull_ShouldNotThrow` | `Items` is initialized in constructor; can never be null |
| `OnPlatformRenderer_WhenDetached` | Requires native platform runtime; not testable in xUnit |
```

---

## Step 5: Build and Run Tests

```bash
# Build first to catch compile errors
dotnet build ./Syncfusion.Maui.Toolkit.sln

# Run all unit tests
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/

# Run tests filtered to this control area
dotnet test maui/tests/Syncfusion.Maui.Toolkit.UnitTest/ --filter "FullyQualifiedName~{ControlArea}"
```

Report results to the caller or user: tests passed ✅, failed ❌, or skipped with reason.

