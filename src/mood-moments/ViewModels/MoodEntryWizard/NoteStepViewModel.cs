using CommunityToolkit.Mvvm.ComponentModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class NoteStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? note;
    }
}
