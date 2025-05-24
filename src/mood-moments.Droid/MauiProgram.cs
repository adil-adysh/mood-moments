using Plugin.LocalNotification;

namespace mood_moments.Droid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseSharedMauiApp()
                .UseLocalNotification();

            return builder.Build();
        }
    }
}
