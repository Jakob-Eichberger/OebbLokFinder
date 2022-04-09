using Infrastructure;
using Model;
using System.ComponentModel;

namespace OebbLokFinder;

public partial class VehicleView : ContentView
{
    public VehicleView(IServiceProvider serviceProvider, Vehicle vehicle, Action<VehicleView> valueUpdated)
    {
        Vehicle = vehicle;
        OnDelete = valueUpdated;
        Margin = new Thickness(5);
        InitializeComponent();
        Db = serviceProvider.GetService<Database>();
        EName.Text = vehicle.Name;
        ESerialNumber.Text = $"{vehicle.SerialNumber}";
        EClassNumber.Text = $"{vehicle.ClassNumber}";
    }

    public Database Db { get; set; }

    public Vehicle Vehicle { get; set; }

    public Action<VehicleView> OnDelete { get; }

    private void EName_TextChanged(object sender, TextChangedEventArgs e)
    {
        Vehicle = Db.Vehicles.FirstOrDefault(e => e.Id == Vehicle.Id);
        Vehicle.Name = EName.Text;
        Db.Vehicles.Update(Vehicle);
        Db.SaveChanges();
    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        Vehicle = Db.Vehicles.FirstOrDefault(e => e.Id == Vehicle.Id);
        Db.Vehicles.Remove(Vehicle);
        Db.SaveChanges();
        OnDelete?.Invoke(this);
    }

    private void ESerialNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        Vehicle = Db.Vehicles.FirstOrDefault(e => e.Id == Vehicle.Id);
        Vehicle.SerialNumber = int.Parse(ESerialNumber.Text);
        Db.Vehicles.Update(Vehicle);
        Db.SaveChanges();
    }

    private void EClassNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        Vehicle = Db.Vehicles.FirstOrDefault(e => e.Id == Vehicle.Id);
        Vehicle.ClassNumber = int.Parse(EClassNumber.Text);
        Db.Vehicles.Update(Vehicle);
        Db.SaveChanges();
    }
}