namespace mood_moments;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

#if ANDROID
		Plugin.LocalNotification.LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
#endif
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
