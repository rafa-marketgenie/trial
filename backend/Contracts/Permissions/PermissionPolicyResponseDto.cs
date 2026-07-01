using trial.Models;

namespace trial.Contracts.Permissions
{
    public class PermissionPolicyResponseDto
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public int GuestId { get; set; }

        public int NoteId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
