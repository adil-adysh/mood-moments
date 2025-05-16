using Microsoft.Maui.Controls;
using System;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class NoteStep : ContentView
    {
        public event EventHandler<string>? NoteChanged;
        public NoteStep()
        {
            InitializeComponent();
            NoteEditor.TextChanged += (s, e) => NoteChanged?.Invoke(this, e.NewTextValue);
        }
        public void SetNote(string? note)
        {
            NoteEditor.Text = note ?? string.Empty;
        }
    }
}
