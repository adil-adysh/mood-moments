using System.Collections.ObjectModel;
using mood_moments.Models;
using mood_moments.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mood_moments.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MoodJournalEntry> Entries { get; } = new();
        public ObservableCollection<EntryGroup> GroupedEntries { get; } = new();
        public string CurrentTimeUnit { get; set; } = "All";
        public string? SelectedTimeValue { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public MainPageViewModel()
        {
            // Example data
            Entries.Add(new MoodJournalEntry { Date = "2025-05-16", Mood = "😊 Happy", Context = "Work", Trigger = "Meeting", Intensity = "Low", Notes = "Had a great day at the park." });
            Entries.Add(new MoodJournalEntry { Date = "2025-05-15", Mood = "😐 Neutral", Context = "Home", Trigger = "Routine", Intensity = "Medium", Notes = "Just an average day." });
            Entries.Add(new MoodJournalEntry { Date = "2025-05-01", Mood = "😔 Sad", Context = "School", Trigger = "Exam", Intensity = "High", Notes = "Felt a bit down today." });
            UpdateGrouping();
        }

        public void UpdateGrouping()
        {
            GroupedEntries.Clear();
            foreach (var group in TimelineService.GroupEntries(Entries, CurrentTimeUnit, SelectedTimeValue))
                GroupedEntries.Add(group);
        }
    }
}
