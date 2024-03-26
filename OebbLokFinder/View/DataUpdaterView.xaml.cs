using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;

namespace OebbLokFinder.View;

public partial class DataUpdaterView : ContentView
{

    private bool loadingData = false;
    private double progress = 0;
    private TimeSpan timeRemaing = TimeSpan.Zero;

    public DataUpdaterView()
    {
        Db = App.ServiceProvider.GetRequiredService<Database>();
        WebService = App.ServiceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        InitializeComponent();
    }

    public Database Db { get; set; }

    public DateTime? LastUpdated => Db.Stops.Any() ? Db.Stops.Min(e => e.Fetched) : DateTime.Now;

    public bool LoadingData
    {
        get => loadingData; set
        {
            loadingData = value;
            OnPropertyChanged(nameof(LoadingData));
        }
    }

    public double Progress
    {
        get => progress; set
        {
            progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }

    public TimeSpan TimeRemaing
    {
        get => timeRemaing; set
        {
            timeRemaing = value;
            OnPropertyChanged(nameof(TimeRemaing));
        }
    }

    public OebbWebService WebService { get; set; }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var start = DateTime.Now;
            TimeRemaing = Timeout.InfiniteTimeSpan;
            LoadingData = true;

            await WebService.UpdateVehiclesFromLokfinderOebbWebsiteListe();
            await WebService.UpdateStopsForAllVehicles((p) => Dispatcher.Dispatch(() =>
            {
                Progress = p;
                TimeRemaing = Progress != 0 ? new TimeSpan(0, 0, (int)((DateTime.Now - start).TotalSeconds / Progress)) : Timeout.InfiniteTimeSpan;
            }));

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            LoadingData = false;
            OnPropertyChanged(nameof(LastUpdated));
        }
    }
}