using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class WizardNavigationViewModel : ObservableObject
    {
        [ObservableProperty]
        private int currentStep = 0;

        public int StepCount { get; set; } = 7;
        public event Action? WizardFinished;

        [RelayCommand]
        public void NextStep()
        {
            if (CurrentStep < StepCount - 1)
                CurrentStep++;
        }

        [RelayCommand]
        public void BackStep()
        {
            if (CurrentStep > 0)
                CurrentStep--;
        }

        [RelayCommand]
        public void SkipToIntensity()
        {
            // Intensity step is index 3
            CurrentStep = 3;
        }

        [RelayCommand]
        public void Finish()
        {
            CurrentStep = 0;
            WizardFinished?.Invoke();
        }
    }
}
