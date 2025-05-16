using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class ContextStep : ContentView
    {
        public event EventHandler<string>? ContextChanged;

        public class ContextOption
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
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
            ContextOptionsView.SelectionChanged += (s, e) =>
            {
                if (ContextOptionsView.SelectedItem is ContextOption opt)
                    ContextChanged?.Invoke(this, opt.Name);
            };
        }

        public void SetContext(string? context)
        {
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
