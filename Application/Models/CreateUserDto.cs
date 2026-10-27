using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public sealed class CreateUserDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
