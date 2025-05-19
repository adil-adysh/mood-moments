using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using mood_moments.Models;
using mood_moments.Views.MoodEntryWizard;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;

namespace mood_moments.ViewModels
{
    public partial class NewEntryWizardViewModel : ObservableObject
    {
        public NewEntryWizardViewModel()
        {
            _ = LoadEmotionHierarchy();
        }

        // Change signature to return Task
        private async Task LoadEmotionHierarchy()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("EmotionsData.json");
                emotionHierarchy = await EmotionHierarchy.LoadFromJsonAsync(stream);
                CoreEmotions.Clear();
                foreach (var core in emotionHierarchy.GetCoreEmotions())
                    CoreEmotions.Add(core);
            }
            catch (Exception ex)
            {
                Application.Current?.MainPage?.DisplayAlert("Error", $"Failed to load emotions: {ex.Message}", "OK");
            }
        }

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

        public ObservableCollection<string> CoreEmotions { get; private set; } = new();
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

        public string StepTitle => StepTitles.Count > CurrentStep ? StepTitles[CurrentStep] : string.Empty;

        private EmotionHierarchy? emotionHierarchy;

        public View? CurrentStepView
        {
            get
            {
                return CurrentStep switch
                {
                    0 => new EmotionStep { BindingContext = this },
                    1 => new EmotionStep { BindingContext = this },
                    2 => new EmotionStep { BindingContext = this },
                    3 => new IntensityStep { BindingContext = this },
                    4 => new NoteStep { BindingContext = this },
                    5 => new ContextStep { BindingContext = this },
                    6 => new TriggerStep { BindingContext = this },
                    _ => null
                };
            }
        }

        public int StepCount => StepTitles.Count;
        public bool CanGoNext => CurrentStep < StepCount - 1;
        public bool IsFinishVisible => CurrentStep == StepCount - 1;

        [RelayCommand]
        void NextStep()
        {
            if (CurrentStep < StepCount - 1)
            {
                CurrentStep++;
                UpdateStepProperties();
            }
        }

        public event Action? WizardFinished;

        [RelayCommand]
        void Finish()
        {
            // Save the mood entry (example logic)
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
            UpdateStepProperties();
            WizardFinished?.Invoke();
        }

        private void UpdateStepProperties()
        {
            OnPropertyChanged(nameof(StepTitle));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(IsFinishVisible));
            OnPropertyChanged(nameof(CurrentStepView));
        }

        [RelayCommand]
        void SelectCoreEmotion(string emotion)
        {
            SelectedCoreEmotion = emotion;
            MidEmotions.Clear();
            NuancedEmotions.Clear();
            if (!string.IsNullOrEmpty(emotion) && emotionHierarchy != null)
            {
                foreach (var mid in emotionHierarchy.GetMidLevelEmotions(emotion))
                    MidEmotions.Add(mid);
            }
            // Removed automatic NextStep() call here
        }

        [RelayCommand]
        void SelectMidEmotion(string emotion)
        {
            SelectedMidEmotion = emotion;
            NuancedEmotions.Clear();
            if (!string.IsNullOrEmpty(SelectedCoreEmotion) && !string.IsNullOrEmpty(emotion) && emotionHierarchy != null)
            {
                foreach (var nuance in emotionHierarchy.GetNuancedEmotions(SelectedCoreEmotion, emotion))
                    NuancedEmotions.Add(nuance);
            }
            // Removed automatic NextStep() call
        }

        [RelayCommand]
        void SelectNuancedEmotion(string emotion)
        {
            SelectedNuancedEmotion = emotion;
            // Removed automatic NextStep() call
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

        [RelayCommand]
        void SkipToIntensity()
        {
            // Intensity step is index 3
            CurrentStep = 3;
            UpdateStepProperties();
        }
    }
}
