using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ApplicationDbContext _context;
        public CompetitionRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<Competition>> GetAllAsync() =>
            await _context.Competitions.OrderBy(c => c.Name).ToListAsync();

        public async Task<Competition?> GetByExternalIdAsync(int externalId) =>
            await _context.Competitions.FirstOrDefaultAsync(c => c.ExternalId == externalId);

        public async Task<List<Competition>> GetByExternalIdsAsync(IEnumerable<int> externalIds)
        {
            var ids = externalIds.ToList();
            return await _context.Competitions.Where(c => ids.Contains(c.ExternalId)).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Competition> competitions)
        {
            await _context.Competitions.AddRangeAsync(competitions);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Competition> competitions)
        {
            _context.Competitions.UpdateRange(competitions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetLinkedTeamIdsAsync(int competitionId, int season) =>
            await _context.CompetitionTeams
                .Where(ct => ct.CompetitionId == competitionId && ct.Season == season)
                .Select(ct => ct.TeamId)
                .ToListAsync();

        public async Task AddCompetitionTeamsAsync(IEnumerable<CompetitionTeam> links)
        {
            await _context.CompetitionTeams.AddRangeAsync(links);
            await _context.SaveChangesAsync();
        }
    }
}