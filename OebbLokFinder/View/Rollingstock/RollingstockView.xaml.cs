using OebbLokFinder.Model;

namespace OebbLokFinder.View.Rollingstock;

public partial class RollingstockView : ContentView
{
    public RollingstockView(int rollingstockId)
    {
        InitializeComponent();
    }

    public Model.Rollingstock Rollingstock { get; set; }
}