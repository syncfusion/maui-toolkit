# Copilot Review Rules for Syncfusion .NET MAUI Toolkit
# GitHub Copilot will use these rules to automatically review pull requests

# .NET MAUI Toolkit specific review guidelines
rules:
  # Code Quality and Standards
  - rule: "Check async/await patterns"
    description: "Ensure proper async/await usage and ConfigureAwait(false) where appropriate"
    files: "**/*.cs"
    review_guidance: |
      - Async methods should end with 'Async' suffix
      - Use ConfigureAwait(false) in library code to avoid deadlocks
      - Avoid async void except for event handlers
      - Consider using ValueTask for hot paths

  - rule: "MAUI control implementation standards"
    description: "Verify MAUI control implementations follow framework patterns"
    files: "maui/src/**/*.cs"
    review_guidance: |
      - Controls should inherit from appropriate base classes (View, Layout, Element)
      - Implement proper bindable properties with BindableProperty.Create
      - Use proper property change callbacks and coercion
      - Follow MAUI handler pattern for platform-specific implementations
      - Ensure proper disposal of platform resources

  - rule: "Cross-platform compatibility"
    description: "Ensure code works across all supported MAUI platforms"
    files: "maui/src/**/*.cs"
    review_guidance: |
      - Avoid platform-specific APIs in shared code
      - Use conditional compilation (#if) sparingly
      - Leverage MAUI handlers for platform-specific functionality
      - Test on all target platforms (Android, iOS, Windows, macOS)

  - rule: "Performance optimization"
    description: "Review for performance implications in UI controls"
    files: "maui/src/**/*.cs"
    review_guidance: |
      - Minimize allocations in frequently called methods (Measure, Arrange, OnDraw)
      - Use object pooling for frequently created/destroyed objects
      - Avoid expensive operations in property setters
      - Consider lazy initialization for heavy resources
      - Profile memory usage and avoid leaks

  # API Design Guidelines  
  - rule: "Public API consistency"
    description: "Ensure public APIs follow consistent design patterns"
    files: "maui/src/**/*.cs"
    review_guidance: |
      - Use clear, descriptive names following .NET naming conventions
      - Provide XML documentation for all public members
      - Follow async patterns consistently
      - Use appropriate access modifiers
      - Consider backwards compatibility for public APIs

  - rule: "Bindable properties implementation"
    description: "Review bindable property implementations for correctness"
    files: "maui/src/**/*.cs"  
    review_guidance: |
      - Use BindableProperty.Create with proper parameters
      - Implement property change callbacks when needed
      - Provide default values appropriate for the property type
      - Use proper binding modes (TwoWay, OneWay, OneTime)
      - Consider coercion callbacks for value validation

  - rule: "Event handling patterns"
    description: "Ensure proper event handling and memory management"
    files: "maui/src/**/*.cs"
    review_guidance: |
      - Use weak event patterns to prevent memory leaks
      - Properly unsubscribe from events in Dispose methods
      - Follow .NET event naming conventions
      - Use EventArgs-derived classes for event data
      - Consider using EventHandler<T> for type safety

  # XAML and Resources
  - rule: "XAML best practices"
    description: "Review XAML files for best practices and performance"
    files: "**/*.xaml"
    review_guidance: |
      - Use x:Name sparingly to avoid memory overhead
      - Prefer compiled bindings where possible
      - Use StaticResource over DynamicResource when values don't change
      - Organize resources logically in ResourceDictionaries
      - Avoid deep nesting that impacts layout performance

  - rule: "Resource organization"
    description: "Ensure proper organization of styles, templates, and resources"
    files: "maui/src/Resources/**/*.*"
    review_guidance: |
      - Group related resources together
      - Use consistent naming conventions for resource keys
      - Provide both light and dark theme variants where applicable
      - Optimize image resources for different screen densities
      - Use vector graphics (SVG) where possible

  # Testing and Quality Assurance
  - rule: "Unit test coverage"
    description: "Encourage comprehensive unit test coverage"
    files: "maui/tests/**/*.cs"
    review_guidance: |
      - Test public API methods and properties
      - Include edge cases and error conditions
      - Use descriptive test method names (Should_When_Given pattern)
      - Mock external dependencies appropriately
      - Achieve reasonable code coverage (aim for >80%)

  - rule: "Sample application quality"
    description: "Ensure samples demonstrate proper usage patterns"
    files: "maui/samples/**/*.*"
    review_guidance: |
      - Demonstrate real-world usage scenarios
      - Include proper error handling
      - Follow MVVM pattern where applicable
      - Provide clear code comments and documentation
      - Test samples on all target platforms

  # Security and Validation
  - rule: "Input validation"
    description: "Ensure proper validation of user inputs and parameters"
    files: "**/*.cs"
    review_guidance: |
      - Validate public method parameters
      - Use ArgumentNullException for null parameters where appropriate
      - Sanitize user inputs to prevent injection attacks
      - Implement proper bounds checking for numeric values
      - Use secure defaults for configuration options

  # Documentation and Comments
  - rule: "Code documentation"
    description: "Ensure adequate code documentation and comments"
    files: "**/*.cs"
    review_guidance: |
      - Provide XML documentation for all public APIs
      - Include usage examples in documentation where helpful
      - Document breaking changes and migration guidance
      - Keep comments up-to-date with code changes
      - Use inline comments sparingly for complex logic only

# File-specific configurations
configurations:
  - path: "maui/src/Charts/**"
    additional_rules:
      - "Review chart rendering performance and memory usage"
      - "Ensure data binding efficiency for large datasets" 
      - "Validate accessibility features for chart elements"

  - path: "maui/src/*/Platform/**"
    additional_rules:
      - "Review platform-specific implementations for consistency"
      - "Ensure proper lifecycle management of platform resources"
      - "Validate proper handler registration and cleanup"

  - path: "maui/tests/**"
    additional_rules:
      - "Ensure tests are isolated and don't depend on external state"
      - "Verify mock objects are properly configured"
      - "Check that async tests use proper assertion patterns"

# Review priorities
priority_areas:
  - performance_critical: "maui/src/**/Platform/**"
  - public_api: "maui/src/**/*.cs"
  - documentation: "**/*.md"
  - security_sensitive: "maui/src/**/Authentication/**"