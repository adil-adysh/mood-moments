using mood_moments.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace mood_moments.Views
{
    public partial class NewEntryWizardPage : ContentPage
    {
        private int currentStep = 0;
        private readonly List<string> moods = new() { "😊 Happy", "😐 Neutral", "😔 Sad", "😡 Angry", "😨 Anxious" };
        private readonly List<string> intensities = new() { "Low", "Medium", "High" };
        private string? selectedMood;
        private string? selectedIntensity;
        private string? personalNote;
        private string? contextValue = string.Empty;
        private string? triggersValue = string.Empty;

        public event EventHandler<MoodJournalEntry>? EntrySaved;

        public NewEntryWizardPage()
        {
            InitializeComponent();
            ShowStep();
        }

        private void ShowStep()
        {
            StepContent.Children.Clear();
            BackButton.IsVisible = currentStep > 0;
            NextButton.Text = currentStep == 4 ? "Finish" : "Next";

            switch (currentStep)
            {
                case 0:
                    StepTitle.Text = "Step 1: Select Your Mood";
                    var moodPicker = new Picker { Title = "Mood" };
                    moodPicker.ItemsSource = moods;
                    if (!string.IsNullOrEmpty(selectedMood))
                        moodPicker.SelectedItem = selectedMood;
                    moodPicker.SelectedIndexChanged += (s, e) => selectedMood = (string)moodPicker.SelectedItem;
                    StepContent.Children.Add(moodPicker);
                    break;
                case 1:
                    StepTitle.Text = "Step 2: Select Intensity";
                    var intensityPicker = new Picker { Title = "Intensity" };
                    intensityPicker.ItemsSource = intensities;
                    if (!string.IsNullOrEmpty(selectedIntensity))
                        intensityPicker.SelectedItem = selectedIntensity;
                    intensityPicker.SelectedIndexChanged += (s, e) => selectedIntensity = (string)intensityPicker.SelectedItem;
                    StepContent.Children.Add(intensityPicker);
                    break;
                case 2:
                    StepTitle.Text = "Step 3: Add a Personal Note";
                    var noteEntry = new Editor { Placeholder = "Write your note here...", AutoSize = EditorAutoSizeOption.TextChanges, HeightRequest = 100, Text = personalNote };
                    noteEntry.TextChanged += (s, e) => personalNote = e.NewTextValue;
                    StepContent.Children.Add(noteEntry);
                    break;
                case 3:
                    StepTitle.Text = "Step 4: Context";
                    var contextEntry = new Entry { Placeholder = "Where are you? (e.g., Home, Work)", Text = contextValue };
                    contextEntry.TextChanged += (s, e) => contextValue = e.NewTextValue;
                    StepContent.Children.Add(contextEntry);
                    break;
                case 4:
                    StepTitle.Text = "Step 5: Triggers";
                    var triggersEntry = new Entry { Placeholder = "What triggered this mood?", Text = triggersValue };
                    triggersEntry.TextChanged += (s, e) => triggersValue = e.NewTextValue;
                    StepContent.Children.Add(triggersEntry);
                    break;
            }
        }

        private static void BackButton_Clicked(object sender, EventArgs e)
        {
            if (sender is NewEntryWizardPage page && page.currentStep > 0)
            {
                page.currentStep--;
                page.ShowStep();
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (currentStep < 4)
            {
                currentStep++;
                ShowStep();
            }
            else
            {
                // Save entry
                var entry = new MoodJournalEntry
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Mood = selectedMood,
                    Intensity = selectedIntensity,
                    Notes = personalNote,
                    Context = contextValue,
                    Trigger = triggersValue
                };
                EntrySaved?.Invoke(this, entry);
                await Navigation.PopAsync();
            }
        }
    }
}
