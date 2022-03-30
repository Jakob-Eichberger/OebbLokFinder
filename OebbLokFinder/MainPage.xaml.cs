using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class MainPage : TabbedPage
{
    public Database Database { get; set; }

    public OebbWebService OebbWebService { get; set; }

    public MainPage(Database database, OebbWebService oebbWebService)
    {
        Database = database;
        OebbWebService = oebbWebService;
        InitializeComponent();
        Children.Add(new LokFinder(Database, OebbWebService));
        Children.Add(new VehicleManagment());
        Children.Add(new Settings());
    }
}

