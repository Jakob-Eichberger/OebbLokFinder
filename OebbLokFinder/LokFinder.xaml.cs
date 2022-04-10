using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Model;
using Service;
using System.ComponentModel;

namespace OebbLokFinder;

public partial class LokFinder : ContentPage, INotifyPropertyChanged
{
    public LokFinder(IServiceProvider serviceProvider)
    {
        Db = serviceProvider.GetRequiredService<Database>();
        WebService = serviceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        InitializeComponent();
        Appearing += (a, b) => OnPropertyChanged(nameof(Stations));
    }

    public Database Db { get; set; }

    public string Station { get; set; }

    public List<string> Stations => Db.Stops.Where(e => (e.Arrival ?? e.Departure) >= DateTime.Now).Select(e => e.Station.StationName).Distinct().OrderBy(e => e).ToList();

    public OebbWebService WebService { get; set; }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        VSLStops.Clear();
        var stops = Db.Stops.Include(e => e.Vehicle).ToList().Where(e => e.Station.StationName == Station && (e.Arrival ?? e.Departure) >= DateTime.Now).OrderBy(e => e.Arrival ?? e.Departure);
        foreach (var stop in stops)
        {
            VSLStops.Add(new StopView(stop));
        }
    }
}