## 🧠 Mood Moments System Design (Private, Reminder-Driven Check-Ins)

---

### 1. 🎯 MVP Objectives

1. **Simple, Tap‑Only Mood Check‑Ins**
   – Capture emotion, intensity, context, and optional personal notes — no typing required.

2. **Private & Offline-First**
   – Data stored locally as JSON; no internet or account needed.

3. **Daily Reminders (Local Notifications)**
   – Users receive configurable notifications to reflect and log moods.

---

### 2. 🏛 Architecture Overview

```text
┌────────────────────────┐
│  Presentation Layer    │ ← .NET MAUI (MVVM)
│ ┌────────────────────┐ │
│ │ MoodCheckInPage     │ │
│ │ SettingsPage (optional)│
│ └────────────────────┘ │
└─────────┬──────────────┘
          │
┌─────────▼──────────────┐
│     Services Layer     │
│ ┌────────────────────┐ │
│ │ StorageService     │ │
│ │ NotificationService│ │
│ └────────────────────┘ │
└─────────┬──────────────┘
          │
┌─────────▼──────────────┐
│       Data Layer       │
│ ┌────────────────────┐ │
│ │ MoodEntries.json    │ │
│ │ UserSettings.json   │ │
│ └────────────────────┘ │
└────────────────────────┘
```

---

### 3. 🧩 Core Components

| Component             | Purpose                                                    |
| --------------------- | ---------------------------------------------------------- |
| `MoodCheckInPage`     | UI for logging mood with emotion, intensity, context, note |
| `StorageService`      | Read/write entries and settings as JSON                    |
| `NotificationService` | Manage local reminders and navigation on tap               |
| `UserSettings.json`   | Stores user preferences (e.g., reminder times)             |

---

### 4. 📄 Data Models

#### 4.1 `MoodEntry.json`

```json
{
  "id": "uuid-abc123",
  "timestamp": "2025-05-15T09:00:00Z",
  "emotion": "Content",
  "intensity": 2,
  "contexts": ["Work", "Alone Time"],
  "note": "Focused well this morning."
}
```

#### 4.2 `UserSettings.json`

```json
{
  "remindersEnabled": true,
  "reminderTimes": ["09:00"],
  "timezone": "Asia/Kolkata"
}
```

---

### 5. ⏰ Notification Handling

#### 5.1 Schedule Reminders

* On startup or settings update:

  * Load `UserSettings.json`
  * For each time → schedule repeating notification

#### 5.2 Tap to Open Check-In

* Tapping a reminder deep-links into `MoodCheckInPage`

```csharp
Shell.Current.GoToAsync("///checkin");
```

---

### 6. 📱 User Interaction Flows

#### 6.1 Mood Check-In

1. Notification received at configured time(s)
2. User taps → navigates to `MoodCheckInPage`
3. Selects:

   * Emotion
   * Intensity (0–3 scale)
   * One or more Contexts
   * Optional personal Note
4. Entry saved to `MoodEntries.json`

#### 6.2 Reminder Configuration (Optional)

* User toggles reminders and selects preferred time(s)
* Updates saved to `UserSettings.json`
* Reminders rescheduled accordingly

---

### 7. 🔐 Privacy & Simplicity

| Feature          | Detail                                      |
| ---------------- | ------------------------------------------- |
| **Storage**      | All data saved locally using JSON           |
| **Offline Mode** | 100% functional without internet            |
| **No Account**   | No sign-in or cloud sync in MVP             |
| **Simple UI**    | Large buttons, no typing, fast screen loads |

---

### 8. 📁 Project Structure

```
MoodMoments/
├── Pages/
│   ├── MoodCheckInPage.xaml(.cs)
│   └── SettingsPage.xaml(.cs)         (optional in MVP)
├── Services/
│   ├── StorageService.cs
│   └── NotificationService.cs
├── Models/
│   ├── MoodEntry.cs
│   └── UserSettings.cs
├── Data/
│   ├── MoodEntries.json
│   └── UserSettings.json
├── AppShell.xaml(.cs)
```

---

## ✅ Summary

This MVP design for *Mood Moments* delivers:

* Tap-based emotional check-ins
* Scheduled reminders for daily engagement
* Private, offline, and account-free experience
* Clean architecture, ready for future extension

It’s minimal but meaningful — giving users the space to reflect without friction.
