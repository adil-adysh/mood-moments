using System.Collections.ObjectModel;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public class TriggerGroup
    {
        public string Category { get; set; } = string.Empty;
        public ObservableCollection<TriggerItem> Triggers { get; set; } = new();
    }
}
