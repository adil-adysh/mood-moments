---
title: Mood Moments Data Specification
version: 1.0
date_created: 2025-07-27
owner: Mood Moments Team
tags: [data, model, contract, app]
---

# Introduction

This specification defines the data models, storage schema, and data contracts for the Mood Moments application. It ensures that all data structures are explicit, unambiguous, and optimized for privacy, extensibility, and generative AI consumption.

## 1. Purpose & Scope

To provide a clear, machine-readable definition of all core data entities, their relationships, and storage formats for Mood Moments. Intended for developers, architects, and integrators.

## 2. Definitions

- **MoodJournalEntry**: A record of a user's mood check-in.
- **EmotionHierarchy**: Structured set of emotions for selection.
- **ContextAndTriggers**: Contextual data and triggers for mood events.
- **UserSettings**: User preferences and app configuration.
- **JSON**: JavaScript Object Notation, used for local storage.

## 3. Requirements, Constraints & Guidelines

- **REQ-001**: All data must be stored locally in encrypted JSON files.
- **REQ-002**: Data models must be versioned for future migration.
- **REQ-003**: All fields must be explicitly typed and documented.
- **CON-001**: No personally identifiable information (PII) stored by default.
- **SEC-001**: All data at rest must be encrypted.
- **GUD-001**: Use camelCase for all JSON property names.

## 4. Interfaces & Data Contracts

### MoodJournalEntry (JSON Schema)
```json
{
  "id": "string (GUID)",
  "timestamp": "string (ISO 8601)",
  "emotionId": "string",
  "intensity": "integer (1-5)",
  "context": "string",
  "triggers": ["string"],
  "notes": "string (optional)"
}
```

### EmotionHierarchy (JSON Schema)
```json
{
  "id": "string",
  "name": "string",
  "parentId": "string (nullable)",
  "children": [ { /* EmotionHierarchy */ } ]
}
```

### ContextAndTriggers (JSON Schema)
```json
{
  "contexts": ["string"],
  "triggers": ["string"]
}
```

### UserSettings (JSON Schema)
```json
{
  "reminderTimes": ["string (HH:mm)"] ,
  "notificationsEnabled": "boolean",
  "theme": "string (light|dark|system)",
  "dataExportEnabled": "boolean"
}
```

## 5. Acceptance Criteria

- **AC-001**: All data files must validate against the above schemas.
- **AC-002**: Data migration scripts must exist for any schema changes.
- **AC-003**: No PII is present in any stored data by default.
- **AC-004**: All data must be encrypted at rest.

## 6. Test Automation Strategy

- **Test Levels**: Unit (model validation), Integration (data read/write), Migration (schema evolution)
- **Frameworks**: MSTest, FluentAssertions
- **Test Data Management**: Use sample JSON files for validation
- **CI/CD Integration**: Schema validation in pipeline
- **Coverage Requirements**: 100% for model validation logic

## 7. Rationale & Context

Explicit schemas and local storage ensure privacy, facilitate future extension, and support generative AI use cases. Versioning and migration enable safe evolution of data models.

## 8. Dependencies & External Integrations

### Data Storage
- **DAT-001**: Local JSON files (MoodEntries.json, UserSettings.json)

### Technology Platform Dependencies
- **PLT-001**: .NET MAUI file APIs

### Compliance Dependencies
- **COM-001**: Must comply with local data privacy regulations (e.g., GDPR)

## 9. Examples & Edge Cases

```json
// Example MoodJournalEntry with edge cases
{
  "id": "b1c2d3e4-f5a6-7890-1234-56789abcdef0",
  "timestamp": "2025-07-27T14:30:00Z",
  "emotionId": "happy",
  "intensity": 5,
  "context": "work",
  "triggers": [],
  "notes": "Felt great after finishing a big project."
}
```

## 10. Validation Criteria

- All data files must pass schema validation tests
- No untyped or undocumented fields allowed
- Data must be readable and writable by the app without error

## 11. Related Specifications / Further Reading

- [spec-architecture-mood-moments.md](./spec-architecture-mood-moments.md)
- [docs/requirements.md](../docs/requirements.md)
- [docs/design.md](../docs/design.md)
