using OebbLokFinder.View;

namespace OebbLokFinder
{
    public partial class MainPage : TabbedPage
    {
        public MainPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
