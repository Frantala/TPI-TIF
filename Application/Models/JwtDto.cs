namespace Application.Dto
{
    public sealed class JwtDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAtUtc { get; set; }
    }
}