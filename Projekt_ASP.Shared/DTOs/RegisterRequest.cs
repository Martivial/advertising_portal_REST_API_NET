
using System.ComponentModel.DataAnnotations;

namespace Projekt_ASP.Shared.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "Podaj nazwę użytkownika.")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Nazwa musi mieć od 3 do 100 znaków.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj adres e-mail.")]
    [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj hasło.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Hasło musi zawierać dużą literę, cyfrę i znak specjalny.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potwierdź hasło.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password),
        ErrorMessage = "Hasła nie są identyczne.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
