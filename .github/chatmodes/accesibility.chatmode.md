---
description: 'Accessibility-first mode for .NET MAUI apps.'
platform: '.NET MAUI (iOS, Android, Windows, macOS)'
model: GPT-4.1
title: 'Accessibility mode for MAUI'
---

## ⚠️ Accessibility is a Core Requirement

This project treats accessibility as a **non-negotiable feature**. All views, components, and controls must comply with **WCAG 2.1 AA principles**, as interpreted for native cross-platform development with .NET MAUI.

Accessibility ensures your app works for **screen reader users (TalkBack, VoiceOver, Narrator)**, **keyboard-only users**, and those using **switch devices or alternate input modes**.

---

## 📋 Key Accessibility Principles (Adapted for MAUI)

We follow the four WCAG principles—**Perceivable**, **Operable**, **Understandable**, and **Robust**—mapped to native app behaviors and platform quirks.

---

### 1. ✅ Perceivable

- Use `SemanticProperties.Description` to provide screen-reader text for `Image`, `Button`, `ImageButton`, and any custom views.
- Add `SemanticProperties.Hint` to give additional context or guidance.
- Set `SemanticProperties.HeadingLevel` for section headers or page titles. This is **critical** for VoiceOver rotor and Android group navigation.
- For decorative images or icons, set `AutomationProperties.IsInAccessibleTree="False"`.
- Use high-contrast colors (text vs. background), and test with dark mode and Windows high contrast themes.
- Avoid conveying meaning solely through color, position, or animation.
- Use `FontSize` ≥ 12pt minimum. Avoid tightly packed, hard-to-read layouts.
- Respect system font scaling for accessibility (`UsePlatformDefaults`, `FontAutoScalingEnabled="True"` where applicable).
- Avoid using `IsVisible="False"` to hide elements that screen readers should still access — use `Opacity=0` and `InputTransparent=True` if you want it hidden visually but readable semantically.

---

### 2. ✅ Operable

- Ensure all interactive controls are focusable via keyboard using `TabIndex` and `IsTabStop`.
- Use `VisualStateManager` to visually highlight keyboard focus (especially on Windows/macOS).
- Avoid gesture-only interactions. Support tap, key press (Enter/Space), and screen reader activation events.
- Manage modal focus properly:
  - Set focus to the modal header/title on open.
  - Restore focus to the invoking control on close.
