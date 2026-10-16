
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_ASP.API.Data;
using Projekt_ASP.Shared.DTOs;
using Projekt_ASP.Shared.Models;

namespace Projekt_ASP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        string userName = request.UserName.Trim();
        string email = request.Email.Trim();

        bool exists = await _context.Users.AnyAsync(u =>
            u.UserName == userName || u.Email == email);

        if (exists)
        {
            return BadRequest("Użytkownik lub e-mail już istnieje.");
        }

        var user = new User
        {
            UserName = userName,
            Email = email,
            PasswordHash = request.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Rejestracja zakończona sukcesem.");
    }
    
[HttpPost("login")]
public async Task<IActionResult> Login(LoginRequest request)
{
    var email = request.Email.Trim();

    var user = await _context.Users.FirstOrDefaultAsync(u =>
        u.Email == email);

    if (user == null || user.PasswordHash != request.Password)
    {
        return Unauthorized(new
        {
            message = "Nieprawidłowy e-mail lub hasło."
        });
    }

    return Ok(new
    {
        id = user.Id,
        userName = user.UserName,
        email = user.Email
    });
}

}
