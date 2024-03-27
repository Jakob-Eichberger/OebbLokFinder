using OebbLokFinder.Service;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockPage : ContentPage
{
    public RollingstockPage()
    {
        RollingstockService = App.ServiceProvider.GetService<RollingstockService>();
        RollingstockService.RollingstocksAddedOrDeleted += RollingstockService_RollingstocksAddedOrDeleted;
        InitializeComponent();
    }

    private async void RollingstockService_RollingstocksAddedOrDeleted(object? sender, EventArgs e)
    {
        await NewMethod();
    }

    private async Task NewMethod()
    {
        VSLRollingstocks.Clear();
        (await RollingstockService.GetAllVehicleIds()).ForEach(vehicle => VSLRollingstocks.Children.Add(new RollingstockView(vehicle)));
    }

    public RollingstockService RollingstockService { get; set; }

    private async void BtnAddRollingstock_Pressed(object sender, EventArgs e)
    {
        await RollingstockService.GetOrCreatRollingstockAsync(0, 0);
    }

    private void BtnAddAdvertRollingstock_Pressed(object sender, EventArgs e)
    {

    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await NewMethod();
    }
}