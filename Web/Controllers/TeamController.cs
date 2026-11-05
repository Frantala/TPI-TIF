using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamExternalService _teamService;

    public TeamController (ITeamExternalService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("teams")]
    public async Task<ActionResult<List<Team>>> GetTeams(
        [FromQuery] int leagueId,
        [FromQuery] int season)
    {
        var teams = await _teamService.GetTeamsByLeagueAsync(
            leagueId,
            season
        );

        return Ok(teams);
    }
}