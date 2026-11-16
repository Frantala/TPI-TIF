using Application.Dto;
using Application.Models;

namespace Application.Interfaces
{
    public interface IFavouriteService
    {
        Task<List<FavouriteDto>> GetByUserAsync(int userId);
        Task<bool> ExistsAsync(int userId, int teamId);
        Task<FavouriteDto?> AddAsync(int userId, int teamId);
        Task<bool> DeleteAsync(int userId, int teamId);
    }
}