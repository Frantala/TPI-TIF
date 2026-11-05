namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class LeagueInfo
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Logo { get; set; } = string.Empty;
}