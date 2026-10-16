
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Models;
using Projekt_ASP.Services;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdsApiClient _adsApiClient;

        public HomeController(
            ILogger<HomeController> logger,
            AdsApiClient adsApiClient)
        {
            _logger = logger;
            _adsApiClient = adsApiClient;
        }

        public async Task<IActionResult> Index(
            string? search,
            string? category)
        {
            List<Ad> ads = new();

            ViewBag.Search = search ?? string.Empty;
            ViewBag.Category = category ?? string.Empty;

            try
            {
                ads = await _adsApiClient.GetAdsAsync();

                // Lista kategorii do wyświetlenia na stronie.
                ViewBag.Categories = ads
                    .Select(ad => ad.Category)
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();

                // Wyszukiwanie po tytule i opisie.
                if (!string.IsNullOrWhiteSpace(search))
                {
                    string phrase = search.Trim();

                    ads = ads.Where(ad =>
                        ad.Title.Contains(
                            phrase,
                            StringComparison.OrdinalIgnoreCase)
                        || ad.Description.Contains(
                            phrase,
                            StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Filtrowanie po wybranej kategorii.
                if (!string.IsNullOrWhiteSpace(category))
                {
                    ads = ads.Where(ad =>
                        string.Equals(
                            ad.Category,
                            category,
                            StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Najnowsze ogłoszenia na początku.
                ads = ads
                    .OrderByDescending(ad => ad.PostedDate)
                    .ToList();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Nie udało się pobrać ogłoszeń z API.");

                ViewBag.ApiError =
                    "Nie można pobrać ogłoszeń. Sprawdź, czy API jest uruchomione.";
                ViewBag.Categories = new List<string>();
            }

            return View(ads);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
