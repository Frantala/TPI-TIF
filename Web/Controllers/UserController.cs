using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
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
        public async Task<ActionResult> Delete(int id)
        {
            if (await _userService.GetByIdAsync(id) is null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
