using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class TriggerStep : ContentView
    {
        public event EventHandler<string>? TriggerChanged;

        public class TriggerOption : INotifyPropertyChanged
        {
            public string Name { get; set; } = string.Empty;
            public string Explanation { get; set; } = string.Empty;
            private bool isSelected;
            public bool IsSelected
            {
                get => isSelected;
                set { if (isSelected != value) { isSelected = value; OnPropertyChanged(); } }
            }
            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static readonly List<TriggerOption> Options = new()
        {
            new TriggerOption { Name = "Exclusion / Rejection", Explanation = "Being left out, ignored, dismissed" },
            new TriggerOption { Name = "Criticism / Judgment", Explanation = "Negative feedback or blame" },
            new TriggerOption { Name = "Approval / Validation", Explanation = "Positive recognition, praise" },
            new TriggerOption { Name = "Disconnection", Explanation = "Emotional or physical distance/isolation" },
            new TriggerOption { Name = "Achievement / Failure", Explanation = "Success or failure in goals" },
            new TriggerOption { Name = "Loss / Absence", Explanation = "Missing something or someone" },
            new TriggerOption { Name = "Uncertainty / Ambiguity", Explanation = "Confusion or lack of clarity" },
            new TriggerOption { Name = "Repetition / Pattern", Explanation = "Recurring events or feelings" },
            new TriggerOption { Name = "Self-Evaluation (Neg)", Explanation = "Self-doubt, guilt, negative self-judgment" },
            new TriggerOption { Name = "Self-Evaluation (Pos)", Explanation = "Confidence, pride, positive self-judgment" },
            new TriggerOption { Name = "Violation of Expectation", Explanation = "Reality mismatched with hopes" },
            new TriggerOption { Name = "Physical State", Explanation = "Bodily discomfort, fatigue, illness" },
            new TriggerOption { Name = "Relationship Conflict", Explanation = "Active conflict or disagreement" },
        };

        public TriggerStep()
        {
            InitializeComponent();
            TriggerOptionsView.ItemsSource = Options;
            TriggerOptionsView.SelectionChanged += TriggerOptionsView_SelectionChanged;
        }

        private void TriggerOptionsView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (TriggerOptionsView.SelectedItem is TriggerOption opt)
            {
                foreach (var o in Options) o.IsSelected = false;
                opt.IsSelected = true;
                TriggerChanged?.Invoke(this, opt.Name);
            }
        }

        public void SetTrigger(string? trigger)
        {
            foreach (var opt in Options)
            {
                opt.IsSelected = (opt.Name == trigger);
            }
            if (!string.IsNullOrEmpty(trigger))
            {
                foreach (var opt in Options)
                {
                    if (opt.Name == trigger)
                    {
                        TriggerOptionsView.SelectedItem = opt;
                        break;
                    }
                }
            }
        }
    }
}
