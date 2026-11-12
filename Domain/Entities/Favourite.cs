using System;
namespace Domain.Entities
{
public class Favourite
{
	public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
}
}