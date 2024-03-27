using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using OebbLokFinder.Model;

namespace OebbLokFinder.Infrastructure
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

        public event EventHandler? CollectionChanged;

        public virtual DbSet<Stop> Stops => Set<Stop>();

        public virtual DbSet<Rollingstock> Rollingstocks => Set<Rollingstock>();

        public void InvokeCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LokFinder");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source=\"{path}Loco.sqlite\"");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stop>().OwnsOne(e => e.Station);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override EntityEntry<TEntity> Add<TEntity>(TEntity obj) where TEntity : class
        {
            var result = Set<TEntity>().Add(obj);
            SaveChanges();
            CollectionChanged?.Invoke(this, new EventArgs());
            return result;
        }

        public async Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity obj) where TEntity : class
        {
            var result = await Set<TEntity>().AddAsync(obj);
            await SaveChangesAsync();
            CollectionChanged?.Invoke(this, new EventArgs());
            return result;
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity obj) where TEntity : class
        {
            var value = Set<TEntity>().Remove(obj);
            SaveChanges();
            CollectionChanged?.Invoke(this, new EventArgs());
            return value;
        }

        public async Task<EntityEntry<TEntity>> RemoveAsync<TEntity>(TEntity obj) where TEntity : class
        {
            var value = Set<TEntity>().Remove(obj);
            await SaveChangesAsync();
            CollectionChanged?.Invoke(this, new EventArgs());
            return value;
        }

        public void RemoveRange<TEntity>(List<TEntity> obj) where TEntity : class
        {
            Set<TEntity>().RemoveRange(obj);
            SaveChanges();
            CollectionChanged?.Invoke(this, new EventArgs());
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity obj) where TEntity : class
        {
            var result = Set<TEntity>().Update(obj);
            SaveChanges();
            CollectionChanged?.Invoke(this, new EventArgs());
            return result;
        }

        public async Task<EntityEntry<TEntity>> UpdateAsync<TEntity>(TEntity obj) where TEntity : class
        {
            var result = Set<TEntity>().Update(obj);
            await SaveChangesAsync();
            CollectionChanged?.Invoke(this, new EventArgs());
            return result;
        }

        public void UpdateRange<TEntity>(List<TEntity> obj) where TEntity : class
        {
            Set<TEntity>().UpdateRange(obj);
            SaveChanges();
            CollectionChanged?.Invoke(this, new EventArgs());
        }

        public async Task UpdateRangeAsync<TEntity>(List<TEntity> obj) where TEntity : class
        {
            Set<TEntity>().UpdateRange(obj);
            await SaveChangesAsync();
            CollectionChanged?.Invoke(this, new EventArgs());
        }
    }
}