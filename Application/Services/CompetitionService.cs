using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICompetitionExternalService _externalService;

        public CompetitionService(
            ICompetitionRepository competitionRepository,
            ICompetitionExternalService externalService)
        {
            _competitionRepository = competitionRepository;
            _externalService = externalService;
        }

        public Task<List<Competition>> GetAllAsync() => _competitionRepository.GetAllAsync();

        public async Task<List<Competition>> SyncLeaguesAsync(string country, int season)
        {
            var fromApi = (await _externalService.GetLeaguesAsync(country, season))
                .GroupBy(c => c.ExternalId).Select(g => g.First()).ToList();

            var existing = (await _competitionRepository
                    .GetByExternalIdsAsync(fromApi.Select(c => c.ExternalId)))
                .ToDictionary(c => c.ExternalId);

            var toAdd = new List<Competition>();
            var toUpdate = new List<Competition>();

            foreach (var item in fromApi)
            {
                if (existing.TryGetValue(item.ExternalId, out var competition))
                {
                    competition.Name = item.Name;
                    competition.Country = item.Country;
                    toUpdate.Add(competition);
                }
                else
                {
                    toAdd.Add(item);
                }
            }

            if (toUpdate.Count > 0) await _competitionRepository.UpdateRangeAsync(toUpdate);
            if (toAdd.Count > 0) await _competitionRepository.AddRangeAsync(toAdd);

            return toUpdate.Concat(toAdd).ToList();
        }
    }
}