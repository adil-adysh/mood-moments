using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class MidEmotionStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? selectedMidEmotion;

        public ObservableCollection<string> MidEmotions { get; } = new();

        [RelayCommand]
        void SelectMidEmotion(string emotion)
        {
            SelectedMidEmotion = emotion;
        }
    }
}
