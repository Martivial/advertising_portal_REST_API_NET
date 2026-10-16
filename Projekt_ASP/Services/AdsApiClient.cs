
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

    private HttpClient CreateClient()
        => _httpClientFactory.CreateClient("AdvertisingApi");

    public async Task<List<Ad>> GetAdsAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await CreateClient()
            .GetAsync("api/ads", cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<List<Ad>>(
                cancellationToken: cancellationToken)
            ?? new List<Ad>();
    }

    public async Task<Ad?> GetAdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await CreateClient()
            .GetAsync($"api/ads/{id}", cancellationToken);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Ad>(
            cancellationToken: cancellationToken);
    }

   public async Task<Ad?> CreateAdAsync(
    Ad ad,
    CancellationToken cancellationToken = default)
{
    var response = await CreateClient()
        .PostAsJsonAsync("api/ads", ad, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content
            .ReadAsStringAsync(cancellationToken);

        throw new HttpRequestException(
            $"API zwróciło błąd {(int)response.StatusCode} " +
            $"({response.StatusCode}). Szczegóły: {error}");
    }

    return await response.Content.ReadFromJsonAsync<Ad>(
        cancellationToken: cancellationToken);
}

    public async Task<bool> UpdateAdAsync(
        int id,
        Ad ad,
        CancellationToken cancellationToken = default)
    {
        var response = await CreateClient()
            .PutAsJsonAsync($"api/ads/{id}", ad, cancellationToken);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await CreateClient()
            .DeleteAsync($"api/ads/{id}", cancellationToken);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}
