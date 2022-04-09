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

    public List<string> Stations => Db.Stops.Select(e => e.Station.StationName).Distinct().OrderBy(e => e).ToList();

    public OebbWebService WebService { get; set; }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        VSLStops.Clear();
        var stops = Db.Stops.Include(e => e.Vehicle).Where(e => e.Station.StationName == Station).OrderBy(e => e.Arrival ?? e.Departure);
        foreach (var stop in stops)
        {
            //VSLStops.Add(new Stop(stop));
            VSLStops.Add(new StopView(stop));
        }
    }
}

public class Stop : Frame
{
    public Stop(Model.Stop stop)
    {
        Padding = new Thickness(8);
        Margin = new Thickness(5);

        var c = new StackLayout
        {
            new Label{Text = $"{stop.Vehicle.ClassNumber}.{stop.Vehicle.SerialNumber}"},
        };
        if (!string.IsNullOrWhiteSpace(stop.Vehicle.Name))
        {
            c.Add(new Label() { Text = $"{stop.Vehicle.Name}" });
        }
        if (stop.Arrival is DateTime a)
        {
            c.Add(new Label() { Text = $"Arrival:\t{TimeZoneInfo.ConvertTimeFromUtc(a, TimeZoneInfo.Local):dd/MM/yy hh:mm}" });
        }
        c.Add(new Label() { Text = $"Departure: {TimeZoneInfo.ConvertTimeFromUtc(stop.Departure, TimeZoneInfo.Local):dd/MM/yy hh:mm}" });
        Content = c;
    }
}