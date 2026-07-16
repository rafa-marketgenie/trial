using trial.Models;

namespace trial.Repositories
{
    public interface IPermissionPolicyRepository : IRepository<PermissionPolicy>
    {
        public Task<IEnumerable<Guid>> GetAccessibleNotesByUserIdAsync(Guid userId);
    }
}