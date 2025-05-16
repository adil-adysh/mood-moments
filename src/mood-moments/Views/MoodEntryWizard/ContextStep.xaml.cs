using Microsoft.Maui.Controls;
using System;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class ContextStep : ContentView
    {
        public event EventHandler<string>? ContextChanged;
        public ContextStep()
        {
            InitializeComponent();
            ContextEntry.TextChanged += (s, e) => ContextChanged?.Invoke(this, e.NewTextValue);
        }
        public void SetContext(string? context)
        {
            ContextEntry.Text = context ?? string.Empty;
        }
    }
}
