namespace Application.Interfaces
{
    public interface IApiFootballService
    {
        public interface IApiFootballService
        {
            Task<string> ObtenerEquiposAsync(
                int liga,
                int temporada,
                CancellationToken cancellationToken = default);
        }
    }
}

