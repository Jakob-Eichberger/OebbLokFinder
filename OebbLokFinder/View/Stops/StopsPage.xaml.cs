using Microsoft.EntityFrameworkCore;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;
using System.ComponentModel;

namespace OebbLokFinder.View.Stops;

public partial class StopsPage : ContentPage, INotifyPropertyChanged
{
    private bool updatingStops = true;

    public StopsPage()
    {
        Db = App.ServiceProvider.GetRequiredService<Database>();
        StationService = App.ServiceProvider.GetRequiredService<StationService>();
        BindingContext = this;
        InitializeComponent();
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public Database Db { get; set; }

    public StationService StationService { get; set; }

    public string Station { get; set; } = string.Empty;

    public List<string> Stations => Db.Stops.Where(e => (e.Arrival ?? e.Departure) >= DateTime.Now).Select(e => e.Station.StationName).Distinct().OrderBy(e => e).ToList();


    public bool UpdatingStops
    {
        get => updatingStops; set
        {
            updatingStops = value;
            PropertyChanged?.Invoke(this, new(null));
        }
    }

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
        try
        {
            UpdatingStops = false;
            await StationService.UpdateStopsForRollingStockAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert($"Error while updating stops", ex.Message, "Ok");
        }
        finally
        {
            UpdatingStops = true;
        }
    }
}