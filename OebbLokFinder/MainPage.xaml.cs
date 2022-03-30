namespace OebbLokFinder;

public partial class MainPage : TabbedPage
{
    public MainPage()
    {
        InitializeComponent();
        Children.Add(new ContentPage() {Title="TEST" });
        Children.Add(new ContentPage() {Title="TEST" });
        Children.Add(new Settings());
    }

}

