
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nazwa użytkownika musi być unikalna.
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Adres e-mail musi być unikalny.
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Jeden użytkownik może mieć wiele ogłoszeń.
        modelBuilder.Entity<Ad>()
            .HasOne(a => a.User)
            .WithMany(u => u.Ads)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Jedno ogłoszenie może mieć wiele zdjęć.
        modelBuilder.Entity<Ad>()
            .HasMany(a => a.Images)
            .WithOne(i => i.Ad)
            .HasForeignKey(i => i.AdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
