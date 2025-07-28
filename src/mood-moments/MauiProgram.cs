using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Plugin.LocalNotification;

namespace mood_moments;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        // Register converters for XAML
        builder.Services.AddSingleton<Converters.SelectedToColorConverter>();
        builder.Services.AddSingleton<Converters.CategoryToColorConverter>();
        builder.Services.AddSingleton<Converters.NullToBoolConverter>();

        builder.UseLocalNotification();

        return builder.Build();
    }
}
