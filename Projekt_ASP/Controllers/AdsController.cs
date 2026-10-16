using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projekt_ASP.Services;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.Controllers
{
    public class AdsController : Controller
    {
        private readonly AdsApiClient _api;

        public AdsController(AdsApiClient api)
        {
            _api = api;
        }

        // GET: Ads/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(new Ad());
        }

        // POST: Ads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ad model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            RemoveUnusedValidation();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ad = new Ad
            {
                Title = model.Title.Trim(),
                Description = model.Description?.Trim() ?? string.Empty,
                Category = model.Category,
                Price = model.Price,
                Location = model.Location.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber)
                    ? null
                    : model.PhoneNumber.Trim(),
                UserId = userId.Value,
                PostedDate = DateTime.UtcNow
            };

            try
            {
                var createdAd = await _api.CreateAdAsync(ad);

                if (createdAd == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "API nie zwróciło danych utworzonego ogłoszenia.");

                    return View(model);
                }

                TempData["Success"] = "Ogłoszenie zostało opublikowane!";

                return RedirectToAction(
                    nameof(Details),
                    new { id = createdAd.Id });
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Nie można połączyć się z API. Sprawdź, czy API działa.");

                return View(model);
            }
        }

        // GET: Ads/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ad = await _api.GetAdAsync(id);

                if (ad == null)
                {
                    return NotFound();
                }

                return View(ad);
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Nie można pobrać ogłoszenia. Sprawdź, czy API działa.");
            }
        }

        // GET: Ads/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var ad = await _api.GetAdAsync(id);

                if (ad == null)
                {
                    return NotFound();
                }

                if (ad.UserId != userId.Value)
                {
                    return Forbid();
                }

                return View(ad);
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Nie można pobrać ogłoszenia. Sprawdź, czy API działa.");
            }
        }

        // POST: Ads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ad model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            RemoveUnusedValidation();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingAd = await _api.GetAdAsync(id);

                if (existingAd == null)
                {
                    return NotFound();
                }

                if (existingAd.UserId != userId.Value)
                {
                    return Forbid();
                }

                // Zmieniamy tylko pola formularza.
                existingAd.Title = model.Title.Trim();
                existingAd.Description =
                    model.Description?.Trim() ?? string.Empty;
                existingAd.Category = model.Category;
                existingAd.Price = model.Price;
                existingAd.Location = model.Location.Trim();
                existingAd.PhoneNumber =
                    string.IsNullOrWhiteSpace(model.PhoneNumber)
                        ? null
                        : model.PhoneNumber.Trim();

                bool updated = await _api.UpdateAdAsync(id, existingAd);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["Success"] = "Ogłoszenie zostało zaktualizowane!";

                return RedirectToAction("Profile", "User");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Nie udało się zapisać zmian. Sprawdź, czy API działa.");

                return View(model);
            }
        }

        // POST: Ads/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var ad = await _api.GetAdAsync(id);

                if (ad == null)
                {
                    TempData["Error"] = "Nie znaleziono ogłoszenia.";
                    return RedirectToAction("Profile", "User");
                }

                if (ad.UserId != userId.Value)
                {
                    return Forbid();
                }

                bool deleted = await _api.DeleteAdAsync(id);

                if (!deleted)
                {
                    TempData["Error"] = "Nie udało się usunąć ogłoszenia.";
                    return RedirectToAction("Profile", "User");
                }

                TempData["Success"] = "Ogłoszenie zostało usunięte!";

                return RedirectToAction("Profile", "User");
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "Nie można połączyć się z API. Spróbuj ponownie.";

                return RedirectToAction("Profile", "User");
            }
        }

        // Pomija walidację pól, których użytkownik nie edytuje.
        private void RemoveUnusedValidation()
        {
            ModelState.Remove(nameof(Ad.Id));
            ModelState.Remove(nameof(Ad.UserId));
            ModelState.Remove(nameof(Ad.PostedDate));
            ModelState.Remove(nameof(Ad.User));
            ModelState.Remove(nameof(Ad.Images));
            ModelState.Remove(nameof(Ad.VehicleDetails));
            ModelState.Remove(nameof(Ad.PropertyDetails));
            ModelState.Remove(nameof(Ad.JobDetails));
            ModelState.Remove(nameof(Ad.ElectronicsDetails));
            ModelState.Remove(nameof(Ad.ServiceDetails));
            ModelState.Remove(nameof(Ad.HomeAndGardenDetails));
            ModelState.Remove(nameof(Ad.FashionDetails));
            ModelState.Remove(nameof(Ad.KidsItemDetails));
        }
    }
}
