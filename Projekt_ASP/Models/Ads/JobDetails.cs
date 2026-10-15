
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_ASP.Models.Ads
{
    public class JobDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? JobTitle { get; set; }
        public string? JobCompany { get; set; }
        public string? JobEmploymentType { get; set; }
    }
}
