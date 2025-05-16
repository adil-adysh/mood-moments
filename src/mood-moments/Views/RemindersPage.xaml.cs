using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using mood_moments.Services;
using Plugin.LocalNotification;

namespace mood_moments.Views
{
    public partial class RemindersPage : ContentPage
    {
        public ObservableCollection<string> Reminders { get; set; } = new();
        private RemindersService remindersService = new();

        public RemindersPage()
        {
            InitializeComponent();
            RemindersList.ItemsSource = Reminders;
            AddReminderButton.Clicked += AddReminderButton_Clicked;
            RemindersList.SelectionChanged += RemindersList_SelectionChanged;
            // NotificationTapped event is not available in v12+.
            // See Plugin.LocalNotification v12 docs for tap handling.
        }

        private async void AddReminderButton_Clicked(object? sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("New Reminder", "Enter reminder text:");
            if (!string.IsNullOrWhiteSpace(result))
            {
                Reminders.Add(result);
                // Schedule notification for 1 minute from now (for demo)
                remindersService.ScheduleReminder(result, DateTime.Now.AddMinutes(1), Reminders.Count);
            }
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

        // The OnNotificationTapped method is removed because NotificationTapped event is not available in the latest Plugin.LocalNotification API.
        // If you need to handle notification taps, refer to the Plugin.LocalNotification documentation for platform-specific handling.
    }
}
