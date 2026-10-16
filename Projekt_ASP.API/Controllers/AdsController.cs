
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_ASP.API.Data;
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
    // Pobiera wszystkie ogłoszenia wraz ze zdjęciami.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
    {
        var ads = await _context.Ads
            .AsNoTracking()
            .Include(a => a.Images)
            .ToListAsync();

        return Ok(ads);
    }

    // GET: api/ads/5
    // Pobiera jedno ogłoszenie wraz ze zdjęciami.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ad>> GetAd(int id)
    {
        var ad = await _context.Ads
            .AsNoTracking()
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono ogłoszenia o ID {id}."
            });
        }

        return Ok(ad);
    }

    // POST: api/ads
    // Dodaje nowe ogłoszenie.
    [HttpPost]
    public async Task<ActionResult<Ad>> CreateAd([FromBody] Ad ad)
    {
        // Użytkownik jest powiązany przez UserId.
        // Nie dodajemy obiektu User jako nowego rekordu.
        ad.User = null;

        // Powiązujemy zdjęcia z nowym ogłoszeniem.
        if (ad.Images != null)
        {
            foreach (var image in ad.Images)
            {
                image.Ad = ad;
            }
        }

        _context.Ads.Add(ad);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "Nie udało się zapisać ogłoszenia. Sprawdź UserId i dane zdjęć."
            });
        }

        return CreatedAtAction(
            nameof(GetAd),
            new { id = ad.Id },
            ad);
    }

    // PUT: api/ads/5
    // Aktualizuje podstawowe pola ogłoszenia.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAd(
        int id,
        [FromBody] Ad updatedAd)
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
        {
            return NotFound(new
            {
                message = "Nie znaleziono ogłoszenia."
            });
        }

        // Aktualizujemy tylko pola edytowane w formularzu.
        existingAd.Title = updatedAd.Title;
        existingAd.Description = updatedAd.Description;
        existingAd.Category = updatedAd.Category;
        existingAd.Price = updatedAd.Price;
        existingAd.Location = updatedAd.Location;
        existingAd.PhoneNumber = updatedAd.PhoneNumber;

        // Nie zmieniamy UserId, PostedDate ani zdjęć.
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ads/5
    // Usuwa ogłoszenie.
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var ad = await _context.Ads
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
        {
            return NotFound(new
            {
                message = "Nie znaleziono ogłoszenia."
            });
        }

        _context.Ads.Remove(ad);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
