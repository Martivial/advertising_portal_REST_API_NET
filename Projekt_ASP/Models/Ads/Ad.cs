
using System.ComponentModel.DataAnnotations;
using Projekt_ASP.Models;

namespace Projekt_ASP.Models.Ads
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        // Użytkownik, który dodał ogłoszenie
        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        // Szczegóły konkretnej kategorii
        public VehicleDetails? VehicleDetails { get; set; }

        public PropertyDetails? PropertyDetails { get; set; }

        public JobDetails? JobDetails { get; set; }

        public ElectronicsDetails? ElectronicsDetails { get; set; }

        public ServiceDetails? ServiceDetails { get; set; }

        public HomeAndGardenDetails? HomeAndGardenDetails { get; set; }

        public FashionDetails? FashionDetails { get; set; }

        public KidsItemDetails? KidsItemDetails { get; set; }

        // Zdjęcia ogłoszenia
        public ICollection<AdImage> Images { get; set; }
            = new List<AdImage>();
    }
}
