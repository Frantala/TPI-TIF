using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ITeamExternalService _externalService;

        public TeamService(
            ITeamRepository teamRepository,
            ICompetitionRepository competitionRepository,
            ITeamExternalService externalService)
        {
            _teamRepository = teamRepository;
            _competitionRepository = competitionRepository;
            _externalService = externalService;
        }

        public Task<List<Team>> GetAllAsync() => _teamRepository.GetAllAsync();

        public async Task<List<Team>?> SyncTeamsByLeagueAsync(int leagueExternalId, int season)
        {
            var competition = await _competitionRepository.GetByExternalIdAsync(leagueExternalId);
            if (competition is null) return null;

            var fromApi = (await _externalService.GetTeamsByLeagueAsync(leagueExternalId, season))
                .GroupBy(t => t.ExternalId).Select(g => g.First()).ToList();

            var existing = (await _teamRepository
                    .GetByExternalIdsAsync(fromApi.Select(t => t.ExternalId)))
                .ToDictionary(t => t.ExternalId);

            var toAdd = new List<Team>();
            var toUpdate = new List<Team>();

            foreach (var item in fromApi)
            {
                if (existing.TryGetValue(item.ExternalId, out var team))
                {
                    team.TeamName = item.TeamName;
                    team.Country = item.Country;
                    team.SoccerTeam = item.SoccerTeam;
                    toUpdate.Add(team);
                }
                else
                {
                    toAdd.Add(item);
                }
            }

            if (toUpdate.Count > 0) await _teamRepository.UpdateRangeAsync(toUpdate);
            if (toAdd.Count > 0) await _teamRepository.AddRangeAsync(toAdd); // acá EF asigna los Id

            var allTeams = toUpdate.Concat(toAdd).ToList();

            var alreadyLinked = (await _competitionRepository
                .GetLinkedTeamIdsAsync(competition.Id, season)).ToHashSet();

            var newLinks = allTeams
                .Where(t => !alreadyLinked.Contains(t.Id))
                .Select(t => new CompetitionTeam
                {
                    CompetitionId = competition.Id,
                    TeamId = t.Id,
                    Season = season
                }).ToList();

            if (newLinks.Count > 0) await _competitionRepository.AddCompetitionTeamsAsync(newLinks);

            competition.TeamsCount = alreadyLinked.Count + newLinks.Count;
            await _competitionRepository.UpdateRangeAsync(new[] { competition });

            return allTeams;
        }
    }
}