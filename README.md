# Mood Moments

> Your personal mood journal and emotional wellness companion, built with .NET MAUI.

---

## Overview

Mood Moments is a cross-platform mobile and desktop application designed to help users track, reflect, and gain insights into their emotional well-being. With a focus on privacy, simplicity, and actionable insights, Mood Moments empowers you to understand your moods, triggers, and emotional patterns over time.

> [!TIP]
> Mood Moments is open source and welcomes contributions from the community!

---

## Features

- **Mood Tracking:** Log daily moods and emotions with a simple, intuitive interface.
- **Emotion Hierarchy:** Explore and select from a rich hierarchy of emotions for deeper self-awareness.
- **Context & Triggers:** Record situations, triggers, and notes for each entry.
- **Reminders:** Set up gentle reminders to encourage regular journaling.
- **Timeline & Insights:** Visualize your mood history and discover trends.
- **Cross-Platform:** Runs on Android, Windows, and more via .NET MAUI.
- **Privacy First:** All data is stored locally on your device.

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with MAUI workload
- Android/iOS/Windows device or emulator

### Build & Run

```sh
# Clone the repository
 git clone https://github.com/adil-adysh/mood-moments.git
 cd mood-moments

# Restore dependencies
 dotnet restore

# Build and run (choose your target platform)
 dotnet build
 dotnet maui run -f:net8.0-android   # Android
 dotnet maui run -f:net8.0-windows   # Windows
```

---

## Project Structure

```
src/
  mood-moments/         # Main MAUI app (UI, ViewModels, Services)
  mood-moments.Droid/   # Android platform-specific code
  mood-moments.WinUI/   # Windows platform-specific code
  mood-moments.Tests/   # Unit tests
```

---

## Documentation

- [docs/requirements.md](docs/requirements.md) — App requirements
- [docs/design.md](docs/design.md) — Design and architecture
- [docs/app-philosophy.md](docs/app-philosophy.md) — Philosophy and goals

---

## Acknowledgements

- Built with [Microsoft .NET MAUI](https://learn.microsoft.com/dotnet/maui/)
- Inspired by the open source and mental health communities

> [!NOTE]
> Mood Moments does not provide medical advice. For mental health support, consult a professional.
