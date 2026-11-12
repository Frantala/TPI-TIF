namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class TeamInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
}