namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class SeasonInfo
{
    public int Year { get; set; }

    public DateOnly Start { get; set; }

    public DateOnly End { get; set; }

    public bool Current { get; set; }
}