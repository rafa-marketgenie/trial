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

        public async Task<PermissionPolicyResponseDto?> GetPermissionPolicyAsync(int id, Guid actorUserId)
        {
            var policy = await _permissionPolicyRepository.GetByIdAsync(id);
            if(policy == null) return null;
            // TODO implement verification

            return new PermissionPolicyResponseDto
            {
                Id = policy.Id,
                UserId = policy.UserId,
                NoteId = policy.NoteId,
                PermissionType = policy.PermissionType
            };
        }

        public async Task<PermissionPolicyResponseDto> GrantPermissionAsync(GrantPermissionRequestDto request, Guid actorUserId)
        {
            // TODO implement verification

            var newPolicy = new PermissionPolicy
            {
                UserId = request.UserId,
                NoteId = request.NoteId,
                PermissionType = request.PermissionType
            };

            await _permissionPolicyRepository.AddAsync(newPolicy);

            return new PermissionPolicyResponseDto
            {
                Id = newPolicy.Id,
                UserId = newPolicy.UserId,
                NoteId = newPolicy.NoteId,
                PermissionType = newPolicy.PermissionType
            };
        }

        public async Task<bool> UpdatePermissionPolicyAsync(int id, UpdatePermissionRequestDto request, Guid actorUserId)
        {
            var existingPolicy = await _permissionPolicyRepository.GetByIdAsync(id);
            if (existingPolicy == null) return false;
            // TODO implement verification

            existingPolicy.PermissionType = request.PermissionType;

            await _permissionPolicyRepository.UpdateAsync(existingPolicy);
            return true;
        }

        public async Task<bool> DeletePermissionPolicyAsync(int id, Guid actorUserId)
        {
            var existingPolicy = await _permissionPolicyRepository.GetByIdAsync(id);
            if (existingPolicy == null) return false;
            // TODO implement verification

            await _permissionPolicyRepository.DeleteAsync(existingPolicy);
            return true;
        }

        public async Task<IEnumerable<Guid>> GetAccessibleNoteIdsAsync(Guid userId)
        {
            var accessibleNoteIds = await _permissionPolicyRepository.GetAccessibleNotesByUserIdAsync(userId);

            return accessibleNoteIds;
        }
    }
}