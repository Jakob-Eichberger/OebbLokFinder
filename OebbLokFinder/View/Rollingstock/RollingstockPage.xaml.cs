using OebbLokFinder.Service;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockPage : ContentPage
{
    public RollingstockPage()
    {
        RollingstockService = App.ServiceProvider.GetService<RollingstockService>();
        RollingstockService.RollingstocksAddedOrDeleted += RollingstockService_RollingstocksAddedOrDeleted;

        OebbWebService = App.ServiceProvider.GetService<OebbWebService>();
        OebbWebService.FetchedData += (a, b) => RenderVehicleViews();
        InitializeComponent();
    }

    private void RollingstockService_RollingstocksAddedOrDeleted(object? sender, EventArgs e) => RenderVehicleViews();

    private void RenderVehicleViews() => Dispatcher.Dispatch(async () =>
    {
        var x = new VerticalStackLayout();
        (await RollingstockService.GetAllVehicleIds()).Select(e => new RollingstockView(e)).ToList().ForEach(e => x.Children.Add(e));
        SVRollingstocks.Content = x;
    });

    public RollingstockService RollingstockService { get; set; }

    public OebbWebService OebbWebService { get; set; }

    private async void BtnAddRollingstock_Pressed(object sender, EventArgs e)
    {
        await RollingstockService.GetOrCreatRollingstockAsync(0, 0);
    }

    private async void BtnAddAdvertRollingstock_Pressed(object sender, EventArgs e)
    {
        await OebbWebService.UpdateRollingstockFromLokfinderOebbWebsiteListeAsync();
    }

    private void SVRollingstocks_Loaded(object sender, EventArgs e) => RenderVehicleViews();
}