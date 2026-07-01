using System.ComponentModel.DataAnnotations;
using trial.Models;

namespace trial.Contracts.Permissions
{
    public class UpdatePermissionRequestDto
    {
        [Required]
        public PermissionType PermissionType { get; set; } = PermissionType.View;
    }
}
