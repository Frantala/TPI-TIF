using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/usuarios")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Usuario, Admin,SuperAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        [Authorize(Roles = "Usuario, Admin,SuperAdmin")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Usuario, Admin,SuperAdmin")]
        public async Task<ActionResult<UserDto>> Add(CreateUserDto userDto)
        {
            if (await _userService.EmailExistsAsync(userDto.Email))
            {
                return Conflict();
            }

            var user = await _userService.AddAsync(userDto);
            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Usuario, Admin,SuperAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _userService.GetByIdAsync(id) is null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPatch("{id:int}/ascender-a-admin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<UserDto>> MakeAdmin(int id)
        {
            var user = await _userService.PromoteToAdminAsync(id);

            if (user is null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            return Ok(user);
        }
        

    }
}

