using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class ContextStep : ContentView
    {
        public event EventHandler<string>? ContextChanged;

        public class ContextOption : INotifyPropertyChanged
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            private bool isSelected;
            public bool IsSelected
            {
                get => isSelected;
                set { if (isSelected != value) { isSelected = value; OnPropertyChanged(); } }
            }
            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static readonly List<ContextOption> Options = new()
        {
            new ContextOption { Name = "Social Interaction", Description = "Any interaction or engagement with others" },
            new ContextOption { Name = "Task or Activity", Description = "Doing something intentional (work, study)" },
            new ContextOption { Name = "Rest or Pause", Description = "Downtime, inactivity, breaks" },
            new ContextOption { Name = "Internal Reflection", Description = "Thinking, feeling, planning internally" },
            new ContextOption { Name = "Change or Event", Description = "Notable external event or transition" },
            new ContextOption { Name = "Anticipation", Description = "Awaiting or worrying about future" },
            new ContextOption { Name = "Physical/Sensory", Description = "Body sensations, health, physical state" },
            new ContextOption { Name = "Environment", Description = "Surroundings like weather, noise, place" },
        };

        public ContextStep()
        {
            InitializeComponent();
            ContextOptionsView.ItemsSource = Options;
            ContextOptionsView.SelectionChanged += ContextOptionsView_SelectionChanged;
        }

        private void ContextOptionsView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (ContextOptionsView.SelectedItem is ContextOption opt)
            {
                foreach (var o in Options) o.IsSelected = false;
                opt.IsSelected = true;
                ContextChanged?.Invoke(this, opt.Name);
            }
        }

        public void SetContext(string? context)
        {
            foreach (var opt in Options)
            {
                opt.IsSelected = (opt.Name == context);
            }
            if (!string.IsNullOrEmpty(context))
            {
                foreach (var opt in Options)
                {
                    if (opt.Name == context)
                    {
                        ContextOptionsView.SelectedItem = opt;
                        break;
                    }
                }
            }
        }
    }
}
