
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Projekt_ASP.Shared.Models.Ads
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

        // ID użytkownika, który dodał ogłoszenie
        [Required]
        public int UserId { get; set; }

        // Nawigacja do użytkownika, pomijana w JSON
        [JsonIgnore]
        public User? User { get; set; }

        // Zdjęcia ogłoszenia
        public ICollection<AdImage> Images { get; set; }
            = new List<AdImage>();
    }
}
