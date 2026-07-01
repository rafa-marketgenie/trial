using System.ComponentModel.DataAnnotations;

namespace trial.Contracts.Users
{
    public class UpdateUserRequestDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(128, MinimumLength = 8)]
        public string? Password { get; set; }
    }
}
