using Microsoft.EntityFrameworkCore;
using trial.Data;
using trial.Models;

namespace trial.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<IEnumerable<Note>> FindAsync(System.Linq.Expressions.Expression<Func<Note, bool>> predicate)
        {
            return await _context.Notes.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Note entity)
        {
            await _context.Notes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note entity)
        {
            _context.Notes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Note entity)
        {
            _context.Notes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Notes.AnyAsync(u => u.Id == id);
        }

        public async Task<IReadOnlyCollection<Note>> GetNotesByCreatedByUserIdAsync(Guid userId)
        {
            return await _context.Notes
                .Where(n => n.CreatedByUserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Note>> GetNotesByNoteGroupIdAsync(int noteGroupId)
        {
            return await _context.Notes
                .Where(n => n.GroupId == noteGroupId)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Note>> SearchNotesByPhraseAsync(string phrase)
        {
            return await _context.Notes
                .Where(n => n.Title.Contains(phrase) || n.Content.Contains(phrase))
                .ToListAsync();
        }
    }
}