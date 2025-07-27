---
applyTo: '**/*.cs,**/*.csproj,**/Program.cs,**/*.razor'
description: 'DDD and .NET architecture guidelines'
---
# DDD and .NET Architecture Guidelines

## Domain-Driven Design (DDD)
- Model the domain using aggregates, entities, and value objects.
- Use repositories for data access abstraction.
- Keep domain logic in the domain layer, not in services or controllers.
- Use domain events for decoupling and side effects.

## Clean Architecture
- Separate concerns into layers: Presentation, Application, Domain, Infrastructure.
- Use dependency inversion to decouple layers.
- Keep business logic independent of frameworks and infrastructure.
- Use interfaces for abstractions and inject dependencies.

## Project Structure
- Organize by feature or bounded context, not by technical type.
- Use solution folders for logical grouping.
- Keep projects small and focused.

## Testing
- Write unit, integration, and end-to-end tests.
- Use test doubles (mocks, stubs) for dependencies.
- Test business logic in isolation from infrastructure.

## Security
- Use authentication and authorization middleware.
- Validate all input and output.
- Use secure defaults and follow OWASP guidelines.
