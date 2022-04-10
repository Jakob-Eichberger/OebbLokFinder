using Infrastructure;
using Service;

namespace OebbLokFinder;

public partial class MainPage : TabbedPage
{

    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Children.Add(new LokFinder(serviceProvider));
        Children.Add(new VehicleManagment(serviceProvider));
        //Children.Add(new Settings(serviceProvider));
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }
}

