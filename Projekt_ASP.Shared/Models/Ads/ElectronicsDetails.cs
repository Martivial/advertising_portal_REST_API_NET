
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.Shared.Models.Ads
{
    public class ElectronicsDetails
    {
        [Key]
        public int Id { get; set; }

        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        public string? ElectronicsType { get; set; }
        public string? ElectronicsBrand { get; set; }
        public string? ElectronicsModel { get; set; }
    }
}
