using trial.Contracts.Permissions;
using trial.Models;
using trial.Repositories;

namespace trial.Services
{
    public class PermissionPolicyService : IPermissionPolicyService
    {
        private readonly IPermissionPolicyRepository _permissionPolicyRepository;

        public PermissionPolicyService(IPermissionPolicyRepository permissionPolicyRepository)
        {
            _permissionPolicyRepository = permissionPolicyRepository;
        }

        public async Task<PermissionPolicyResponseDto?> GetPermissionPolicyAsync(int id, int actorUserId)
        {
            var policy = await _permissionPolicyRepository.GetByIdAsync(id);
            if(policy == null) return null;

            return new PermissionPolicyResponseDto
            {
                Id = policy.Id,
                OwnerId = policy.OwnerId,
                GuestId = policy.GuestId,
                NoteId = policy.NoteId,
                PermissionType = policy.PermissionType
            };
        }

        public async Task<PermissionPolicyResponseDto> GrantPermissionAsync(GrantPermissionRequestDto request, int actorUserId)
        {
            var newPolicy = new PermissionPolicy
            {
                OwnerId = actorUserId,
                GuestId = request.GuestId,
                NoteId = request.NoteId,
                PermissionType = request.PermissionType
            };

            await _permissionPolicyRepository.AddAsync(newPolicy);

            return new PermissionPolicyResponseDto
            {
                Id = newPolicy.Id,
                OwnerId = newPolicy.OwnerId,
                GuestId = newPolicy.GuestId,
                NoteId = newPolicy.NoteId,
                PermissionType = newPolicy.PermissionType
            };
        }

        public async Task<bool> UpdatePermissionPolicyAsync(int id, UpdatePermissionRequestDto request, int actorUserId)
        {
            var existingPolicy = await _permissionPolicyRepository.GetByIdAsync(id);
            if (existingPolicy == null) return false;

            existingPolicy.PermissionType = request.PermissionType;

            await _permissionPolicyRepository.UpdateAsync(existingPolicy);
            return true;
        }

        public async Task<bool> DeletePermissionPolicyAsync(int id, int actorUserId)
        {
            var existingPolicy = await _permissionPolicyRepository.GetByIdAsync(id);
            if (existingPolicy == null) return false;

            await _permissionPolicyRepository.DeleteAsync(existingPolicy);
            return true;
        }
    }
}