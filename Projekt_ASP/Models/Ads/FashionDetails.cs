
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_ASP.Models.Ads
{
    public class FashionDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? FashionType { get; set; }
        public string? FashionSize { get; set; }
        public string? FashionColor { get; set; }
    }
}
