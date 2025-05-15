using System.Collections.ObjectModel;

namespace mood_moments;

public partial class MainPage : ContentPage
{
    ObservableCollection<MoodJournalEntry> entries;

    public MainPage()
    {
        InitializeComponent();

        // Dummy mood journal entries
        entries = new ObservableCollection<MoodJournalEntry>
        {
            new MoodJournalEntry { Date = "2024-06-01", Mood = "😊 Happy", Context = "Work", Trigger = "Meeting", Intensity = "Low", Notes = "Had a great day at the park." },
            new MoodJournalEntry { Date = "2024-06-02", Mood = "😐 Neutral", Context = "Home", Trigger = "Routine", Intensity = "Medium", Notes = "Just an average day." },
            new MoodJournalEntry { Date = "2024-06-03", Mood = "😔 Sad", Context = "School", Trigger = "Exam", Intensity = "High", Notes = "Felt a bit down today." }
        };

        JournalEntriesView.ItemsSource = entries;

        // Attach event handler for New Entry button
        NewEntryButton.Clicked += NewEntryButton_Clicked;
    }

    private async void NewEntryButton_Clicked(object sender, EventArgs e)
    {
        var newEntryPage = new NewEntryPage();
        newEntryPage.EntrySaved += (s, entry) =>
        {
            entries.Add(entry);
        };
        await Navigation.PushAsync(newEntryPage);
    }

    public class MoodJournalEntry
    {
        public string Date { get; set; }
        public string Mood { get; set; }
        public string Context { get; set; }
        public string Trigger { get; set; }
        public string Intensity { get; set; }
        public string Notes { get; set; }
    }
}

