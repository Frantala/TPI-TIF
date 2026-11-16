using Application.Dto;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/favoritos")]
    public sealed class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FavouriteDto>>> GetMine()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            return Ok(await _favouriteService.GetByUserAsync(userId));
        }

        [HttpPost("{teamId:int}")]
        public async Task<ActionResult<FavouriteDto>> Add(int teamId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (await _favouriteService.ExistsAsync(userId, teamId))
            {
                return Conflict();
            }

            var favourite = await _favouriteService.AddAsync(userId, teamId);

            if (favourite is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetMine), favourite);
        }

        [HttpDelete("{teamId:int}")]
        public async Task<ActionResult> Delete(int teamId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            if (!await _favouriteService.DeleteAsync(userId, teamId))
            {
                return NotFound();
            }

            return NoContent();
        }
        private bool TryGetUserId(out int userId)
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("nameid")
                        ?? User.FindFirstValue("sub");

            return int.TryParse(value, out userId);
        }
    }
}