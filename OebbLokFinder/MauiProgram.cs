namespace OebbLokFinder;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Service;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });
        builder.Services.AddDbContext<Database>();
        builder.Services.AddSingleton<OebbWebService>();
        return builder.Build();
    }
}
