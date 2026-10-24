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
        public ActionResult<List<UserDto>> GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public ActionResult<UserDto> GetById(int id)
        {
            var user = _userService.GetById(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public ActionResult<UserDto> Add(CreateUserDto userDto)
        {
            if (_userService.EmailExists(userDto.Email))
            {
                return Conflict();
            }

            var user = _userService.Add(userDto);
            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (_userService.GetById(id) is null)
            {
                return NotFound();
            }

            _userService.Delete(id);
            return NoContent();
        }
    }
}
