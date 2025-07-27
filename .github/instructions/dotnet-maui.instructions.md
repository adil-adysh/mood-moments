---
applyTo: '**/*.xaml, **/*.cs'
description: '.NET MAUI component and application patterns'
---
# .NET MAUI Component and Application Patterns

## General Guidelines
- Use XAML for UI layout and C# for logic.
- Prefer MVU or MVVM patterns for maintainability.
- Use `BindableProperty` for custom controls.
- Always use `SemanticProperties` for accessibility.
- Use `AutomationId` for UI testing.
- Prefer `Shell` for navigation in new apps.
- Use platform-specifics and effects for native features.
- Validate all user input and handle errors gracefully.
- Use dependency injection for services.
- Write unit and UI tests for all features.

## Accessibility
- Set `SemanticProperties.Description` and `Hint` for all controls.
- Ensure color contrast and font size accessibility.
- Support screen readers and keyboard navigation.
- Test with accessibility tools on all platforms.

## Performance
- Use compiled bindings where possible.
- Minimize use of `OnPropertyChanged` for performance.
- Use `CollectionView` for large lists.
- Avoid blocking the UI thread.
- Profile with built-in tools and optimize startup time.

## Security
- Never store secrets in code or local storage.
- Use secure storage APIs for sensitive data.
- Validate all external input.
- Use HTTPS for all network requests.

## Deployment
- Use .NET MAUI's multi-targeting for cross-platform builds.
- Test on all target platforms (Android, iOS, Windows, MacCatalyst).
- Use CI/CD for automated builds and tests.
- Follow app store guidelines for each platform.
