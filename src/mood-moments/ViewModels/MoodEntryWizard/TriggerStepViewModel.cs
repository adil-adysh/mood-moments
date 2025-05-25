using CommunityToolkit.Mvvm.ComponentModel;
using mood_moments.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class TriggerStepViewModel : ObservableObject
    {
        public ObservableCollection<string> TriggerOptions { get; } = new();
        [ObservableProperty]
        private string? trigger;
        public ICommand SelectTriggerCommand { get; }

        public TriggerStepViewModel(ContextAndTriggersService contextService, string? selectedContext)
        {
            var context = contextService.Data.Domains.SelectMany(d => d.Contexts).FirstOrDefault(c => c.Name == selectedContext);
            if (context != null)
            {
                foreach (var trig in context.Triggers.Positive)
                    TriggerOptions.Add(trig);
                foreach (var trig in context.Triggers.Neutral)
                    TriggerOptions.Add(trig);
                foreach (var trig in context.Triggers.Negative)
                    TriggerOptions.Add(trig);
            }
            SelectTriggerCommand = new RelayCommand<string?>(OnSelectTrigger);
        }

        private void OnSelectTrigger(string? selected)
        {
            Trigger = selected;
        }
    }
}
