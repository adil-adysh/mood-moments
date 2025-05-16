using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class IntensityStep : ContentView
    {
        public event EventHandler<string>? IntensitySelected;
        public IntensityStep()
        {
            InitializeComponent();
            IntensityPicker.SelectedIndexChanged += (s, e) =>
            {
                if (IntensityPicker.SelectedItem is string intensity)
                    IntensitySelected?.Invoke(this, intensity);
            };
        }
        public void SetIntensities(IEnumerable<string> intensities, string? selected = null)
        {
            IntensityPicker.ItemsSource = new List<string>(intensities);
            if (!string.IsNullOrEmpty(selected))
                IntensityPicker.SelectedItem = selected;
        }
    }
}
