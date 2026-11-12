using Domain.Entities;

namespace Application.Interfaces;

public interface ICompetitionExternalService
{
    Task<List<Competition>> GetLeaguesAsync(
        string country,
        int season
    );
}