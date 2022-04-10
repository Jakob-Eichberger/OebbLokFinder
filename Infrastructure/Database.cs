using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using Newtonsoft.Json;

namespace Infrastructure
{
    // All the code in this file is included in all platforms.
    public partial class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {
        }

        public Database()
        {
        }

        public event EventHandler CollectionChanged;

        public virtual DbSet<Stop> Stops => Set<Stop>();

        public virtual DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public virtual DbSet<Setting> Settings => Set<Setting>();

        public void InvokeCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{Path.DirectorySeparatorChar}LokFinder{Path.DirectorySeparatorChar}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source=\"{path}Loco.sqlite\"");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stop>().OwnsOne(e => e.Station);
            modelBuilder.Entity<Setting>().Property(b => b.Properties).HasConversion(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public void Init()
        {
            if (Database.EnsureCreated())
            {
                Vehicles.Add(new Model.Vehicle() { ClassNumber = 1116, SerialNumber = 36, Name = "CAT", AddedManually = true });
                if (!Settings.Any())
                {
                    Settings.Add(new Model.Setting());
                }
                SaveChanges();
            }
        }
    }
}