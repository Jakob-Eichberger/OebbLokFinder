﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

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

        public void InvokeCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source=\"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{Path.DirectorySeparatorChar}Loco.sqlite\"");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().OwnsOne(e => e.VehicleClassification);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}