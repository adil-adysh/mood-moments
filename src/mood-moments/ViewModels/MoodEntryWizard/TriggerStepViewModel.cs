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
        public ObservableCollection<string> TriggerOptions { get; } = new();
        [ObservableProperty]
        private string? trigger;
        public ICommand SelectTriggerCommand { get; }
        [ObservableProperty]
        private string? errorMessage;

        public TriggerStepViewModel(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            DomainFileName = domainFileName;
            SelectedContext = selectedContext;
            SelectTriggerCommand = new RelayCommand<string?>(OnSelectTrigger);
            _ = LoadTriggerOptionsAsync(contextService, domainFileName, selectedContext);
        }

        private async Task LoadTriggerOptionsAsync(ContextAndTriggersService contextService, string domainFileName, string? selectedContext)
        {
            try
            {
                var domain = await contextService.LoadDomainAsync(domainFileName);
                TriggerOptions.Clear();
                if (domain != null && domain.Contexts != null)
                {
                    var context = domain.Contexts.FirstOrDefault(c => c.Name == selectedContext);
                    if (context != null && context.Triggers != null)
                    {
                        if (context.Triggers.Positive != null)
                            foreach (var trig in context.Triggers.Positive)
                                TriggerOptions.Add(trig);
                        if (context.Triggers.Neutral != null)
                            foreach (var trig in context.Triggers.Neutral)
                                TriggerOptions.Add(trig);
                        if (context.Triggers.Negative != null)
                            foreach (var trig in context.Triggers.Negative)
                                TriggerOptions.Add(trig);
                    }
                }
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load triggers: {ex.Message}";
            }
        }

        private static void OnSelectTrigger(string? selected)
        {
            // This method is static due to analyzer requirements. Actual trigger selection logic should be handled via command binding or refactored if needed.
        }
    }
}
