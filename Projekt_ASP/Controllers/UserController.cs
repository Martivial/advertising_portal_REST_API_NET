
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Shared.DTOs;
using Projekt_ASP.Shared.Models.Ads;

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

            ModelState.AddModelError(
                "",
                "Nie udało się utworzyć konta. Sprawdź dane lub spróbuj innej nazwy użytkownika.");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError("", "Nie można połączyć się z API.");
        }

        return View(request);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var client = _httpClientFactory.CreateClient("AdvertisingApi");

        try
        {
            var response = await client.PostAsJsonAsync(
                "api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Nieprawidłowy e-mail lub hasło.");

                return View(request);
            }

            var user = await response.Content
                .ReadFromJsonAsync<LoginResponse>();

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Nie udało się odczytać odpowiedzi API.");

                return View(request);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName);

            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                "",
                "Nie można połączyć się z API. Sprawdź, czy API jest uruchomione.");
        }

        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction(nameof(Login));

        var client = _httpClientFactory.CreateClient("AdvertisingApi");

        try
        {
            var ads = await client.GetFromJsonAsync<List<Ad>>("api/ads")
                      ?? new List<Ad>();

            var myAds = ads
                .Where(ad => ad.UserId == userId.Value)
                .OrderByDescending(ad => ad.PostedDate)
                .ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(myAds);
        }
        catch (HttpRequestException)
        {
            return StatusCode(
                503,
                "Nie można pobrać danych konta. Sprawdź, czy API jest uruchomione.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}

public class LoginResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
