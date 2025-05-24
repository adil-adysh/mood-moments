using Android.App;
using Android.Runtime;
using Android; // Add this to resolve 'Manifest'

// Minimum permissions
[assembly: UsesPermission(Manifest.Permission.Vibrate)]
[assembly: UsesPermission("android.permission.POST_NOTIFICATIONS")]

// Required so that the plugin can schedule
[assembly: UsesPermission(Manifest.Permission.WakeLock)]

//Required so that the plugin can reschedule notifications upon a reboot
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]

namespace mood_moments.Droid
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
