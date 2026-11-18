using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICompetitionRepository : IGenericRepository<Competition>
    {
        Task<Competition?> GetByExternalIdAsync(int externalId);
        Task<List<Competition>> GetByExternalIdsAsync(IEnumerable<int> externalIds);
        Task AddRangeAsync(IEnumerable<Competition> competitions);
        Task UpdateRangeAsync(IEnumerable<Competition> competitions);
        Task<List<int>> GetLinkedTeamIdsAsync(int competitionId, int season);
        Task AddCompetitionTeamsAsync(IEnumerable<CompetitionTeam> links);
    }
}