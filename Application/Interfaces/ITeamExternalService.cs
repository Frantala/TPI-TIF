using Domain.Entities;

namespace Application.Interfaces;

public interface ITeamExternalService
{
    Task<List<Team>> GetTeamsByLeagueAsync(int leagueId, int season);
}