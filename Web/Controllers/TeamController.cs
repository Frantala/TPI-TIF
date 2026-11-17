using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamExternalService _teamExternalService;
    private readonly ITeamService _teamService;

    public TeamController(ITeamExternalService teamExternalService, ITeamService teamService)
    {
        _teamExternalService = teamExternalService;
        _teamService = teamService;
    }

    [HttpGet("teams")]
    public async Task<ActionResult<List<Team>>> GetTeams(
        [FromQuery] int leagueId,
        [FromQuery] int season)
    {
        var teams = await _teamExternalService.GetTeamsByLeagueAsync(
            leagueId,
            season
        );

        return Ok(teams);
    }

    [HttpGet]
    public async Task<ActionResult<List<Team>>> GetAll() =>
    Ok(await _teamService.GetAllAsync());

    [HttpPost("sync")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<List<Team>>> Sync(
        [FromQuery] int leagueId, [FromQuery] int season)
    {
        try
        {
            var teams = await _teamService.SyncTeamsByLeagueAsync(leagueId, season);
            return teams is null
                ? NotFound(new { message = "La liga no está en la base. Sincronizá las competiciones primero." })
                : Ok(teams);
        }
        catch (Exception ex) when (ex is InvalidOperationException or HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { message = ex.Message });
        }
    }
}