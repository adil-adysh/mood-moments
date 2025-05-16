using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using mood_moments.Models;

namespace mood_moments.ViewModels
{
    public partial class NewEntryWizardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int currentStep = 0;

        [ObservableProperty]
        private string? selectedCoreEmotion;
        [ObservableProperty]
        private string? selectedMidEmotion;
        [ObservableProperty]
        private string? selectedNuancedEmotion;
        [ObservableProperty]
        private string? selectedIntensity;
        [ObservableProperty]
        private string? personalNote;
        [ObservableProperty]
        private string? contextValue;
        [ObservableProperty]
        private string? triggersValue;

        public ObservableCollection<string> CoreEmotions { get; } = new(new[] { "Joy", "Sadness", "Fear", "Anger", "Disgust", "Surprise" });
        public ObservableCollection<string> MidEmotions { get; } = new();
        public ObservableCollection<string> NuancedEmotions { get; } = new();

        public ObservableCollection<string> IntensityLabels { get; } = new(new[] { "Very Low", "Low", "Moderate", "High", "Very High" });

        public ObservableCollection<string> StepTitles { get; } = new(new[]
        {
            "How are you feeling at your core?",
            "Can you be more specific about your feeling?",
            "Is there a more precise word for your feeling?",
            "How intense is your feeling?",
            "Add a personal note (optional)",
            "Context: Where are you?",
            "What triggered this feeling?"
        });

        public ObservableCollection<object> StepContents { get; } = new();

        public object? StepContent => StepContents.Count > CurrentStep ? StepContents[CurrentStep] : null;

        // Core to mid and mid to nuanced emotion mappings
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

        public ObservableCollection<string> ContextOptions { get; } = new(new[]
        {
            "Social Interaction",
            "Task or Activity",
            "Rest or Pause",
            "Internal Reflection",
            "Change or Event",
            "Anticipation",
            "Physical/Sensory",
            "Environment"
        });

        public ObservableCollection<string> TriggerOptions { get; } = new(new[]
        {
            "Exclusion / Rejection",
            "Criticism / Judgment",
            "Approval / Validation",
            "Disconnection",
            "Achievement / Failure",
            "Loss / Absence",
            "Uncertainty / Ambiguity",
            "Repetition / Pattern",
            "Self-Evaluation (Neg)",
            "Self-Evaluation (Pos)",
            "Violation of Expectation",
            "Physical State",
            "Relationship Conflict"
        });

        [RelayCommand]
        void NextStep()
        {
            if (CurrentStep < 6) CurrentStep++;
        }

        [RelayCommand]
        void BackStep()
        {
            if (CurrentStep > 0) CurrentStep--;
        }

        [RelayCommand]
        void Finish()
        {
            // Save the mood entry (example logic)
            string mood = SelectedCoreEmotion ?? string.Empty;
            if (!string.IsNullOrEmpty(SelectedNuancedEmotion))
                mood = SelectedNuancedEmotion;
            else if (!string.IsNullOrEmpty(SelectedMidEmotion))
                mood = SelectedMidEmotion;

            // Example: You could raise an event, call a service, or navigate away here.
            // For now, just clear the wizard state as a placeholder for completion.
            CurrentStep = 0;
            SelectedCoreEmotion = null;
            SelectedMidEmotion = null;
            SelectedNuancedEmotion = null;
            SelectedIntensity = null;
            PersonalNote = null;
            ContextValue = null;
            TriggersValue = null;
        }

        [RelayCommand]
        void SelectCoreEmotion(string emotion)
        {
            SelectedCoreEmotion = emotion;
            if (!string.IsNullOrEmpty(emotion))
                NextStep();
        }

        [RelayCommand]
        void SelectMidEmotion(string emotion)
        {
            SelectedMidEmotion = emotion;
            if (!string.IsNullOrEmpty(emotion))
                NextStep();
        }

        [RelayCommand]
        void SelectNuancedEmotion(string emotion)
        {
            SelectedNuancedEmotion = emotion;
            if (!string.IsNullOrEmpty(emotion))
                NextStep();
        }

        [RelayCommand]
        void SelectIntensity(string intensity)
        {
            SelectedIntensity = intensity;
            if (!string.IsNullOrEmpty(intensity))
                NextStep();
        }

        [RelayCommand]
        void SetPersonalNote(string note)
        {
            PersonalNote = note;
            NextStep();
        }

        [RelayCommand]
        void SelectContext(string context)
        {
            ContextValue = context;
            NextStep();
        }

        [RelayCommand]
        void SelectTrigger(string trigger)
        {
            TriggersValue = trigger;
            NextStep();
        }
    }
}
