
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_ASP.Shared.Models.Ads
{
    public class HomeAndGardenDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? HomeAndGardenType { get; set; }
        public string? HomeAndGardenCondition { get; set; }
    }
}
