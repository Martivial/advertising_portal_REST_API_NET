using Microsoft.EntityFrameworkCore;
using Projekt_ASP.Shared.Models;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ad> Ads => Set<Ad>();
    public DbSet<User> Users => Set<User>();
    public DbSet<AdImage> AdImages => Set<AdImage>();

    public DbSet<VehicleDetails> VehicleDetails => Set<VehicleDetails>();
    public DbSet<PropertyDetails> PropertyDetails => Set<PropertyDetails>();
    public DbSet<JobDetails> JobDetails => Set<JobDetails>();
    public DbSet<ElectronicsDetails> ElectronicsDetails => Set<ElectronicsDetails>();
    public DbSet<ServiceDetails> ServiceDetails => Set<ServiceDetails>();
    public DbSet<HomeAndGardenDetails> HomeAndGardenDetails => Set<HomeAndGardenDetails>();
    public DbSet<FashionDetails> FashionDetails => Set<FashionDetails>();
    public DbSet<KidsItemDetails> KidsItemDetails => Set<KidsItemDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ad>()
            .HasOne(a => a.User)
            .WithMany(u => u.Ads)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ad>()
            .HasMany(a => a.Images)
            .WithOne(i => i.Ad)
            .HasForeignKey(i => i.AdId)
            .OnDelete(DeleteBehavior.Cascade);

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