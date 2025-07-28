using CommunityToolkit.Mvvm.ComponentModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class TriggerItem : ObservableObject
    {
        public string Name { get; set; } = string.Empty;
        [ObservableProperty]
        private bool isSelected;
    }
}
