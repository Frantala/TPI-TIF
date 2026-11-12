namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class LeagueResponse
{
    public LeagueInfo League { get; set; } = new();

    public CountryInfo Country { get; set; } = new();

    public List<SeasonInfo> Seasons { get; set; } = new();
}