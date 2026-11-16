using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFavouriteRepository
    {
        Task<List<Favourite>> GetByUserIdAsync(int userId);
        Task<Favourite?> GetByUserAndTeamAsync(int userId, int teamId);
        Task AddAsync(Favourite favourite);
        Task DeleteAsync(Favourite favourite);
    }
}