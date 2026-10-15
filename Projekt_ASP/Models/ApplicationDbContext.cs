
using Microsoft.EntityFrameworkCore;
using Projekt_ASP.Models.Ads;

namespace Projekt_ASP.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Główne ogłoszenia
        public DbSet<Ad> Ads { get; set; }

        // Szczegóły kategorii
        public DbSet<VehicleDetails> VehicleDetails { get; set; }
        public DbSet<PropertyDetails> PropertyDetails { get; set; }
        public DbSet<JobDetails> JobDetails { get; set; }
        public DbSet<ElectronicsDetails> ElectronicsDetails { get; set; }
        public DbSet<ServiceDetails> ServiceDetails { get; set; }
        public DbSet<HomeAndGardenDetails> HomeAndGardenDetails { get; set; }
        public DbSet<FashionDetails> FashionDetails { get; set; }
        public DbSet<KidsItemDetails> KidsItemDetails { get; set; }

        // Użytkownicy i zdjęcia
        public DbSet<User> Users { get; set; }
        public DbSet<AdImage> AdImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Każde ogłoszenie może mieć maksymalnie jeden zestaw
            // szczegółów dla danej kategorii.
            modelBuilder.Entity<Ad>()
                .HasOne(a => a.VehicleDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<VehicleDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.PropertyDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<PropertyDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.JobDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<JobDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.ElectronicsDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<ElectronicsDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.ServiceDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<ServiceDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.HomeAndGardenDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<HomeAndGardenDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.FashionDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<FashionDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.KidsItemDetails)
                .WithOne(d => d.Ad)
                .HasForeignKey<KidsItemDetails>(d => d.AdId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
