using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace mood_moments.ViewModels
{
    public class ReminderViewModel : INotifyPropertyChanged
    {
        private string _text = string.Empty;
        private DateTime _time = DateTime.Now;

        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }
        public DateTime Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }
        public string Display => $"{Text} at {Time:hh:mm tt}";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RemindersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ReminderViewModel> Reminders { get; } = new();
        private string _newReminderText = string.Empty;
        private int _hour = DateTime.Now.Hour % 12 == 0 ? 12 : DateTime.Now.Hour % 12;
        private int _minute = DateTime.Now.Minute;
        private bool _isPm = DateTime.Now.Hour >= 12;

        public string NewReminderText
        {
            get => _newReminderText;
            set { _newReminderText = value; OnPropertyChanged(); }
        }
        public int Hour
        {
            get => _hour;
            set { _hour = value; OnPropertyChanged(); }
        }
        public int Minute
        {
            get => _minute;
            set { _minute = value; OnPropertyChanged(); }
        }
        public bool IsPm
        {
            get => _isPm;
            set { _isPm = value; OnPropertyChanged(); }
        }
        public List<int> Hours { get; } = Enumerable.Range(1, 12).ToList();
        public List<int> Minutes { get; } = Enumerable.Range(0, 60).ToList();

        public ICommand AddReminderCommand { get; }
        public ICommand DeleteReminderCommand { get; }

        public RemindersViewModel()
        {
            AddReminderCommand = new RelayCommand(AddReminder);
            DeleteReminderCommand = new RelayCommand<ReminderViewModel>(DeleteReminder);
        }

        private void AddReminder()
        {
            if (string.IsNullOrWhiteSpace(NewReminderText)) return;
            int hour24 = IsPm ? (Hour < 12 ? Hour + 12 : Hour) : (Hour == 12 ? 0 : Hour);
            var now = DateTime.Now;
            var scheduled = new DateTime(now.Year, now.Month, now.Day, hour24, Minute, 0, DateTimeKind.Local);
            if (scheduled < now)
                scheduled = scheduled.AddDays(1);
            Reminders.Add(new ReminderViewModel { Text = NewReminderText, Time = scheduled });
            NewReminderText = string.Empty;
        }
        private void DeleteReminder(ReminderViewModel? reminder)
        {
            if (reminder != null)
                Reminders.Remove(reminder);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
