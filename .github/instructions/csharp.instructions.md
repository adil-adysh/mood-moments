---
applyTo: '**/*.cs'
description: 'Guidelines for building C# applications'
---
# C# Application Development Guidelines

## General Principles
- Use clear, descriptive names for classes, methods, and variables.
- Follow SOLID principles and design patterns where appropriate.
- Write self-explanatory code; comment only when necessary to explain intent.
- Use async/await for asynchronous operations.
- Prefer dependency injection for testability and maintainability.
- Validate all input and handle exceptions gracefully.
- Write unit tests for all business logic.
- Use logging for diagnostics and monitoring.

## .NET Best Practices
- Use `var` for local variables when the type is obvious.
- Prefer interfaces for abstractions.
- Use `readonly` for fields that do not change after construction.
- Avoid magic strings and numbers; use constants or enums.
- Use `using` statements for resource management.
- Prefer immutable types for data models.

## Security
- Never store secrets in code; use secure configuration.
- Validate all external input.
- Use parameterized queries to prevent SQL injection.
- Handle sensitive data with care and follow GDPR/PII guidelines.

## Performance
- Profile and optimize only after measuring.
- Use efficient data structures and algorithms.
- Minimize allocations in performance-critical code.
- Avoid blocking the main/UI thread.

## Documentation
- Use XML comments for public APIs.
- Keep documentation up to date with code changes.
