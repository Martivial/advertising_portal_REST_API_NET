
using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Shared.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Podaj adres e-mail.")]
    [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj hasło.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
