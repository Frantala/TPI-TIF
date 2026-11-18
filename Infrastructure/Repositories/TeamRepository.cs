using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<List<Team>> GetAllAsync() =>
            await Context.Teams.OrderBy(t => t.TeamName).ToListAsync();

        public async Task<List<Team>> GetByExternalIdsAsync(IEnumerable<int> externalIds)
        {
            var ids = externalIds.ToList();
            return await Context.Teams.Where(t => ids.Contains(t.ExternalId)).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Team> teams)
        {
            await Context.Teams.AddRangeAsync(teams);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Team> teams)
        {
            Context.Teams.UpdateRange(teams);
            await Context.SaveChangesAsync();
        }
    }
}