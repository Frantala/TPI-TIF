using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        JwtDto GenerateToken(User user);
    }
}
