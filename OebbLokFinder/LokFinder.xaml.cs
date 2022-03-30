using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class LokFinder : ContentPage
{

    public LokFinder(Database database, OebbWebService oebbWebService)
    {
        Database = database;
        WebService = oebbWebService;
        InitializeComponent();
    }

    public Database Database { get; set; }

    public OebbWebService WebService { get; set; }

    private async void Button_Clicked(object sender, EventArgs e)
    {
       await WebService.GetStops(new Model.VehicleClassification(1116, 1));
    }
}