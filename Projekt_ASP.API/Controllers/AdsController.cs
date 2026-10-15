
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_ASP.API.Data;
using Projekt_ASP.Shared.Models;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ads
    // Pobiera wszystkie ogłoszenia
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
    {
        var ads = await _context.Ads
            .AsNoTracking()
            .Include(a => a.Images)
            .Include(a => a.VehicleDetails)
            .Include(a => a.PropertyDetails)
            .Include(a => a.JobDetails)
            .Include(a => a.ElectronicsDetails)
            .Include(a => a.ServiceDetails)
            .Include(a => a.HomeAndGardenDetails)
            .Include(a => a.FashionDetails)
            .Include(a => a.KidsItemDetails)
            .ToListAsync();

        return Ok(ads);
    }

    // GET: api/ads/5
    // Pobiera jedno ogłoszenie
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ad>> GetAd(int id)
    {
        var ad = await _context.Ads
            .AsNoTracking()
            .Include(a => a.Images)
            .Include(a => a.VehicleDetails)
            .Include(a => a.PropertyDetails)
            .Include(a => a.JobDetails)
            .Include(a => a.ElectronicsDetails)
            .Include(a => a.ServiceDetails)
            .Include(a => a.HomeAndGardenDetails)
            .Include(a => a.FashionDetails)
            .Include(a => a.KidsItemDetails)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
            return NotFound(new { message = $"Nie znaleziono ogłoszenia o ID {id}." });

        return Ok(ad);
    }

    // POST: api/ads
    // Dodaje nowe ogłoszenie
    [HttpPost]
    public async Task<ActionResult<Ad>> CreateAd([FromBody] Ad ad)
    {
        // Nie dodawaj użytkownika ponownie jako nowego rekordu.
        ad.User = null!;

        // Ustaw prawidłowe powiązania dla danych zależnych.
        if (ad.Images != null)
        {
            foreach (var image in ad.Images)
                image.Ad = ad;
        }

        if (ad.VehicleDetails != null)
            ad.VehicleDetails.Ad = ad;

        if (ad.PropertyDetails != null)
            ad.PropertyDetails.Ad = ad;

        if (ad.JobDetails != null)
            ad.JobDetails.Ad = ad;

        if (ad.ElectronicsDetails != null)
            ad.ElectronicsDetails.Ad = ad;

        if (ad.ServiceDetails != null)
            ad.ServiceDetails.Ad = ad;

        if (ad.HomeAndGardenDetails != null)
            ad.HomeAndGardenDetails.Ad = ad;

        if (ad.FashionDetails != null)
            ad.FashionDetails.Ad = ad;

        if (ad.KidsItemDetails != null)
            ad.KidsItemDetails.Ad = ad;

        _context.Ads.Add(ad);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "Nie udało się zapisać ogłoszenia. Sprawdź UserId i dane powiązane."
            });
        }

        return CreatedAtAction(
            nameof(GetAd),
            new { id = ad.Id },
            ad);
    }

    // PUT: api/ads/5
    // Aktualizuje podstawowe pola ogłoszenia
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAd(int id, [FromBody] Ad updatedAd)
    {
        if (id != updatedAd.Id)
        {
            return BadRequest(new
            {
                message = "ID w adresie musi być zgodne z ID ogłoszenia."
            });
        }

        var existingAd = await _context.Ads.FindAsync(id);

        if (existingAd == null)
            return NotFound(new { message = "Nie znaleziono ogłoszenia." });

        // Aktualizuje pola proste, bez nadpisywania nawigacji.
        _context.Entry(existingAd).CurrentValues.SetValues(updatedAd);

        // Nie zmieniamy powiązanych zdjęć ani szczegółów kategorii
        // w tym endpointcie.
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ads/5
    // Usuwa ogłoszenie
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var ad = await _context.Ads.FindAsync(id);

        if (ad == null)
            return NotFound(new { message = "Nie znaleziono ogłoszenia." });

        _context.Ads.Remove(ad);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
