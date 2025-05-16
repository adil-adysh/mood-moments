using mood_moments.ViewModels;
using mood_moments.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace mood_moments
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
            JournalEntriesView.ItemsSource = viewModel.GroupedEntries;
            AllTab.Clicked += (s, e) => SwitchTimeUnit("All");
            YearTab.Clicked += (s, e) => SwitchTimeUnit("Year");
            MonthTab.Clicked += (s, e) => SwitchTimeUnit("Month");
            WeekTab.Clicked += (s, e) => SwitchTimeUnit("Week");
            DayTab.Clicked += (s, e) => SwitchTimeUnit("Day");
            TimePicker.SelectedIndexChanged += TimePicker_SelectedIndexChanged;
        }

        void SwitchTimeUnit(string unit)
        {
            viewModel.CurrentTimeUnit = unit;
            viewModel.SelectedTimeValue = null;
            if (unit == "All")
            {
                TimePickerPanel.IsVisible = false;
                viewModel.UpdateGrouping();
            }
            else
            {
                TimePickerPanel.IsVisible = true;
                TimePickerLabel.Text = unit switch
                {
                    "Year" => "Select Year:",
                    "Month" => "Select Month:",
                    "Week" => "Select Week:",
                    "Day" => "Select Day:",
                    _ => "Select:"
                };
                var values = TimelineService.GetTimeValues(viewModel.Entries, unit);
                TimePicker.ItemsSource = values.ToList();
                TimePicker.SelectedIndex = 0;
            }
        }

        void TimePicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                if (TimePicker.SelectedIndex < 0) return;
                viewModel.SelectedTimeValue = TimePicker.SelectedItem as string;
                viewModel.UpdateGrouping();
            }
            catch
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating the timeline. Please try again.", "OK");
                    });
                }
            }
        }
    }
}

