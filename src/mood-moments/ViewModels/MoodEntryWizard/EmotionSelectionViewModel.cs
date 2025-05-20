using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mood_moments.Models;
using System.Collections.ObjectModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class EmotionSelectionViewModel : ObservableObject
    {
        private readonly EmotionHierarchy emotionHierarchy;

        public ObservableCollection<string> CoreEmotions { get; } = new();
        public ObservableCollection<string> MidEmotions { get; } = new();
        public ObservableCollection<string> NuancedEmotions { get; } = new();

        [ObservableProperty]
        private string? selectedCoreEmotion;
        [ObservableProperty]
        private string? selectedMidEmotion;
        [ObservableProperty]
        private string? selectedNuancedEmotion;

        public EmotionSelectionViewModel(EmotionHierarchy hierarchy)
        {
            emotionHierarchy = hierarchy;
            CoreEmotions.Clear();
            foreach (var core in emotionHierarchy.GetCoreEmotions())
                CoreEmotions.Add(core);
        }

        [RelayCommand]
        public void SelectCoreEmotion(string emotion)
        {
            SelectedCoreEmotion = emotion;
            MidEmotions.Clear();
            NuancedEmotions.Clear();
            foreach (var mid in emotionHierarchy.GetMidLevelEmotions(emotion))
                MidEmotions.Add(mid);
        }

        [RelayCommand]
        public void SelectMidEmotion(string emotion)
        {
            SelectedMidEmotion = emotion;
            NuancedEmotions.Clear();
            foreach (var nuance in emotionHierarchy.GetNuancedEmotions(SelectedCoreEmotion!, emotion))
                NuancedEmotions.Add(nuance);
        }

        [RelayCommand]
        public void SelectNuancedEmotion(string emotion)
        {
            SelectedNuancedEmotion = emotion;
        }
    }
}
