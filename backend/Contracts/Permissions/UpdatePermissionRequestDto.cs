using System.ComponentModel.DataAnnotations;
using trial.Models;

namespace trial.Contracts.Permissions
{
    public class UpdatePermissionRequestDto
    {
        // public int Id { get; set; }

        [Required]
        public PermissionType PermissionType { get; set; } = PermissionType.View;
    }
}
