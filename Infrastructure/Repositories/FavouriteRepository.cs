using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Favourite>> GetByUserIdAsync(int userId)
        {
            return await Context.Favourites
                .Include(favourite => favourite.Team)
                .Where(favourite => favourite.UserId == userId)
                .OrderBy(favourite => favourite.Id)
                .ToListAsync();
        }

        public async Task<Favourite?> GetByUserAndTeamAsync(int userId, int teamId)
        {
            return await Context.Favourites.FirstOrDefaultAsync(
                favourite => favourite.UserId == userId && favourite.TeamId == teamId);
        }

        public async Task DeleteAsync(Favourite favourite)
        {
            Context.Favourites.Remove(favourite);
            await Context.SaveChangesAsync();
        }
    }
}