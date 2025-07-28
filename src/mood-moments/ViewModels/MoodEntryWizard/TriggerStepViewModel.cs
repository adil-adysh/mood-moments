using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mood_moments.Services;
using mood_moments.ViewModels.MoodEntryWizard;

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
                if (domain == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Domain not found for file: {domainFileName}");
                }
                else if (domain.Contexts == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] No contexts in domain: {domain.Name}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Looking for context: '{selectedContext}' in domain: {domain.Name}");
                    foreach (var ctx in domain.Contexts)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Available context: '{ctx.Name}'");
                    }
                    var context = domain.Contexts.FirstOrDefault(c => c.Name == selectedContext);
                    if (context == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Context '{selectedContext}' not found in domain '{domain.Name}'.");
                    }
                    else if (context.Triggers == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] No triggers for context '{context.Name}'.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Loading triggers for context '{context.Name}'.");
                        AddTriggerGroup("Positive", context.Triggers.Positive);
                        AddTriggerGroup("Neutral", context.Triggers.Neutral);
                        AddTriggerGroup("Negative", context.Triggers.Negative);
                    }
                }
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Exception: {ex}");
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

        // ...existing code...
    }
}
