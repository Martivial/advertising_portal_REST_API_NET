using System.Net.Http.Json;
using Projekt_ASP.Shared.Models.Ads;

namespace Projekt_ASP.Services;

public class AdsApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdsApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Ad>> GetAdsAsync()
    {
        var client = _httpClientFactory.CreateClient("AdvertisingApi");

        var ads = await client.GetFromJsonAsync<List<Ad>>("api/Ads");

        return ads ?? new List<Ad>();
    }

    public async Task<Ad?> GetAdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("AdvertisingApi");

        var response = await client.GetAsync($"api/Ads/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Ad>();
    }
}