- Use platform-specific focus APIs (`element.Focus()` in C#) for focus redirection when needed.
- Maintain logical tab/focus order: layout order = navigation order.
- Avoid timed dismissals or animations that interrupt users (e.g., snackbars that vanish too quickly).
- On Android, don’t rely on TalkBack reading items in expected order — test all layouts manually.

---

### 3. ✅ Understandable

- Use `AutomationProperties.LabeledBy` to associate `Label` with `Entry`, `Editor`, or `Picker`.
- Never rely on `Placeholder` alone. Always include a persistent label.
- Use `SemanticProperties.Hint` or `AutomationProperties.HelpText` for instructions or expected input format.
- For invalid fields, update the `SemanticProperties.Description` or inject an error message label **after** the control.
- Ensure dynamic content (like validation messages or banners) are in a `LiveRegion`:
  - `SemanticProperties.LiveSetting="Polite"` or `"Assertive"`
- Don’t use animations or transitions that cause disorientation or rapid layout shifts.
- Respect `ReducedMotion` settings where applicable (macOS, Windows).

---

### 4. ✅ Robust

- Use built-in MAUI controls (like `Entry`, `Button`, `Switch`) where possible for maximum native accessibility.
- Test across platforms — each renders screen reader output differently.
- Avoid deeply nested layouts that confuse assistive tech traversal.
- For custom controls, expose proper accessibility properties (`Description`, `Hint`, `HeadingLevel`, `IsInAccessibleTree`, etc.).
- Use `AutomationId` only for UI test automation. Don’t substitute it for screen reader content.
- Support localization of all spoken strings — screen readers may skip controls if `Description` is missing or not localized.
- Keep app logic and UI behavior predictable and consistent across input types (keyboard, touch, screen reader).

---

## 🧩 Developer Reminders

### Semantic Best Practices

| Role | Property |
|------|----------|
| Control name | `SemanticProperties.Description` |
| Guidance text | `SemanticProperties.Hint` |
| Section heading | `SemanticProperties.HeadingLevel="Level1-6"` |
| Dynamic updates | `SemanticProperties.LiveSetting` |
| Custom grouping | Use nested layout with semantic heading or description |

---

### Labels and Form Fields

- ✅ Pair `Label` with `Entry` using `AutomationProperties.LabeledBy="{x:Reference myLabel}"`.
- ✅ Always provide both label and placeholder. Never rely on placeholder alone.
- ✅ For errors, use a `Label` bound to validation state and announce it via screen reader.
- ❌ Don’t hide the label just because placeholder looks cleaner — screen reader users rely on it.

---

### Images and Icons

- ✅ Provide `SemanticProperties.Description` for non-decorative images.
- ✅ Use `FontImageSource` with `AutomationProperties.Name` or `SemanticProperties.Description`.
- ❌ For decorative images, use `AutomationProperties.IsInAccessibleTree="False"`.

---

### Custom Controls and Layout

- If creating custom controls:
  - Expose all relevant `SemanticProperties`.
  - Implement focus logic (keyboard, programmatic).
  - Ensure logical structure in the accessibility tree.
- For `CollectionView`, use:
  - `Header` as a semantic heading
  - Item containers with `SemanticProperties.Description` set to concise summaries
  - Announce total count on page load, if applicable

---

### Live Content and Dynamic UI

- Use `SemanticProperties.LiveSetting="Polite"` for:
  - Validation banners
  - Toasts/snackbars
  - New content added to lists
- Announce modal open/close events using screen reader focus or description changes.
- Don’t rely on users to “notice” visual changes — ensure semantic notification.

---

### Focus Management

- On modal open, focus heading or primary interactive element.
- On modal dismiss, return focus to prior trigger.
- Set focus programmatically only when needed, and never steal focus mid-interaction.
- Prevent nested focus traps.

---

## 🧪 Accessibility Testing Guide

### ✅ Manual Testing Checklist

- [ ] Enable screen reader (TalkBack/VoiceOver/Narrator) and navigate the entire screen
- [ ] Use only keyboard (Tab/Shift+Tab/Enter/Esc) to interact on Windows/macOS
- [ ] Enable high contrast and dark mode themes
- [ ] Test font scaling and large text settings (esp. Android/iOS)
- [ ] Verify that every control has a spoken name and hint (when appropriate)

### ✅ Tools

| Platform | Tools |
|----------|-------|
| Windows  | Accessibility Insights, Narrator |
| Android  | TalkBack, Accessibility Scanner |
| iOS/macOS | VoiceOver, Xcode Accessibility Inspector |
| Cross-platform | Appium + Accessibility, UI Test Projects |

---

## 📌 Platform-Specific Notes

| Platform | Notes |
|----------|-------|
| **Android** | TalkBack reads order based on layout and z-index — verify focus order manually |
| **iOS** | Use `HeadingLevel` to support VoiceOver rotor navigation |
| **Windows** | Ensure all focusable items show **visible focus outlines** |
| **macOS** | Use Accessibility Inspector to validate tree structure and role info |
| **Shell Apps** | Set `Title` or `AutomationProperties.Name` on Shell pages so screen reader announces screen name |

---

## 🚫 Don’ts

- ❌ Don’t hide controls with `IsVisible="False"` if screen reader access is needed — prefer `IsInAccessibleTree="False"` or `Opacity=0` + `InputTransparent=True`
- ❌ Don’t use `AutomationId` as a description substitute
- ❌ Don’t rely on color, icon, or position to communicate state
- ❌ Don’t show validation messages without updating focus or `LiveSetting`
- ❌ Don’t reorder layout visually without matching semantic/focus order

---

## ✅ Bonus: Pro MAUI Accessibility Tips

- ✅ Use `Shell` + `Title` + `HeadingLevel` to build proper screen structure
- ✅ Use `BindableLayout` for dynamic content + announce counts (e.g., “10 messages loaded”)
- ✅ Prefer `CollectionView` with proper semantic wrapping over `ListView`
- ✅ Use `VisualStateManager` for visible focus, error, and disabled states
- ✅ Localize all spoken content, including `Hint`, `Description`, and error strings

---

## ✅ Summary: Your Code is Accessible When...

- Each interactive control has a `Description` and clear purpose
- Navigation is predictable and supports keyboard/screen reader
- Errors are announced and user can recover easily
- Structure is semantically grouped using `HeadingLevel` and hints
- Testing includes screen reader + keyboard + high contrast modes
- Visual order matches screen reader and keyboard traversal order
- Custom controls explicitly expose accessibility semantics

---
