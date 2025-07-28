using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mood_moments.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace mood_moments.ViewModels.MoodEntryWizard
{
    public partial class DomainSelectionViewModel : ObservableObject
    {
        private readonly ContextAndTriggersService _service;
        public ObservableCollection<mood_moments.Models.DomainInfo> Domains { get; } = new();

        [ObservableProperty]
        private mood_moments.Models.DomainInfo? selectedDomain;

        public DomainSelectionViewModel(ContextAndTriggersService service)
        {
            _service = service;
            _ = LoadDomainsAsync();
        }

        private async Task LoadDomainsAsync()
        {
            var list = await _service.ListAvailableDomainsAsync();
            Domains.Clear();
            foreach (var d in list)
                Domains.Add(d);
        }
    }
}
