using Infrastructure;
using Service;
using System.ComponentModel;

namespace OebbLokFinder;

public partial class VehicleManagment : ContentPage, INotifyPropertyChanged
{
    public VehicleManagment(IServiceProvider serviceProvider)
    {
        Db = serviceProvider.GetRequiredService<Database>();
        WebService = serviceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        ServiceProvider = serviceProvider;
        InitializeComponent();
        SLUpdate.Add(new DataUpdater(serviceProvider));
        OnDelete = (v) =>
        {
            var asd = VSLVehicles.Remove(v);
            OnPropertyChanged(nameof(VehicleManagment));
        };

        UpdateData();
    }
    public bool Test { get; set; } = true;

    private void UpdateData()
    {
        VSLVehicles.Children.Clear();
        Db.Vehicles.Select(e => new VehicleView(ServiceProvider, e, OnDelete)).ToList().ForEach(e => VSLVehicles.Children.Add(e));
        OnPropertyChanged(nameof(Test));
    }

    public Action<VehicleView> OnDelete { get; set; }

    public Database Db { get; set; }

    public IServiceProvider ServiceProvider { get; }

    public OebbWebService WebService { get; set; }
}

