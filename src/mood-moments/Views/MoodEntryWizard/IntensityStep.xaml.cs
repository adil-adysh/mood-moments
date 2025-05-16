using Microsoft.Maui.Controls;
using System;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class IntensityStep : ContentView
    {
        public event EventHandler<string>? IntensitySelected;
        private static readonly string[] IntensityLabels = { "Very Low", "Low", "Moderate", "High", "Very High" };
        public IntensityStep()
        {
            InitializeComponent();
            IntensitySlider.ValueChanged += (s, e) =>
            {
                // Snap to nearest integer value
                var intValue = (int)Math.Round(IntensitySlider.Value);
                IntensitySlider.Value = intValue;
                var label = IntensityLabels[intValue - 1];
                IntensityValueLabel.Text = $"{label} ({intValue})";
                IntensitySelected?.Invoke(this, label);
            };
            // Set initial label
            IntensitySlider.Value = 3;
            IntensityValueLabel.Text = $"{IntensityLabels[2]} (3)";
        }

        public Slider IntensitySliderControl => IntensitySlider;
    }
}
