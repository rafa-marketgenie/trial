using trial.Contracts.Permissions;

namespace trial.Services
{
    public interface IPermissionPolicyService
    {
        Task<PermissionPolicyResponseDto?> GetPermissionPolicyAsync(int id, Guid actorUserId);

        Task<PermissionPolicyResponseDto> GrantPermissionAsync(GrantPermissionRequestDto request, Guid actorUserId);

        Task<bool> UpdatePermissionPolicyAsync(int id, UpdatePermissionRequestDto request, Guid actorUserId);

        Task<bool> DeletePermissionPolicyAsync(int id, Guid actorUserId);

        Task<IEnumerable<Guid>> GetAccessibleNoteIdsAsync(Guid userId);
    }
}