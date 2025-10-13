# GitHub Copilot Review Ruleset for Syncfusion .NET MAUI Toolkit

This directory contains GitHub Copilot review rulesets specifically designed for the Syncfusion .NET MAUI Toolkit repository. These rulesets help automate code reviews and ensure consistent code quality across the project.

## Files Overview

### 📋 `copilot-ruleset.yaml`
The main configuration file that GitHub Copilot uses for automated code reviews. This file defines:
- Review scope and file patterns
- Specific rules for different code categories (performance, API design, security, etc.)
- MAUI-specific review guidelines
- Custom review prompts for different file types

### 📝 `copilot-review-rules.md`
Detailed documentation of the review rules and guidelines. This file provides:
- Comprehensive explanations of each rule
- Code examples and best practices
- MAUI-specific patterns and anti-patterns
- Platform compatibility guidelines

### ⚙️ `copilot-ruleset.yml` 
Alternative YAML format configuration with pattern matching rules for specific code issues.

## 🚀 Getting Started

### Enabling Copilot Reviews

1. **Repository Settings**: Navigate to your repository settings → Code security and analysis
2. **Enable Copilot Reviews**: Turn on "GitHub Copilot code review" 
3. **Configure Rulesets**: The rulesets in this directory will be automatically detected
4. **Customize**: Modify the YAML files to match your team's specific requirements

### Manual Review Triggers

You can manually trigger Copilot reviews by:
- Creating a pull request (automatic)
- Adding the comment `/copilot review` in a PR
- Using the GitHub CLI: `gh pr review --copilot`

## 🎯 Review Focus Areas

### .NET MAUI Specific Rules
- **Control Implementation**: Proper inheritance from MAUI base classes
- **Bindable Properties**: Correct BindableProperty.Create patterns
- **Platform Handlers**: Cross-platform compatibility checks
- **Performance**: UI thread operations and memory management
- **XAML Patterns**: Resource organization and binding efficiency

### Code Quality Rules  
- **Async/Await Patterns**: Proper async method naming and ConfigureAwait usage
- **API Design**: Consistent public API patterns and documentation
- **Memory Management**: Disposal patterns and leak prevention
- **Input Validation**: Parameter checking and security considerations

### Testing Standards
- **Unit Test Coverage**: Encouraging comprehensive test coverage
- **Test Naming**: Descriptive test method names
- **Mock Usage**: Proper mocking patterns and isolation

## 📊 Rule Categories

| Category | Focus | Severity Levels |
|----------|--------|-----------------|
| **Performance** | Memory usage, UI responsiveness, async patterns | Error, Warning, Info |
| **MAUI Framework** | Platform compatibility, control patterns | Error, Warning, Info |  
| **API Design** | Documentation, naming, backwards compatibility | Warning, Info |
| **Security** | Input validation, resource access | Warning, Info |
| **Testing** | Coverage, naming conventions | Info |

## 🛠️ Customization

### Adding New Rules

1. Edit `copilot-ruleset.yaml`
2. Add new rule under appropriate category:
```yaml
rules:
  performance:
    - id: "perf-004"
      name: "New Performance Rule"
      description: "Description of the new rule"
      severity: "warning"
      enabled: true
```

### Modifying Review Prompts

Update the `review_prompts` section to customize how Copilot analyzes different file types:

```yaml
review_prompts:
  - name: "Custom Review"
    trigger: "files:specific/path/**/*.cs"
    prompt: |
      Custom review instructions...
```

### File Scope Configuration

Adjust which files are included/excluded in reviews:

```yaml
scope:
  include_paths:
    - "maui/src/**"
    - "custom/path/**"
  exclude_paths:  
    - "**/generated/**"
    - "**/temp/**"
```

## 🔧 Integration with CI/CD

These rulesets integrate with GitHub's pull request workflow:

- **Automatic Reviews**: Triggered on every PR to `main` branch
- **Status Checks**: Can be configured as required status checks  
- **Review Comments**: Copilot adds inline comments for identified issues
- **Review Summary**: Overall review summary with recommendations

## 📈 Monitoring and Metrics

Track the effectiveness of your rulesets:
- Review the "Insights" tab for pull request analytics
- Monitor common issues identified by Copilot
- Adjust rule severity based on team feedback
- Update rules as the codebase evolves

## 🤝 Contributing

When contributing to these rulesets:

1. **Test Changes**: Validate rule changes on sample PRs
2. **Document Rules**: Update this README with new rule descriptions  
3. **Version Control**: Consider rule versioning for major changes
4. **Team Feedback**: Gather input from developers on rule effectiveness

## 📚 Additional Resources

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [.NET MAUI Documentation](https://docs.microsoft.com/en-us/dotnet/maui/)
- [Syncfusion MAUI Toolkit Docs](https://help.syncfusion.com/maui-toolkit)
- [Contributing Guidelines](../CONTRIBUTING.md)

## 📝 License

These rulesets are part of the Syncfusion .NET MAUI Toolkit project and follow the same license terms.