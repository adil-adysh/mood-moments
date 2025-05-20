using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using mood_moments.Models;
using mood_moments.Views.MoodEntryWizard;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;
using mood_moments.ViewModels.MoodEntryWizard;

namespace mood_moments.ViewModels
{
    public partial class NewEntryWizardViewModel : ObservableObject
    {
        public EmotionSelectionViewModel? EmotionSelection { get; private set; }

        public WizardNavigationViewModel Navigation { get; }

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

        public NewEntryWizardViewModel()
        {
            Navigation = new WizardNavigationViewModel();

            // 👇 Relay step changes to the UI
            Navigation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Navigation.CurrentStep))
                {
                    OnPropertyChanged(nameof(CurrentStep));
                    OnPropertyChanged(nameof(StepTitle));
                }
            };

            _ = LoadEmotionHierarchyAsync();
        }

        private async Task LoadEmotionHierarchyAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("EmotionsData.json");
                var hierarchy = await EmotionHierarchy.LoadFromJsonAsync(stream);
                EmotionSelection = new EmotionSelectionViewModel(hierarchy);
                OnPropertyChanged(nameof(EmotionSelection));
            }
            catch (Exception ex)
            {
                Application.Current?.MainPage?.DisplayAlert("Error", $"Failed to load emotions: {ex.Message}", "OK");
            }
        }

        // 👉 For UI binding
        public int CurrentStep => Navigation.CurrentStep;
        public string StepTitle => StepTitles.ElementAtOrDefault(CurrentStep) ?? string.Empty;

        public event Action? WizardFinished;

        [RelayCommand]
        void Finish() => WizardFinished?.Invoke();

        public IRelayCommand? SelectCoreEmotionCommand => EmotionSelection?.SelectCoreEmotionCommand;
        public IRelayCommand? SelectMidEmotionCommand => EmotionSelection?.SelectMidEmotionCommand;
        public IRelayCommand? SelectNuancedEmotionCommand => EmotionSelection?.SelectNuancedEmotionCommand;

        public IRelayCommand NextStepCommand => Navigation.NextStepCommand;
        public IRelayCommand? BackStepCommand => Navigation.BackStepCommand;
        public IRelayCommand? SkipToIntensityCommand => Navigation.SkipToIntensityCommand;
    }
}
