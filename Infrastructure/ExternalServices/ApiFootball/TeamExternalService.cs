using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.ExternalServices.ApiFootball.Models;

namespace Infrastructure.ExternalServices.ApiFootball;

public class TeamExternalService : ITeamExternalService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TeamExternalService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Team>> GetTeamsByLeagueAsync(
        int leagueId,
        int season)
    {
        var client = _httpClientFactory.CreateClient(
            HttpClientNames.ApiFootball
        );

        string endpoint = $"teams?league={leagueId}&season={season}";

        var apiResponse =
            await client.GetFromJsonAsync<ApiFootballResponse<TeamResponse>>(
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

        return apiResponse.Response.Select(item => new Team
        {
            ExternalId = item.Team.Id,
            TeamName = item.Team.Name,
            Country = item.Team.Country,
            SoccerTeam = item.Team.Name,
            DtName = string.Empty
        }).ToList();
    }
}