namespace OebbLokFinder.View;

public partial class StopView : ContentView
{
    public StopView(Model.Stop stop)
    {
        Stop = stop;
        BindingContext = this;
        InitializeComponent();
        OnPropertyChanged(nameof(Stop));

        IsVisible = (stop.Arrival ?? stop.Departure) >= DateTime.Now;
    }

    public Model.Stop Stop { get; }
}