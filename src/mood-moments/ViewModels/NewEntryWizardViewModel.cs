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
        [ObservableProperty]
        private int selectedIntensity;

        [ObservableProperty]
        private string? personalNote;

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
            "Select a Life Domain",
            "Context: Where are you?",
            "What triggered this feeling?"
        });


        // Track selected domain file and context name
        private string _selectedDomainFile = "WorkStudyAndPurpose.json";
        public string SelectedDomainFile
        {
            get => _selectedDomainFile;
            set
            {
                if (_selectedDomainFile != value)
                {
                    _selectedDomainFile = value;
                    OnPropertyChanged(nameof(SelectedDomainFile));
                    // Reset context and trigger when domain changes
                    SelectedContextName = null;
                }
            }
        }

        private string? _selectedContextName;
        public string? SelectedContextName
        {
            get => _selectedContextName;
            set
            {
                if (_selectedContextName != value)
                {
                    _selectedContextName = value;
                    OnPropertyChanged(nameof(SelectedContextName));
                    OnPropertyChanged(nameof(CanGoNext));
                }
            }
        }

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
                    NotifyNavigationStateChanged();
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
                var window = Application.Current?.Windows.FirstOrDefault();
                var page = window?.Page;
                if (page != null)
                {
                    _ = page.DisplayAlert("Error", $"Failed to load emotions: {ex.Message}", "OK");
                }
            }
        }

        // 👉 For UI binding

        public int CurrentStep => Navigation.CurrentStep;
        public string StepTitle => StepTitles.ElementAtOrDefault(CurrentStep) ?? string.Empty;

        // Navigation button visibility/enabling
        public bool ShowBackButton => CurrentStep > 0;
        public bool ShowNextButton => CurrentStep < StepTitles.Count - 1;
        public bool ShowFinishButton => CurrentStep == StepTitles.Count - 1;

        // Example: Only allow next if current step is valid (expand as needed)
        public bool CanGoNext
        {
            get
            {
                // Only allow next if a context is selected on the context step (step 6)
                if (CurrentStep == 6)
                    return !string.IsNullOrEmpty(SelectedContextName) && ShowNextButton;
                return ShowNextButton;
            }
        }

        public bool CanGoBack => ShowBackButton;
        public bool CanFinish => ShowFinishButton;

        // Notify UI when navigation state changes
        private void NotifyNavigationStateChanged()
        {
            OnPropertyChanged(nameof(ShowBackButton));
            OnPropertyChanged(nameof(ShowNextButton));
            OnPropertyChanged(nameof(ShowFinishButton));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanFinish));
        }

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
