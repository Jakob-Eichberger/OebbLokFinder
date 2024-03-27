using OebbLokFinder.Model;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockView : ContentView
{
    public RollingstockView(int vehicleId)
    {
        InitializeComponent();
    }

    public Vehicle Vehicle { get; set; }
}