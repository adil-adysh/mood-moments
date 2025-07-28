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
        private ObservableCollection<TriggerGroup> _groupedTriggers = new();
        public ObservableCollection<TriggerGroup> GroupedTriggers
        {
            get => _groupedTriggers;
            private set => SetProperty(ref _groupedTriggers, value);
        }
        [ObservableProperty]
        private string? errorMessage;


        private ContextAndTriggersService? _contextService;
        public TriggerStepViewModel(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            DomainFileName = domainFileName;
            SelectedContext = selectedContext;
            _contextService = contextService;
            System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Constructed with DomainFileName='{domainFileName}', SelectedContext='{selectedContext}'");
        }

        public async Task LoadAsync()
        {
            if (_contextService != null)
                await LoadTriggerOptionsAsync(_contextService, DomainFileName, SelectedContext);
        }

        private async Task LoadTriggerOptionsAsync(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Loading triggers for domain '{domainFileName}', context '{selectedContext}'");
                var domain = await contextService.LoadDomainAsync(domainFileName);
                var newGroups = new List<TriggerGroup>();
                if (domain == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Domain not found for file: {domainFileName}");
                }
                else if (domain.Contexts == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] No contexts in domain: {domain?.Name}");
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
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Positive triggers: {string.Join(", ", context.Triggers.Positive ?? Enumerable.Empty<string>())}");
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Neutral triggers: {string.Join(", ", context.Triggers.Neutral ?? Enumerable.Empty<string>())}");
                        System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Negative triggers: {string.Join(", ", context.Triggers.Negative ?? Enumerable.Empty<string>())}");
                        AddTriggerGroupToList(newGroups, "Positive", context.Triggers.Positive);
                        AddTriggerGroupToList(newGroups, "Neutral", context.Triggers.Neutral);
                        AddTriggerGroupToList(newGroups, "Negative", context.Triggers.Negative);
                    }
                }
                ErrorMessage = null;
                var dispatcher = Microsoft.Maui.Controls.Application.Current?.Dispatcher;
                if (dispatcher != null)
                {
                    await dispatcher.DispatchAsync(() =>
                    {
                        GroupedTriggers = new ObservableCollection<TriggerGroup>(newGroups);
                    });
                }
                else
                {
                    GroupedTriggers = new ObservableCollection<TriggerGroup>(newGroups);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Exception: {ex}");
                ErrorMessage = $"Failed to load triggers: {ex.Message}";
            }
        }

        private void AddTriggerGroupToList(List<TriggerGroup> groupList, string category, IEnumerable<string>? triggers)
        {
            if (triggers == null)
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Skipping group '{category}' because triggers is null.");
                return;
            }
            var filtered = triggers.Where(t => !string.IsNullOrEmpty(t)).ToList();
            foreach (var t in triggers)
            {
                if (string.IsNullOrEmpty(t))
                    System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Skipping null/empty trigger in group '{category}'.");
            }
            var items = filtered.Select(t => new TriggerItem { Name = t! }).ToList();
            if (items.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] Adding group '{category}' with {items.Count} triggers.");
                groupList.Add(new TriggerGroup { Category = category, Triggers = new ObservableCollection<TriggerItem>(items) });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[TriggerStepViewModel] No valid triggers for group '{category}'. Group not added.");
            }
        }

        // replaced by AddTriggerGroupToList

        public IEnumerable<string> SelectedTriggers => GroupedTriggers.SelectMany(g => g.Triggers).Where(t => t.IsSelected).Select(t => t.Name);
        public bool HasSelectedTriggers => SelectedTriggers.Any();

        // ...existing code...
    }
}
