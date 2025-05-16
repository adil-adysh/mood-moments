using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

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
        }

        private async void AddReminderButton_Clicked(object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("New Reminder", "Enter reminder text:");
            if (!string.IsNullOrWhiteSpace(result))
                Reminders.Add(result);
        }
    }
}
