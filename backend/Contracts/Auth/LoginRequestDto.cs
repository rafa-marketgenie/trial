using System.ComponentModel.DataAnnotations;

namespace trial.Contracts.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Password { get; set; } = string.Empty;
    }
}