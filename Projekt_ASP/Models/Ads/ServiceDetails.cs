
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_ASP.Models.Ads
{
    public class ServiceDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? ServiceType { get; set; }
        public string? ServiceDescription { get; set; }
    }
}
