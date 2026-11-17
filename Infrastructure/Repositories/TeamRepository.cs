using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(team => team.Id == id);
        }
        public async Task<List<Team>> GetAllAsync() =>
            await _context.Teams.OrderBy(t => t.TeamName).ToListAsync();

        public async Task<List<Team>> GetByExternalIdsAsync(IEnumerable<int> externalIds)
        {
            var ids = externalIds.ToList();
            return await _context.Teams.Where(t => ids.Contains(t.ExternalId)).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Team> teams)
        {
            await _context.Teams.AddRangeAsync(teams);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Team> teams)
        {
            _context.Teams.UpdateRange(teams);
            await _context.SaveChangesAsync();
        }
    }
}