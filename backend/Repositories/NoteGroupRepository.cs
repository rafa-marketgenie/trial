using Microsoft.EntityFrameworkCore;
using trial.Data;
using trial.Models;

namespace trial.Repositories
{
    public class NoteGroupRepository : INoteGroupRepository
    {
        private readonly AppDbContext _context;

        public NoteGroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NoteGroup?> GetByIdAsync(int id)
        {
            return await _context.NoteGroups.FindAsync(id);
        }

        public async Task<IEnumerable<NoteGroup>> GetAllAsync()
        {
            return await _context.NoteGroups.ToListAsync();
        }

        public async Task<IEnumerable<NoteGroup>> FindAsync(System.Linq.Expressions.Expression<Func<NoteGroup, bool>> predicate)
        {
            return await _context.NoteGroups.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(NoteGroup entity)
        {
            await _context.NoteGroups.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NoteGroup entity)
        {
            _context.NoteGroups.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(NoteGroup entity)
        {
            _context.NoteGroups.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.NoteGroups.AnyAsync(u => u.Id == id);
        }

        
    }
}