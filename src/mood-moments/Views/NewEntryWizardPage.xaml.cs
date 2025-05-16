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
            FinishButton.IsVisible = false;

            if (currentStep == 0)
            {
                ShowCoreEmotionStep();
            }
            else if (currentStep == 1)
            {
                ShowMidEmotionStep();
            }
            else if (currentStep == 2)
            {
                ShowNuancedEmotionStep();
            }
            else if (currentStep == 3)
            {
                ShowIntensityStep();
            }
            else if (currentStep == 4)
            {
                ShowNoteStep();
            }
            else if (currentStep == 5)
            {
                ShowContextStep();
            }
            else if (currentStep == 6)
            {
                ShowTriggerStep();
                FinishButton.IsVisible = true;
            }
        }

        private void ShowCoreEmotionStep()
        {
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
        }

        private void ShowMidEmotionStep()
        {
            var midStep = new EmotionStep();
            if (!string.IsNullOrEmpty(selectedCoreEmotion) && coreToMid.ContainsKey(selectedCoreEmotion))
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
        }

        private void ShowNuancedEmotionStep()
        {
            var nuancedStep = new EmotionStep();
            if (!string.IsNullOrEmpty(selectedCoreEmotion) && !string.IsNullOrEmpty(selectedMidEmotion)
                && midToNuanced.ContainsKey(selectedCoreEmotion) && midToNuanced[selectedCoreEmotion].ContainsKey(selectedMidEmotion))
                nuancedStep.SetEmotions("Step 3: Nuanced Emotion", midToNuanced[selectedCoreEmotion][selectedMidEmotion], selectedNuancedEmotion);
            nuancedStep.EmotionSelected += (s, emotion) => {
                selectedNuancedEmotion = emotion;
                currentStep = 3;
                ShowStep();
            };
            nuancedStep.SkipRequested += (s, e) => { currentStep = 3; ShowStep(); };
            StepContent.Children.Add(nuancedStep);
        }

        private void ShowIntensityStep()
        {
            var intensityStep = new IntensityStep();
            if (!string.IsNullOrEmpty(selectedIntensity) && int.TryParse(selectedIntensity, out var val))
            {
                intensityStep.IntensitySliderControl.Value = val;
            }
            intensityStep.IntensitySelected += (s, intensity) => selectedIntensity = intensity;
            StepContent.Children.Add(intensityStep);
        }

        private void ShowNoteStep()
        {
            var noteStep = new NoteStep();
            noteStep.SetNote(personalNote);
            noteStep.NoteChanged += (s, note) => personalNote = note;
            StepContent.Children.Add(noteStep);
        }

        private void ShowContextStep()
        {
            var contextStep = new ContextStep();
            contextStep.SetContext(contextValue);
            contextStep.ContextChanged += (s, ctx) => contextValue = ctx;
            StepContent.Children.Add(contextStep);
        }

        private void ShowTriggerStep()
        {
            var triggerStep = new TriggerStep();
            triggerStep.SetTrigger(triggersValue);
            triggerStep.TriggerChanged += (s, trig) => triggersValue = trig;
            StepContent.Children.Add(triggerStep);
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

        private string GetSelectedMood()
        {
            if (!string.IsNullOrEmpty(selectedNuancedEmotion))
                return selectedNuancedEmotion;
            if (!string.IsNullOrEmpty(selectedMidEmotion))
                return selectedMidEmotion;
            return selectedCoreEmotion;
        }

        private async void FinishButton_Clicked(object sender, EventArgs e)
        {
            var entry = new MoodJournalEntry
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Mood = GetSelectedMood(),
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
