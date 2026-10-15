
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Shared.DTOs;

namespace Projekt_ASP.Controllers;

public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var client = _httpClientFactory.CreateClient("AdvertisingApi");

        try
        {
            var response = await client.PostAsJsonAsync(
                "api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Konto zostało utworzone!";
                return RedirectToAction(nameof(Register));
            }

            ModelState.AddModelError("", "Nie udało się utworzyć konta. Sprawdź dane lub spróbuj innej nazwy użytkownika.");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError("", "Nie można połączyć się z API. Uruchom projekt Projekt_ASP.API.");
        }

        return View(request);
    }
}