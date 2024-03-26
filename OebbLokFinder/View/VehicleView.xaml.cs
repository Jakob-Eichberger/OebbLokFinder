using OebbLokFinder.Infrastructure;
using OebbLokFinder.Model;

namespace OebbLokFinder.View;

public partial class VehicleView : ContentView
{
    public VehicleView(Vehicle vehicle, Action<VehicleView> valueUpdated)
    {
        Db = App.ServiceProvider.GetService<Database>();
        Vehicle = vehicle;
        OnDelete = valueUpdated;
        Margin = new Thickness(5);
        BindingContext = this;
        InitializeComponent();
    }

    public Database Db { get; set; }

    public Vehicle Vehicle { get; set; }

    public Action<VehicleView> OnDelete { get; }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        Vehicle = Db.Vehicles.FirstOrDefault(e => e.Id == Vehicle.Id);
        Db.Vehicles.Remove(Vehicle);
        Db.SaveChanges();
        OnDelete(this);
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Db.Update(Vehicle);
        Db.SaveChanges();
    }
}