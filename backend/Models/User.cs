using System.ComponentModel.DataAnnotations;
using trial.Models;

namespace trial.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public ICollection<PermissionPolicy> GivenPermissions {get; set;} = new List<PermissionPolicy>();
        public ICollection<PermissionPolicy> ReceivedPermissions {get; set;} = new List<PermissionPolicy>();
    }
}
