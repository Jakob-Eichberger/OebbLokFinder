using OebbLokFinder.Model;
using OebbLokFinder.Service;
using System.ComponentModel;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockView : ContentView, INotifyPropertyChanged
{
    public RollingstockView(int rollingstockId)
    {
        RollingstockId = rollingstockId;
        RollingstockService = App.ServiceProvider.GetService<RollingstockService>();
        InitializeComponent();
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public RollingstockService RollingstockService { get; set; } = default!;

    public Model.Rollingstock? Rollingstock { get; set; }

    public int RollingstockId { get; }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        try
        {
            Rollingstock = await RollingstockService.GetRollingstockByIdAsync(RollingstockId) ?? throw new NullReferenceException();
            BindingContext = Rollingstock;
            PropertyChanged?.Invoke(this, new(nameof(Rollingstock)));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void EntrClassNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        await RollingstockService.UpdateRollingStockAsync(Rollingstock);
    }

    private async void EntrSerialNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        await RollingstockService.UpdateRollingStockAsync(Rollingstock);

    }

    private async void EntrName_TextChanged(object sender, TextChangedEventArgs e)
    {
        await RollingstockService.UpdateRollingStockAsync(Rollingstock);

    }

    private async void BtnDeletEntry_Pressed(object sender, EventArgs e)
    {
        await RollingstockService.RemoveRollingStockAsync(Rollingstock);
    }
}