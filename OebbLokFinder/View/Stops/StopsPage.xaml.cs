using Microsoft.EntityFrameworkCore;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;
using System.ComponentModel;

namespace OebbLokFinder.View.Stops;

public partial class StopsPage : ContentPage, INotifyPropertyChanged
{
    public StopsPage()
    {
        Db = App.ServiceProvider.GetRequiredService<Database>();
        WebService = App.ServiceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        InitializeComponent();
    }

    public Database Db { get; set; }

    public string Station { get; set; }

    public List<string> Stations => Db.Stops.Where(e => (e.Arrival ?? e.Departure) >= DateTime.Now).Select(e => e.Station.StationName).Distinct().OrderBy(e => e).ToList();

    public OebbWebService WebService { get; set; }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        VSLStops.Clear();
        var stops = Db.Stops.Include(e => e.Rollingstock).ToList().Where(e => e.Station.StationName == Station && (e.Arrival ?? e.Departure) >= DateTime.Now).OrderBy(e => e.Arrival ?? e.Departure);
        foreach (var stop in stops)
        {
            VSLStops.Add(new StopView(stop));
        }
    }
}