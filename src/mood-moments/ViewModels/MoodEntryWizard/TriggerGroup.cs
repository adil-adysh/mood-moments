using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public class TriggerGroup : IEnumerable<TriggerItem>
    {
        public string Category { get; set; } = string.Empty;
        public ObservableCollection<TriggerItem> Triggers { get; set; } = new();

        public IEnumerator<TriggerItem> GetEnumerator() => Triggers.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
