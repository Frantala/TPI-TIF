namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class TeamResponse
{
    public TeamInfo Team { get; set; } = new();
    public VenueInfo Venue { get; set; } = new();
}