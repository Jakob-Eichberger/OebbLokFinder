using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class App : Application
{

    public App(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        InitializeComponent();

        using (var db = new Database())
        {
            db.Database.EnsureDeleted();
            db.Init();
        }
        MainPage = new MainPage(ServiceProvider);
    }

    public IServiceProvider ServiceProvider { get; }
}
