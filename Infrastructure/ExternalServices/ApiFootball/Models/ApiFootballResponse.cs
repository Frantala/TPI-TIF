using System.Text.Json;

namespace Infrastructure.ExternalServices.ApiFootball.Models;

public class ApiFootballResponse<T>
{
    // respuesta general que devuelve la API
    public string Get { get; set; } = string.Empty;

    public int Results { get; set; }

    public JsonElement Errors { get; set; }

    public List<T> Response { get; set; } = new();
}