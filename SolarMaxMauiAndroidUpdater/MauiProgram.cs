using Microsoft.Extensions.Logging;

namespace SolarMaxMauiAndroidUpdater;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Add logging
        builder.Logging.AddConsole();

        // Register ApiWorker
        builder.Services.AddTransient<ApiWorker>();

        return builder.Build();
	}
}
