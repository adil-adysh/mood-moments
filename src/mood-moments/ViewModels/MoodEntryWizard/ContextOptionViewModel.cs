using CommunityToolkit.Mvvm.ComponentModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class ContextOptionViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        public ContextOptionViewModel(string name)
        {
            this.name = name;
        }
    }
}
