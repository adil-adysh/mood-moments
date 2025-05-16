using CommunityToolkit.Mvvm.ComponentModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class ContextStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? context;
    }
}
