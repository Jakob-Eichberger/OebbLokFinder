using OebbLokFinder.Infrastructure;
using OebbLokFinder.Service;
using System.ComponentModel;

namespace OebbLokFinder.View;

public partial class VehicleManagment : ContentPage, INotifyPropertyChanged
{
    public VehicleManagment()
    {
        Db = App.ServiceProvider.GetRequiredService<Database>();
        OebbWebService = App.ServiceProvider.GetRequiredService<OebbWebService>();
        BindingContext = this;
        InitializeComponent();

        OebbWebService.FetchedData += (a, b) =>
        {
            Dispatcher.Dispatch(() =>
            {
                AddVehicleViews();
            });
        };

        OnDelete = (v) =>
        {
            var asd = VSLVehicles.Remove(v);
            OnPropertyChanged(nameof(VSLVehicles.Parent));
        };

        AddVehicleViews();
    }

    private void AddVehicleViews()
    {
        Button button = new() { Text = "Add locomotive", Margin = new Thickness(5) };
        button.Clicked += (a, b) =>
        {
            Db.Vehicles.Add(new Model.Vehicle() { AddedManually = true });
            Db.SaveChanges();
            AddVehicleViews();
        };

        VSLVehicles.Children.Clear();
        Db.Vehicles.ToList().OrderByDescending(e => e.Value).Select(e => new VehicleView(e, OnDelete)).ToList().ForEach(e => VSLVehicles.Children.Add(e));
        VSLVehicles.Children.Add(button);
        OnPropertyChanged(nameof(VSLVehicles.Parent));
    }

    public Action<VehicleView> OnDelete { get; set; }

    public Database Db { get; set; }

    public OebbWebService OebbWebService { get; set; }
}