using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();
        Task<List<Team>?> SyncTeamsByLeagueAsync(int leagueExternalId, int season);
    }
}
