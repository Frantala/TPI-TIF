using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.ExternalServices.ApiFootball.Models;

namespace Infrastructure.ExternalServices.ApiFootball;

public class CompetitionExternalService : ICompetitionExternalService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CompetitionExternalService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Competition>> GetLeaguesAsync(
        string country,
        int season)
    {
        var client = _httpClientFactory.CreateClient(
            HttpClientNames.ApiFootball
        );

        string endpoint =
            $"leagues?country={Uri.EscapeDataString(country)}&season={season}";

        var apiResponse =
            await client.GetFromJsonAsync<ApiFootballResponse<LeagueResponse>>(
                endpoint
            );

        if (apiResponse == null)
        {
            throw new InvalidOperationException(
                "API-Football devolvió una respuesta vacía."
            );
        }

        if (apiResponse.Errors.ValueKind == System.Text.Json.JsonValueKind.Object
            && apiResponse.Errors.EnumerateObject().Any())
        {
            throw new InvalidOperationException(
                $"API-Football devolvió errores: {apiResponse.Errors}"
            );
        }

        return apiResponse.Response.Select(item => new Competition
        {
            ExternalId = item.League.Id,
            Name = item.League.Name,
            Country = item.Country.Name,
            TeamsCount = 0
        }).ToList();
    }
}