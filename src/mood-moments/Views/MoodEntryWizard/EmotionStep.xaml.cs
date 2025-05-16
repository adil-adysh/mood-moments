using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class EmotionStep : ContentView
    {
        public event EventHandler<string>? EmotionSelected;
        public event EventHandler? SkipRequested;

        public EmotionStep()
        {
            InitializeComponent();
            SkipButton.Clicked += (s, e) => SkipRequested?.Invoke(this, EventArgs.Empty);
        }

        public void SetEmotions(string title, IEnumerable<string> emotions, string? selected = null)
        {
            StepTitle.Text = title;
            EmotionGrid.Children.Clear();
            int col = 0, row = 0;
            foreach (var emotion in emotions)
            {
                var btn = new Button { Text = emotion, BackgroundColor = (selected == emotion) ? Colors.LightBlue : Colors.LightGray };
                btn.Clicked += (s, e) => EmotionSelected?.Invoke(this, emotion);
                EmotionGrid.Add(btn, col, row);
                col++;
                if (col > 2) { col = 0; row++; }
            }
        }
    }
}
