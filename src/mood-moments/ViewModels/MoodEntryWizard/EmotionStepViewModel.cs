using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class EmotionStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? stepTitle;
        [ObservableProperty]
        private ObservableCollection<string> emotions = new();
        [ObservableProperty]
        private string? selectedEmotion;

        [RelayCommand]
        void SelectEmotion(string emotion)
        {
            SelectedEmotion = emotion;
        }
    }
}
