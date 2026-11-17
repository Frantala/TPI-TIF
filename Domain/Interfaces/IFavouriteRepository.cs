using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFavouriteRepository : IGenericRepository<Favourite>
    {
        Task<List<Favourite>> GetByUserIdAsync(int userId);
        Task<Favourite?> GetByUserAndTeamAsync(int userId, int teamId);
        Task DeleteAsync(Favourite favourite);
    }
}