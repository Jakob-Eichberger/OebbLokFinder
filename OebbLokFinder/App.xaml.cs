using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class App : Application
{
    public Database Database { get; set; }

    public OebbWebService OebbWebService { get; set; }

    public App(Database database, OebbWebService oebbWebService)
    {
        Database = database;
        OebbWebService = oebbWebService;
        InitializeComponent();

        using (var db = new Database())
        {
            db.Database.EnsureDeleted();
            if (db.Database.EnsureCreated())
            {
            }
        }
        MainPage = new MainPage(database, oebbWebService);
    }

}
