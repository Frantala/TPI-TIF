using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavouriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favourite>> GetByUserIdAsync(int userId)
        {
            return await _context.Favourites
                .Include(favourite => favourite.Team)
                .Where(favourite => favourite.UserId == userId)
                .OrderBy(favourite => favourite.Id)
                .ToListAsync();
        }

        public async Task<Favourite?> GetByUserAndTeamAsync(int userId, int teamId)
        {
            return await _context.Favourites.FirstOrDefaultAsync(
                favourite => favourite.UserId == userId && favourite.TeamId == teamId);
        }

        public async Task AddAsync(Favourite favourite)
        {
            await _context.Favourites.AddAsync(favourite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Favourite favourite)
        {
            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();
        }
    }
}