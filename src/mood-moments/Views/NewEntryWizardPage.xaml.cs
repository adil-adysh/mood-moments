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
        private string selectedCoreEmotion = string.Empty;
        private string selectedMidEmotion = string.Empty;
        private string selectedNuancedEmotion = string.Empty;
        private string selectedIntensity = string.Empty;
        private string personalNote = string.Empty;
        private string contextValue = string.Empty;
        private string triggersValue = string.Empty;

        private static readonly Dictionary<string, List<string>> coreToMid = new()
        {
            { "Joy", new List<string> { "Content", "Proud", "Cheerful", "Grateful", "Hopeful", "Excited" } },
            { "Sadness", new List<string> { "Disappointed", "Lonely", "Hurt", "Guilt", "Discouraged", "Depressed" } },
            { "Fear", new List<string> { "Insecure", "Anxious", "Nervous", "Helpless", "Rejected", "Scared" } },
            { "Anger", new List<string> { "Frustrated", "Resentful", "Annoyed", "Hostile", "Irritated", "Bitter" } },
            { "Disgust", new List<string> { "Disapproval", "Judgmental", "Disdain", "Contempt", "Embarrassed", "Uncomfortable" } },
            { "Surprise", new List<string> { "Shocked", "Confused", "Amazed", "Startled", "Intrigued", "Disoriented" } },
        };
        private static readonly Dictionary<string, Dictionary<string, List<string>>> midToNuanced = new()
        {
            { "Joy", new Dictionary<string, List<string>> { { "Excited", new List<string> { "Energetic", "Ecstatic", "Thrilled" } } } },
            { "Sadness", new Dictionary<string, List<string>> { { "Hurt", new List<string> { "Rejected", "Heartbroken", "Grieving" } } } },
            { "Fear", new Dictionary<string, List<string>> { { "Anxious", new List<string> { "Worried", "Panicked", "Apprehensive" } } } },
            { "Anger", new Dictionary<string, List<string>> { { "Frustrated", new List<string> { "Agitated", "Impatient", "Provoked" } } } },
            { "Disgust", new Dictionary<string, List<string>> { { "Embarrassed", new List<string> { "Ashamed", "Self-conscious", "Awkward" } } } },
            { "Surprise", new Dictionary<string, List<string>> { { "Confused", new List<string> { "Perplexed", "Disoriented", "Puzzled" } } } },
        };

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
            // Only show Next button for steps after emotion selection
            NextButton.IsVisible = currentStep >= 3;
            if (currentStep == 0 || currentStep == 1 || currentStep == 2)
            {
                var skipBtn = new Button { Text = "Skip to Intensity", Margin = new Thickness(0, 10, 0, 0) };
                skipBtn.Clicked += (s, e) => { currentStep = 3; ShowStep(); };
                StepContent.Children.Add(skipBtn);
            }

            switch (currentStep)
            {
                case 0:
                    StepTitle.Text = "Step 1: Select Core Emotion";
                    var coreGrid = new Grid { ColumnSpacing = 10, RowSpacing = 10 };
                    int col = 0, row = 0;
                    foreach (var core in coreToMid.Keys)
                    {
                        var btn = new Button { Text = core, BackgroundColor = (selectedCoreEmotion == core) ? Colors.LightBlue : Colors.LightGray };
                        btn.Clicked += (s, e) => {
                            selectedCoreEmotion = core;
                            selectedMidEmotion = string.Empty;
                            selectedNuancedEmotion = string.Empty;
                            currentStep = 1;
                            ShowStep();
                        };
                        coreGrid.Add(btn, col, row);
                        col++;
                        if (col > 2) { col = 0; row++; }
                    }
                    StepContent.Children.Add(coreGrid);
                    break;
                case 1:
                    StepTitle.Text = "Step 2: Select Secondary Emotion";
                    if (string.IsNullOrEmpty(selectedCoreEmotion))
                    {
                        StepContent.Children.Add(new Label { Text = "Please select a core emotion first." });
                        break;
                    }
                    var midGrid = new Grid { ColumnSpacing = 10, RowSpacing = 10 };
                    col = 0; row = 0;
                    foreach (var mid in coreToMid[selectedCoreEmotion])
                    {
                        var btn = new Button { Text = mid, BackgroundColor = (selectedMidEmotion == mid) ? Colors.LightBlue : Colors.LightGray };
                        btn.Clicked += (s, e) => {
                            selectedMidEmotion = mid;
                            selectedNuancedEmotion = string.Empty;
                            // If there are nuanced emotions, go to nuanced, else go to intensity
                            if (midToNuanced.ContainsKey(selectedCoreEmotion) && midToNuanced[selectedCoreEmotion].ContainsKey(mid))
                                currentStep = 2;
                            else
                                currentStep = 3;
                            ShowStep();
                        };
                        midGrid.Add(btn, col, row);
                        col++;
                        if (col > 2) { col = 0; row++; }
                    }
                    StepContent.Children.Add(midGrid);
                    break;
                case 2:
                    StepTitle.Text = "Step 3: Select Nuanced Emotion";
                    if (string.IsNullOrEmpty(selectedCoreEmotion) || string.IsNullOrEmpty(selectedMidEmotion) || !midToNuanced.ContainsKey(selectedCoreEmotion) || !midToNuanced[selectedCoreEmotion].ContainsKey(selectedMidEmotion))
                    {
                        StepContent.Children.Add(new Label { Text = "No nuanced emotions for this selection. Skipping..." });
                        var skip = new Button { Text = "Skip to Intensity" };
                        skip.Clicked += (s, e) => { currentStep = 3; ShowStep(); };
                        StepContent.Children.Add(skip);
                        break;
                    }
                    var nuancedGrid = new Grid { ColumnSpacing = 10, RowSpacing = 10 };
                    col = 0; row = 0;
                    foreach (var nuanced in midToNuanced[selectedCoreEmotion][selectedMidEmotion])
                    {
                        var btn = new Button { Text = nuanced, BackgroundColor = (selectedNuancedEmotion == nuanced) ? Colors.LightBlue : Colors.LightGray };
                        btn.Clicked += (s, e) => {
                            selectedNuancedEmotion = nuanced;
                            currentStep = 3;
                            ShowStep();
                        };
                        nuancedGrid.Add(btn, col, row);
                        col++;
                        if (col > 2) { col = 0; row++; }
                    }
                    StepContent.Children.Add(nuancedGrid);
                    break;
                case 3:
                    StepTitle.Text = "Step 4: Select Intensity";
                    var intensityPicker = new Picker { Title = "Intensity" };
                    intensityPicker.ItemsSource = intensities;
                    if (!string.IsNullOrEmpty(selectedIntensity))
                        intensityPicker.SelectedItem = selectedIntensity;
                    intensityPicker.SelectedIndexChanged += (s, e) => selectedIntensity = (string)intensityPicker.SelectedItem;
                    StepContent.Children.Add(intensityPicker);
                    break;
                case 4:
                    StepTitle.Text = "Step 5: Add a Personal Note";
                    var noteEntry = new Editor { Placeholder = "Write your note here...", AutoSize = EditorAutoSizeOption.TextChanges, HeightRequest = 100, Text = personalNote };
                    noteEntry.TextChanged += (s, e) => personalNote = e.NewTextValue;
                    StepContent.Children.Add(noteEntry);
                    break;
                case 5:
                    StepTitle.Text = "Step 6: Context";
                    var contextEntry = new Entry { Placeholder = "Where are you? (e.g., Home, Work)", Text = contextValue };
                    contextEntry.TextChanged += (s, e) => contextValue = e.NewTextValue;
                    StepContent.Children.Add(contextEntry);
                    break;
                case 6:
                    StepTitle.Text = "Step 7: Triggers";
                    var triggersEntry = new Entry { Placeholder = "What triggered this mood?", Text = triggersValue };
                    triggersEntry.TextChanged += (s, e) => triggersValue = e.NewTextValue;
                    StepContent.Children.Add(triggersEntry);
                    break;
            }
        }
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                if (currentStep == 3 && !string.IsNullOrEmpty(selectedMidEmotion) && midToNuanced.ContainsKey(selectedCoreEmotion) && midToNuanced[selectedCoreEmotion].ContainsKey(selectedMidEmotion))
                    currentStep = 2;
                else
                    currentStep--;
                ShowStep();
            }
        }
        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            // Wizard step logic
            if (currentStep == 0 && !string.IsNullOrEmpty(selectedCoreEmotion))
            {
                currentStep = 1;
                ShowStep();
                return;
            }
            if (currentStep == 1 && !string.IsNullOrEmpty(selectedMidEmotion))
            {
                if (midToNuanced.ContainsKey(selectedCoreEmotion) && midToNuanced[selectedCoreEmotion].ContainsKey(selectedMidEmotion))
                {
                    currentStep = 2;
                }
                else
                {
                    currentStep = 3;
                }
                ShowStep();
                return;
            }
            if (currentStep == 2 && !string.IsNullOrEmpty(selectedNuancedEmotion))
            {
                currentStep = 3;
                ShowStep();
                return;
            }
            if (currentStep == 3)
            {
                currentStep = 4;
                ShowStep();
                return;
            }
            if (currentStep == 4)
            {
                currentStep = 5;
                ShowStep();
                return;
            }
            if (currentStep == 5)
            {
                currentStep = 6;
                ShowStep();
                return;
            }
            if (currentStep == 6)
            {
                // Save entry
                var entry = new MoodJournalEntry
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Mood = !string.IsNullOrEmpty(selectedNuancedEmotion) ? selectedNuancedEmotion : !string.IsNullOrEmpty(selectedMidEmotion) ? selectedMidEmotion : selectedCoreEmotion,
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
