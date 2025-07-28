using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class IntensityStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private int selectedIntensity = 3;

        [ObservableProperty]
        private string stepTitle = "How intense is your feeling?";

        public string[] IntensityLabels { get; } = { "Very Low", "Low", "Moderate", "High", "Very High" };

        public string IntensityLabel => IntensityLabels[SelectedIntensity - 1];

        partial void OnSelectedIntensityChanged(int value)
        {
            OnPropertyChanged(nameof(IntensityLabel));
        }
    }
}
