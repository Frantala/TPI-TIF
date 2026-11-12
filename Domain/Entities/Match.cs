using System;
namespace Domain.Entities
{
public class Match
{
	public int Id { get; set; }

    // Identificador del partido en API-Football
    public int ExternalId { get; set; }

    // Competición
    public int CompetitionId { get; set; }
    public Competition Competition { get; set; } = null!;

    // Equipo local
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;

    // Equipo visitante
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;

    // Información del partido
    public DateTime Date { get; set; }
    public string Score { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public string HomeLineUp { get; set; } = string.Empty;
    public string AwayLineUp { get; set; } = string.Empty;
}
}

// comentar 