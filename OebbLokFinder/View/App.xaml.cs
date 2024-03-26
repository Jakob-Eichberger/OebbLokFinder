using Microsoft.Extensions.DependencyInjection;
using OebbLokFinder.Infrastructure;
using OebbLokFinder.Model;

namespace OebbLokFinder
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = default!;

        public App(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            InitializeComponent();
            using (var db = new Database())
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                if (!db.Settings.Any())
                {
                    db.Settings.Add(new Setting());
                    db.SaveChanges();
                }
            }
            MainPage = new MainPage(ServiceProvider);
        }
    }
}
