using System;

public class Match
{
	public int Id {get; set;}
	public string Score {get; set;} = string.Empty;
	public bool MatchState {get; set;}
	public string HomeLineUp {get; set;} = string.Empty;
	public string AwayLineUp {get; set;} = string.Empty;
}
