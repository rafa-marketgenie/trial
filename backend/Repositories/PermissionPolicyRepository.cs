using Microsoft.EntityFrameworkCore;
using trial.Data;
using trial.Models;

namespace trial.Repositories
{
    public class PermissionPolicyRepository : IPermissionPolicyRepository
    {
        private readonly AppDbContext _context;

        public PermissionPolicyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PermissionPolicy?> GetByIdAsync(int id)
        {
            return await _context.PermissionPolicies.FindAsync(id);
        }

        public async Task<IEnumerable<PermissionPolicy>> GetAllAsync()
        {
            return await _context.PermissionPolicies.ToListAsync();
        }

        public async Task<IEnumerable<PermissionPolicy>> FindAsync(System.Linq.Expressions.Expression<Func<PermissionPolicy, bool>> predicate)
        {
            return await _context.PermissionPolicies.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(PermissionPolicy entity)
        {
            await _context.PermissionPolicies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PermissionPolicy entity)
        {
            _context.PermissionPolicies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PermissionPolicy entity)
        {
            _context.PermissionPolicies.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PermissionPolicies.AnyAsync(u => u.Id == id);
        }

        
    }
}