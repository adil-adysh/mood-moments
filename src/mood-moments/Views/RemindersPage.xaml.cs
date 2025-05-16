using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace mood_moments.Views
{
    public partial class RemindersPage : ContentPage
    {
        public ObservableCollection<string> Reminders { get; set; } = new();
        public RemindersPage()
        {
            InitializeComponent();
            RemindersList.ItemsSource = Reminders;
            AddReminderButton.Clicked += AddReminderButton_Clicked;
            RemindersList.SelectionChanged += RemindersList_SelectionChanged;
        }

        private async void AddReminderButton_Clicked(object? sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("New Reminder", "Enter reminder text:");
            if (!string.IsNullOrWhiteSpace(result))
                Reminders.Add(result);
        }

        private async void RemindersList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is string selectedReminder)
            {
                string action = await DisplayActionSheet($"Reminder: {selectedReminder}", "Cancel", null, "Delete");
                if (action == "Delete")
                {
                    Reminders.Remove(selectedReminder);
                }
            }
        }
    }
}
