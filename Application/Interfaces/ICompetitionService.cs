using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICompetitionService
    {
        Task<List<Competition>> GetAllAsync();
        Task<List<Competition>> SyncLeaguesAsync(string country, int season);
    }
}
