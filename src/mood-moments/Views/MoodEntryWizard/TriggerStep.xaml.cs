using Microsoft.Maui.Controls;
using System;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class TriggerStep : ContentView
    {
        public event EventHandler<string>? TriggerChanged;
        public TriggerStep()
        {
            InitializeComponent();
            TriggerEntry.TextChanged += (s, e) => TriggerChanged?.Invoke(this, e.NewTextValue);
        }
        public void SetTrigger(string? trigger)
        {
            TriggerEntry.Text = trigger ?? string.Empty;
        }
    }
}
