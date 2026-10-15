
using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Services;

namespace Projekt_ASP.Controllers;

public class AdsController : Controller
{
    private readonly AdsApiClient _adsApiClient;

    public AdsController(AdsApiClient adsApiClient)
    {
        _adsApiClient = adsApiClient;
    }

    // GET: /Ads
    public async Task<IActionResult> Index()
    {
        try
        {
            var ads = await _adsApiClient.GetAdsAsync();
            return View(ads);
        }
        catch (HttpRequestException)
        {
            ViewBag.ErrorMessage =
                "Nie można połączyć się z API. Upewnij się, że API jest uruchomione.";
            return View(new List<Projekt_ASP.Shared.Models.Ads.Ad>());
        }
    }

    // GET: /Ads/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var ad = await _adsApiClient.GetAdAsync(id);

            if (ad == null)
                return NotFound();

            return View(ad);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "API jest obecnie niedostępne.");
        }
    }
}
