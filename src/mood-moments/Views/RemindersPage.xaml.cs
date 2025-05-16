using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using mood_moments.Services;
using Plugin.LocalNotification;
using mood_moments.ViewModels;

namespace mood_moments.Views
{
    public partial class RemindersPage : ContentPage
    {
        public ObservableCollection<string> Reminders { get; set; } = new();
        private RemindersService remindersService = new();
        private RemindersViewModel viewModel = new();

        public RemindersPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void AddReminderButton_Clicked(object? sender, System.EventArgs e)
        {
            // Show a custom time picker dialog first
            var now = DateTime.Now;
            var hourPicker = new Picker { Title = "Hour" };
            for (int h = 1; h <= 12; h++) hourPicker.Items.Add(h.ToString());
            int hour12 = now.Hour % 12;
            if (hour12 == 0) hour12 = 12;
            hourPicker.SelectedIndex = hour12 - 1;
            var minutePicker = new Picker { Title = "Minute" };
            for (int m = 0; m < 60; m++) minutePicker.Items.Add(m.ToString("D2"));
            minutePicker.SelectedIndex = now.Minute;
            var amRadio = new RadioButton { Content = "AM", GroupName = "ampm", IsChecked = now.Hour < 12 };
            var pmRadio = new RadioButton { Content = "PM", GroupName = "ampm", IsChecked = now.Hour >= 12 };
            var okClicked = false;
            var okButton = new Button { Text = "OK" };
            okButton.Clicked += (s, e2) => { okClicked = true; Application.Current?.MainPage?.Navigation.PopModalAsync(); };

            var timeStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = { hourPicker, new Microsoft.Maui.Controls.Label { Text = ":", VerticalTextAlignment = TextAlignment.Center }, minutePicker, amRadio, pmRadio }
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

            // Prompt for reminder text after time is picked
            string result = await DisplayPromptAsync("New Reminder", "Enter reminder text:");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var scheduled = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0, DateTimeKind.Local);
                if (scheduled < DateTime.Now)
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
