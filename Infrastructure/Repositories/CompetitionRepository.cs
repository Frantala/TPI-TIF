using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompetitionRepository : GenericRepository<Competition>, ICompetitionRepository
    {
        public CompetitionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<List<Competition>> GetAllAsync() =>
            await Context.Competitions.OrderBy(c => c.Name).ToListAsync();

        public async Task<Competition?> GetByExternalIdAsync(int externalId) =>
            await Context.Competitions.FirstOrDefaultAsync(c => c.ExternalId == externalId);

        public async Task<List<Competition>> GetByExternalIdsAsync(IEnumerable<int> externalIds)
        {
            var ids = externalIds.ToList();
            return await Context.Competitions.Where(c => ids.Contains(c.ExternalId)).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Competition> competitions)
        {
            await Context.Competitions.AddRangeAsync(competitions);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Competition> competitions)
        {
            Context.Competitions.UpdateRange(competitions);
            await Context.SaveChangesAsync();
        }

        public async Task<List<int>> GetLinkedTeamIdsAsync(int competitionId, int season) =>
            await Context.CompetitionTeams
                .Where(ct => ct.CompetitionId == competitionId && ct.Season == season)
                .Select(ct => ct.TeamId)
                .ToListAsync();

        public async Task AddCompetitionTeamsAsync(IEnumerable<CompetitionTeam> links)
        {
            await Context.CompetitionTeams.AddRangeAsync(links);
            await Context.SaveChangesAsync();
        }
    }
}