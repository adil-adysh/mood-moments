namespace mood_moments;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Dummy mood journal entries
        var entries = new List<MoodJournalEntry>
        {
            new MoodJournalEntry { Date = "2024-06-01", Mood = "😊 Happy", Notes = "Had a great day at the park." },
            new MoodJournalEntry { Date = "2024-06-02", Mood = "😐 Neutral", Notes = "Just an average day." },
            new MoodJournalEntry { Date = "2024-06-03", Mood = "😔 Sad", Notes = "Felt a bit down today." }
        };

        JournalEntriesView.ItemsSource = entries;
    }

    public class MoodJournalEntry
    {
        public string Date { get; set; }
        public string Mood { get; set; }
        public string Notes { get; set; }
    }
}

