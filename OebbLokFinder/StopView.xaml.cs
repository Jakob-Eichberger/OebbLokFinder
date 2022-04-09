namespace OebbLokFinder;

public partial class StopView : ContentView
{
    public StopView(Model.Stop stop)
    {
        Stop = stop;
        BindingContext = this;
        InitializeComponent();
        OnPropertyChanged(nameof(Stop));
    }
    public Model.Stop Stop { get; }
}