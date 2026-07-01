using trial.Contracts.Permissions;

namespace trial.Services
{
    public interface IPermissionPolicyService
    {
        Task<PermissionPolicyResponseDto?> GetPermissionPolicyAsync(int id, int actorUserId);

        Task<PermissionPolicyResponseDto> GrantPermissionAsync(GrantPermissionRequestDto request, int actorUserId);

        Task<bool> UpdatePermissionPolicyAsync(int id, UpdatePermissionRequestDto request, int actorUserId);

        Task<bool> DeletePermissionPolicyAsync(int id, int actorUserId);
    }
}