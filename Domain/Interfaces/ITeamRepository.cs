using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int id);
    }
}