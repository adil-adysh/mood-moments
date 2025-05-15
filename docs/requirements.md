## 📘 Software Requirements Specification (SRS)

*Project Name:* Mood Moments

---

### 1. *Introduction*

#### 1.1 Purpose

To create a mobile application that helps users track and reflect on their emotional well-being using a simple, nonverbal, and inclusive interface. The app aims to support emotional awareness and mental health through low-effort interactions and clear insights into emotional patterns.

#### 1.2 Intended Audience

* Individuals seeking emotional self-awareness with minimal input
* Users across diverse ability ranges (inclusive of cognitive, sensory, and emotional needs)
* Mental health researchers (opt-in anonymized data)

#### 1.3 Scope

The app provides:

* Tap-based mood logging with context and intensity
* Visual summaries of emotional trends
* Insight into life domains affecting mood
* Optional prompts for mindfulness and reflection
* Offline functionality with privacy controls

---

### 2. *Overall Description*

#### 2.1 Product Perspective

* Mobile-first, local-first app (with optional cloud sync)
* Simple UI optimized for inclusivity and quick interactions
* Integrates notification system and emotion insight engine

#### 2.2 Product Functions

* Scheduled mood check-in reminders
* Tap-only mood and context selection
* Visualizations and trends over time
* Feedback on emotional patterns and affected life areas
* Optional reflective and mindfulness activities

#### 2.3 User Classes and Characteristics

* *General Users*: Want easy emotional tracking and insight
* *Inclusive Users*: Require minimal interaction, consistent design, and supportive feedback (e.g., neurodivergent users, low-energy users, users with temporary impairments)

#### 2.4 Assumptions and Dependencies

* App runs on Android and iOS
* Local data storage by default; optional cloud sync with consent
* Regular user check-ins (e.g., 2–4x/week) improve insight quality

---

### 3. *Specific Requirements*

#### 3.1 Functional Requirements

* *FR1: Mood Check-In Notification*

  * Customizable periodic reminders
  * Tapping opens mood logging screen

* *FR2: Emotion Selection Interface*

  * 6–10 core emotions with emoji/icon
  * Single-tap selection

* *FR3: Context Tagging (Optional)*

  * Tap-based domain tagging:

    * 💼 Work, ❤ Relationships, 🧍‍♂ Health, 🎓 Learning, 🏠 Family, 🌧 Environment, etc.

* *FR4: Intensity Rating (Optional)*

  * 1–5 scale, emoji or icon-based

* *FR5: Action Prompt (Optional)*

  * After check-in:

    * Take a breath, Play relaxing sound, Review moods, Skip

* *FR6: Mood History Visualization*

  * Calendar view with emotion icons
  * Trends over time by emotion and context

* *FR7: Life Domain Insight Engine*

  * Highlights top domains with frequent negative emotions
  * Supports personal growth and focus areas

* *FR8: Weekly/Monthly Summaries*

  * “You felt calmer on weekends”
  * “Work emotions were more negative in the afternoons”

* *FR9: Reflection Prompts (Optional)*

  * Gentle check-in questions like:

    * “I felt supported today” or “Today was draining”

* *FR10: Privacy & Data Control*

  * All data stored locally by default
  * Optional encrypted cloud sync
  * Opt-in for data sharing

---

### 4. *Non-Functional Requirements*

#### 4.1 Usability

* Designed for low interaction effort and broad accessibility
* Zero typing required
* Clear icons, consistent layout, simple navigation

#### 4.2 Inclusivity & Accessibility

* Large touch targets
* Supportive of neurodiverse and low-energy states
* Optional haptic/audio feedback
* Contrast-friendly design
* Clear feedback cues (visual and auditory)

#### 4.3 Performance

* Mood check-in screen loads in <1 second
* Trend generation under 2 seconds

#### 4.4 Security

* All data encrypted locally
* Explicit opt-in for cloud storage or sharing

---

### ✅ Summary

Mood Moments is a minimalist, inclusive emotional tracking app designed for ease, privacy, and insight. It supports a wide range of users in understanding and reflecting on their emotions through intuitive tap-only interactions and intelligent summaries — all while maintaining a respectful, low-effort design approach.