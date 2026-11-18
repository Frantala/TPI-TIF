using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionController : ControllerBase
{
    private readonly ICompetitionExternalService _competitionExternalService;
    private readonly ICompetitionService _competitionService;
    
    public CompetitionController(
    ICompetitionExternalService competitionExternalService,
    ICompetitionService competitionService)
    {
        _competitionExternalService = competitionExternalService;
        _competitionService = competitionService;
    }


    [HttpGet("leagues")]
    public async Task<ActionResult<List<Competition>>> GetLeagues(
        [FromQuery] string country,
        [FromQuery] int season)
    {
        var leagues = await _competitionExternalService.GetLeaguesAsync(
            country,
            season
        );

        return Ok(leagues);
    }

    [HttpGet]
    public async Task<ActionResult<List<Competition>>> GetAll() =>
    Ok(await _competitionService.GetAllAsync());

    [HttpPost("sync")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<List<Competition>>> Sync(
        [FromQuery] string country, [FromQuery] int season)
    {
        try { return Ok(await _competitionService.SyncLeaguesAsync(country, season)); }
        catch (Exception ex) when (ex is InvalidOperationException or HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { message = ex.Message });
        }
    }
}