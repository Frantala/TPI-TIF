using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserServices userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<UserDto> Register(CreateUserDto request)
        {
            if (_userService.EmailExists(request.Email))
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "El correo ya está registrado."
                });
            }

            var user = _userService.Add(request);
            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        [HttpPost("login")]
        [ProducesResponseType<JwtDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<JwtDto> Login(LoginDto request)
        {
            var user = _userService.Authenticate(request.Email, request.Password);
            if (user is null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Credenciales inválidas."
                });
            }

            return Ok(_jwtService.GenerateToken(user));
        }
    }
}
