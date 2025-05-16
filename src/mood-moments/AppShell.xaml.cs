using mood_moments.Views;

namespace mood_moments;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("NewEntryWizardPage", typeof(Views.NewEntryWizardPage));
	}
}
