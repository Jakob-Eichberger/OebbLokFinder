using Microsoft.Extensions.Logging;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;

namespace OebbLokFinder.View
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

            builder.Services.AddDbContext<Database>()
                            .AddSingleton<OebbWebService>()
                            .AddSingleton<StationService>()
                            .AddSingleton<RollingstockService>()
                            .AddSingleton<SettingService>()
                            .AddLogging();

            ServiceProvider i = builder.Services.BuildServiceProvider();
            return builder.Build();
        }
    }
}
