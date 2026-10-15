using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.Shared.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
}