using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionController : ControllerBase
{
    private readonly ICompetitionExternalService _competitionService;

    public CompetitionController(
        ICompetitionExternalService competitionService)
    {
        _competitionService = competitionService;
    }

    [HttpGet("leagues")]
    public async Task<ActionResult<List<Competition>>> GetLeagues(
        [FromQuery] string country,
        [FromQuery] int season)
    {
        var leagues = await _competitionService.GetLeaguesAsync(
            country,
            season
        );

        return Ok(leagues);
    }
}