using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class DataUpdater : ContentView
{
    private bool loadingData = false;
    private double progress = 0;
    private TimeSpan timeRemaing = TimeSpan.Zero;

    public DataUpdater(IServiceProvider serviceProvider)
    {
        Db = serviceProvider.GetRequiredService<Database>();
        WebService = serviceProvider.GetRequiredService<OebbWebService>();
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
        var start = DateTime.Now;
        TimeRemaing = Timeout.InfiniteTimeSpan;
        LoadingData = true;

        await WebService.UpdateVehilcesFromLokfinderOebbWebsiteListe();
        await WebService.UpdateStopsForAllVehicles((p) => Dispatcher.Dispatch(() =>
        {
            Progress = p;
            TimeRemaing = Progress != 0 ? new TimeSpan(0, 0, (int)((DateTime.Now - start).TotalSeconds / Progress)) : Timeout.InfiniteTimeSpan;
        }));

        LoadingData = false;
        OnPropertyChanged(nameof(LastUpdated));
    }
}