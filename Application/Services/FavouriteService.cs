using Application.Dto;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly ITeamRepository _teamRepository;

        public FavouriteService(
            IFavouriteRepository favouriteRepository,
            ITeamRepository teamRepository)
        {
            _favouriteRepository = favouriteRepository;
            _teamRepository = teamRepository;
        }

        public async Task<List<FavouriteDto>> GetByUserAsync(int userId)
        {
            var favourites = await _favouriteRepository.GetByUserIdAsync(userId);

            return favourites.Select(favourite => new FavouriteDto
            {
                Id = favourite.Id,
                TeamId = favourite.TeamId,
                TeamName = favourite.Team.TeamName,
                Country = favourite.Team.Country
            }).ToList();
        }

        public async Task<bool> ExistsAsync(int userId, int teamId)
        {
            return await _favouriteRepository.GetByUserAndTeamAsync(userId, teamId) is not null;
        }

        public async Task<FavouriteDto?> AddAsync(int userId, int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);

            if (team is null)
            {
                return null;
            }

            var favourite = new Favourite
            {
                UserId = userId,
                TeamId = teamId
            };

            await _favouriteRepository.AddAsync(favourite);

            return new FavouriteDto
            {
                Id = favourite.Id,
                TeamId = team.Id,
                TeamName = team.TeamName,
                Country = team.Country
            };
        }

        public async Task<bool> DeleteAsync(int userId, int teamId)
        {
            var favourite = await _favouriteRepository.GetByUserAndTeamAsync(userId, teamId);

            if (favourite is null)
            {
                return false;
            }

            await _favouriteRepository.DeleteAsync(favourite);
            return true;
        }
    }
}