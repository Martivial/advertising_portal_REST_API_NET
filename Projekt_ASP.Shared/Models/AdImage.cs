
using System.ComponentModel.DataAnnotations;
using Projekt_ASP.Shared.Models.Ads;
namespace Projekt_ASP.Shared.Models
{
    public class AdImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        [Required]
        public int AdId { get; set; }

        public Ad Ad { get; set; } = null!;
    }
}
