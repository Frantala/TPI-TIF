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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
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
            return await ChangeRoleAsync(
                id,
                "user",
                _userService.PromoteToAdminAsync,
                "Solo se puede ascender a Admin a usuarios con rol user.");
        }

        [HttpPatch("{id:int}/descender-a-user")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<UserDto>> DemoteAdmin(int id)
        {
            return await ChangeRoleAsync(
                id,
                "Admin",
                _userService.DemoteAdminToUserAsync,
                "Solo se puede descender a user a usuarios con rol Admin.");
        }

        [HttpPatch("{id:int}/ascender-a-superadmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<UserDto>> PromoteToSuperAdmin(int id)
        {
            return await ChangeRoleAsync(
                id,
                "Admin",
                _userService.PromoteAdminToSuperAdminAsync,
                "Solo se puede ascender a SuperAdmin a usuarios con rol Admin.");
        }

        private async Task<ActionResult<UserDto>> ChangeRoleAsync(
            int id,
            string expectedRole,
            Func<int, Task<UserDto?>> changeRole,
            string invalidRoleMessage)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Usuario no encontrado."
                });
            }

            if (!string.Equals(user.Role, expectedRole, StringComparison.Ordinal))
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Transición de rol no válida.",
                    Detail = invalidRoleMessage
                });
            }

            var updatedUser = await changeRole(id);
            if (updatedUser is null)
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "No se pudo actualizar el rol.",
                    Detail = "El usuario pudo haber cambiado de rol durante la operación."
                });
            }

            return Ok(updatedUser);
        }
    }
}

