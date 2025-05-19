using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class NuanceEmotionStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? selectedNuancedEmotion;

        public ObservableCollection<string> NuancedEmotions { get; } = new();

        [RelayCommand]
        void SelectNuancedEmotion(string emotion)
        {
            SelectedNuancedEmotion = emotion;
        }
    }
}
