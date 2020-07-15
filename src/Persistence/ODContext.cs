using Microsoft.EntityFrameworkCore;
using OrigemDestino.Core;

namespace OrigemDestino.Persistence
{
    public class ODContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Data/sqlite.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Frequenter>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Frequenter>()
                .Property(f => f.Id).ValueGeneratedNever();

            modelBuilder.Entity<LocationFrequenter>()
                .HasKey(lf => new { lf.LocationId, lf.FrequenterId });

            modelBuilder.Entity<LocationFrequenter>()
                .HasOne(lf => lf.Location)
                .WithMany(l => l.LocationFrequenters)
                .HasForeignKey(lf => lf.LocationId);

            modelBuilder.Entity<LocationFrequenter>()
                .HasOne(lf => lf.Frequenter)
                .WithMany(f => f.LocationFrequenters)
                .HasForeignKey(lf => lf.FrequenterId);
        }
    }
}