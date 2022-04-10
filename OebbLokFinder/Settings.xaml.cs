using Service;

namespace OebbLokFinder;

public partial class Settings : ContentPage
{
    public Settings(IServiceProvider serviceProvider)
    {
        Setting = serviceProvider.GetService<SettingService>();
        InitializeComponent();
    }

    public SettingService Setting { get; }


}