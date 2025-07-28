using CommunityToolkit.Mvvm.ComponentModel;
using mood_moments.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class ContextStepViewModel : ObservableObject
    {
        private readonly ContextAndTriggersService _contextService;
        public string DomainFileName { get; }
        public ObservableCollection<string> ContextOptions { get; } = new();
        [ObservableProperty]
        private string? context;
        // Error message for UI binding
        [ObservableProperty]
        private string? errorMessage;

        public ContextStepViewModel(ContextAndTriggersService contextService, string domainFileName)
        {
            _contextService = contextService;
            DomainFileName = domainFileName;
            _ = LoadContextOptionsAsync(domainFileName);
        }

        private async Task LoadContextOptionsAsync(string domainFileName)
        {
            try
            {
                var domain = await _contextService.LoadDomainAsync(domainFileName);
                ContextOptions.Clear();
                if (domain != null && domain.Contexts != null)
                {
                    foreach (var ctx in domain.Contexts)
                    {
                        if (!string.IsNullOrEmpty(ctx?.Name))
                            ContextOptions.Add(ctx.Name!);
                    }
                }
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load contexts: {ex.Message}";
            }
        }
    }
}
