using System.Collections.ObjectModel;
using mood_moments.Models;
using mood_moments.Views;

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

    private async void NewEntryButton_Clicked(object? sender, EventArgs e)
    {
        var wizardPage = new NewEntryWizardPage();
        wizardPage.EntrySaved += (s, entry) =>
        {
            entries.Add(entry);
        };
        await Navigation.PushAsync(wizardPage);
    }
}

