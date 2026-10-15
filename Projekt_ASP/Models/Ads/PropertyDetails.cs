
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_ASP.Models.Ads
{
    public class PropertyDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? PropertyType { get; set; }
        public int? PropertyArea { get; set; }
        public int? PropertyRooms { get; set; }
    }
}
