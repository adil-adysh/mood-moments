using System.Collections.ObjectModel;

namespace mood_moments.Models
{
    public class EntryGroup : ObservableCollection<MoodJournalEntry>
    {
        public string GroupTitle { get; set; } = string.Empty;
        public ObservableCollection<MoodJournalEntry> Entries => this;
    }
}
