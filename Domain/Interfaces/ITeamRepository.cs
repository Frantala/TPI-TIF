using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<List<Team>> GetByExternalIdsAsync(IEnumerable<int> externalIds);
        Task AddRangeAsync(IEnumerable<Team> teams);
        Task UpdateRangeAsync(IEnumerable<Team> teams);
    }
}