using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mood_moments.Services;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class TriggerStepViewModel : ObservableObject
    {
        public string DomainFileName { get; }
        public string? SelectedContext { get; }
        public ObservableCollection<TriggerGroup> GroupedTriggers { get; } = new();
        [ObservableProperty]
        private string? errorMessage;

        public TriggerStepViewModel(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            DomainFileName = domainFileName;
            SelectedContext = selectedContext;
            _ = LoadTriggerOptionsAsync(contextService, domainFileName, selectedContext);
        }

        private async Task LoadTriggerOptionsAsync(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            try
            {
                var domain = await contextService.LoadDomainAsync(domainFileName);
                GroupedTriggers.Clear();
                if (domain != null && domain.Contexts != null)
                {
                    var context = domain.Contexts.FirstOrDefault(c => c.Name == selectedContext);
                    if (context != null && context.Triggers != null)
                    {
                        AddTriggerGroup("Positive", context.Triggers.Positive);
                        AddTriggerGroup("Neutral", context.Triggers.Neutral);
                        AddTriggerGroup("Negative", context.Triggers.Negative);
                    }
                }
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load triggers: {ex.Message}";
            }
        }

        private void AddTriggerGroup(string category, IEnumerable<string>? triggers)
        {
            if (triggers == null) return;
            var items = triggers.Select(t => new TriggerItem { Name = t }).ToList();
            if (items.Count > 0)
                GroupedTriggers.Add(new TriggerGroup { Category = category, Triggers = new ObservableCollection<TriggerItem>(items) });
        }

        public IEnumerable<string> SelectedTriggers => GroupedTriggers.SelectMany(g => g.Triggers).Where(t => t.IsSelected).Select(t => t.Name);
        public bool HasSelectedTriggers => SelectedTriggers.Any();

        public class TriggerGroup
        {
            public string Category { get; set; } = string.Empty;
            public ObservableCollection<TriggerItem> Triggers { get; set; } = new();
        }

        public class TriggerItem : ObservableObject
        {
            public string Name { get; set; } = string.Empty;
            [ObservableProperty]
            private bool isSelected;
        }
    }
}
