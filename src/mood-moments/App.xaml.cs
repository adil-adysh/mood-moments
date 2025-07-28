namespace mood_moments;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
#if ANDROID
		Plugin.LocalNotification.LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
#endif
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

#if ANDROID
	private void OnNotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
	{
		if (e.Request.ReturningData == "reminder")
		{
			// Open the mood entry wizard page
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await Shell.Current.GoToAsync("//MainPage/NewEntryWizardPage");
			});
		}
	}
#endif
}
