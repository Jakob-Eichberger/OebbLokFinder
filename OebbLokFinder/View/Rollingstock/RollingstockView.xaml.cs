using OebbLokFinder.Model;
using OebbLokFinder.Service;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockView : ContentView
{
    public RollingstockView(int rollingstockId)
    {
        InitializeComponent();
        RollingstockId = rollingstockId;
    }

    public RollingstockService RollingstockService { get; set; } = default!;

    public Model.Rollingstock? Rollingstock { get; set; }

    public int RollingstockId { get; }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        try
        {
            Rollingstock = await RollingstockService.GetRollingstockByIdAsync(RollingstockId) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}