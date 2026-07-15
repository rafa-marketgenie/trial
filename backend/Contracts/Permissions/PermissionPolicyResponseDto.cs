using trial.Models;

namespace trial.Contracts.Permissions
{
    public class PermissionPolicyResponseDto
    {
        public int Id { get; set; }

        // public Guid OwnerId { get; set; }

        public Guid UserId { get; set; }

        public int NoteId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
