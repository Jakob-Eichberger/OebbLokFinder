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
        SettingService = App.ServiceProvider.GetRequiredService<SettingService>();
        OebbWebService = App.ServiceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        InitializeComponent();
    }

    public Database Db { get; set; }
    public SettingService SettingService { get; set; }

    public string Station { get; set; }

    public List<string> Stations => Db.Stops.Where(e => (e.Arrival ?? e.Departure) >= DateTime.Now).Select(e => e.Station.StationName).Distinct().OrderBy(e => e).ToList();

    public OebbWebService OebbWebService { get; set; }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        VSLStops.Clear();
        var stops = Db.Stops.Include(e => e.Rollingstock).ToList().Where(e => e.Station.StationName == Station && (e.Arrival ?? e.Departure) >= DateTime.Now).OrderBy(e => e.Arrival ?? e.Departure);
        foreach (var stop in stops)
        {
            VSLStops.Add(new StopView(stop));
        }
    }

    private async void BtnUpdateStops_Pressed(object sender, EventArgs e)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        var stock = Db.Rollingstocks.AsEnumerable().Where(e => SettingService.Setting.RefreshRollingStockCycle > 0 || !e.Stops.Any() || (e.Stops.Any() && (e.Stops.Max(e => e.Departure) - DateTime.Now).TotalHours < SettingService.Setting.RollingStockDataPreloadMin));
        foreach (var item in stock)
        {
            await OebbWebService.UpdateStopsAsync(item);
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}