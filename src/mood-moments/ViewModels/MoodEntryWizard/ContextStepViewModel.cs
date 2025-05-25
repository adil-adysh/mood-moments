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
        public ObservableCollection<string> ContextOptions { get; } = new();
        [ObservableProperty]
        private string? context;
        public ICommand SelectContextCommand { get; }

        public ContextStepViewModel(ContextAndTriggersService contextService)
        {
            _contextService = contextService;
            foreach (var ctx in             _contextService.Data.Domains.SelectMany(d => d.Contexts).Select(c => c.Name))
                ContextOptions.Add(ctx);
            SelectContextCommand = new RelayCommand<string?>(OnSelectContext);
        }

        private void OnSelectContext(string? selected)
        {
            Context = selected;
        }
    }
}
