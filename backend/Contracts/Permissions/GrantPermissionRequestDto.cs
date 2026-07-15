using System.ComponentModel.DataAnnotations;
using trial.Models;

namespace trial.Contracts.Permissions
{
    public class GrantPermissionRequestDto
    {
        [Range(1, int.MaxValue)]
        public Guid UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int NoteId { get; set; }

        [Required]
        public PermissionType PermissionType { get; set; } = PermissionType.View;
    }
}
