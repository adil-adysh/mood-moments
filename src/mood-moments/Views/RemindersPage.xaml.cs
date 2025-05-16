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
                // Show a custom time picker dialog
                var hourPicker = new Picker { Title = "Hour" };
                for (int h = 1; h <= 12; h++) hourPicker.Items.Add(h.ToString());
                hourPicker.SelectedIndex = 0;
                var minutePicker = new Picker { Title = "Minute" };
                for (int m = 0; m < 60; m++) minutePicker.Items.Add(m.ToString("D2"));
                minutePicker.SelectedIndex = 0;

                // Use radio buttons for AM/PM
                var amRadio = new RadioButton { Content = "AM", GroupName = "ampm", IsChecked = true };
                var pmRadio = new RadioButton { Content = "PM", GroupName = "ampm" };

                var okClicked = false;
                var okButton = new Button { Text = "OK" };
                okButton.Clicked += (s, e2) => { okClicked = true; Application.Current?.MainPage?.Navigation.PopModalAsync(); };

                var timeStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 10,
                    Children = { hourPicker, new Label { Text = ":", VerticalTextAlignment = TextAlignment.Center }, minutePicker, amRadio, pmRadio }
                };

                var timeDialog = new ContentPage
                {
                    Content = new StackLayout
                    {
                        Padding = 20,
                        Children =
                        {
                            new Label { Text = "Select Reminder Time", FontAttributes = FontAttributes.Bold, FontSize = 18 },
                            timeStack,
                            okButton
                        }
                    }
                };

                await (Application.Current?.MainPage?.Navigation?.PushModalAsync(timeDialog) ?? Task.CompletedTask);
                // Wait for user to close dialog
                while (Application.Current?.MainPage?.Navigation?.ModalStack?.LastOrDefault() == timeDialog && !okClicked)
                {
                    await Task.Delay(100);
                }

                int hour = hourPicker.SelectedIndex + 1;
                int minute = minutePicker.SelectedIndex;
                bool isPm = pmRadio.IsChecked;
                if (isPm && hour < 12) hour += 12;
                if (!isPm && hour == 12) hour = 0;

                var now = DateTime.Now;
                var scheduled = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
                if (scheduled < now)
                    scheduled = scheduled.AddDays(1); // schedule for next day if time has passed
                Reminders.Add($"{result} at {scheduled:hh:mm tt}");
                remindersService.ScheduleReminder(result, scheduled, Reminders.Count);
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
