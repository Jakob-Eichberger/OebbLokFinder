using Microsoft.Extensions.Logging;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;

namespace OebbLokFinder
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<Database>();
            builder.Services.AddSingleton<OebbWebService>();
            builder.Services.AddSingleton<StationService>();
            builder.Services.AddSingleton<VehicleService>();
            builder.Services.AddSingleton<SettingService>();

            ServiceProvider i = builder.Services.BuildServiceProvider();
            return builder.Build();
        }
    }
}
