using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int id);
        Task<List<Team>> GetAllAsync();
        Task<List<Team>> GetByExternalIdsAsync(IEnumerable<int> externalIds);
        Task AddRangeAsync(IEnumerable<Team> teams);
        Task UpdateRangeAsync(IEnumerable<Team> teams);
    }
}