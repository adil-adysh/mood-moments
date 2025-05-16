using mood_moments.Models;
using mood_moments.Views.MoodEntryWizard;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace mood_moments.Views
{
    public partial class NewEntryWizardPage : ContentPage
    {
        private int currentStep = 0;
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
            NextButton.IsVisible = currentStep >= 3;

            switch (currentStep)
            {
                case 0:
                    var coreStep = new EmotionStep();
                    coreStep.SetEmotions("Step 1: Core Emotion", coreToMid.Keys, selectedCoreEmotion);
                    coreStep.EmotionSelected += (s, emotion) => {
                        selectedCoreEmotion = emotion;
                        selectedMidEmotion = string.Empty;
                        selectedNuancedEmotion = string.Empty;
                        currentStep = 1;
                        ShowStep();
                    };
                    coreStep.SkipRequested += (s, e) => { currentStep = 3; ShowStep(); };
                    StepContent.Children.Add(coreStep);
                    break;
                case 1:
                    var midStep = new EmotionStep();
                    midStep.SetEmotions("Step 2: Secondary Emotion", coreToMid[selectedCoreEmotion], selectedMidEmotion);
                    midStep.EmotionSelected += (s, emotion) => {
                        selectedMidEmotion = emotion;
                        selectedNuancedEmotion = string.Empty;
                        if (midToNuanced.ContainsKey(selectedCoreEmotion) && midToNuanced[selectedCoreEmotion].ContainsKey(emotion))
                            currentStep = 2;
                        else
                            currentStep = 3;
                        ShowStep();
                    };
                    midStep.SkipRequested += (s, e) => { currentStep = 3; ShowStep(); };
                    StepContent.Children.Add(midStep);
                    break;
                case 2:
                    var nuancedStep = new EmotionStep();
                    nuancedStep.SetEmotions("Step 3: Nuanced Emotion", midToNuanced[selectedCoreEmotion][selectedMidEmotion], selectedNuancedEmotion);
                    nuancedStep.EmotionSelected += (s, emotion) => {
                        selectedNuancedEmotion = emotion;
                        currentStep = 3;
                        ShowStep();
                    };
                    nuancedStep.SkipRequested += (s, e) => { currentStep = 3; ShowStep(); };
                    StepContent.Children.Add(nuancedStep);
                    break;
                case 3:
                    var intensityStep = new IntensityStep();
                    intensityStep.SetIntensities(intensities, selectedIntensity);
                    intensityStep.IntensitySelected += (s, intensity) => selectedIntensity = intensity;
                    StepContent.Children.Add(intensityStep);
                    break;
                case 4:
                    var noteStep = new NoteStep();
                    noteStep.SetNote(personalNote);
                    noteStep.NoteChanged += (s, note) => personalNote = note;
                    StepContent.Children.Add(noteStep);
                    break;
                case 5:
                    var contextStep = new ContextStep();
                    contextStep.SetContext(contextValue);
                    contextStep.ContextChanged += (s, ctx) => contextValue = ctx;
                    StepContent.Children.Add(contextStep);
                    break;
                case 6:
                    var triggerStep = new TriggerStep();
                    triggerStep.SetTrigger(triggersValue);
                    triggerStep.TriggerChanged += (s, trig) => triggersValue = trig;
                    StepContent.Children.Add(triggerStep);
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
            if (currentStep < 6)
            {
                currentStep++;
                ShowStep();
            }
            else
            {
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
