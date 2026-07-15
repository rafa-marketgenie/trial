using trial.Models;

namespace trial.Repositories
{
    public interface INoteRepository : IRepository<Note>
    {
        public Task<IReadOnlyCollection<Note>> GetNotesByCreatedByUserIdAsync(Guid userId);

        public Task<IReadOnlyCollection<Note>> GetNotesByNoteGroupIdAsync(int noteGroupId);

        public Task<IReadOnlyCollection<Note>> SearchNotesByPhraseAsync(string phrase);
    }
}