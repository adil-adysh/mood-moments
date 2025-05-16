using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class IntensityStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private int intensity = 3;
        public string[] IntensityLabels { get; } = { "Very Low", "Low", "Moderate", "High", "Very High" };

        public string IntensityLabel => IntensityLabels[Intensity - 1];

        partial void OnIntensityChanged(int value)
        {
            OnPropertyChanged(nameof(IntensityLabel));
        }
    }
}
