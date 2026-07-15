using trial.Contracts.Notes;

namespace trial.Services
{
    public interface INoteService
    {
        Task<IReadOnlyCollection<NoteResponseDto>> GetAllNotesAsync();

        Task<NoteResponseDto?> GetNoteByIdAsync(Guid id);

        Task<IReadOnlyCollection<NoteResponseDto>> SearchNotesAsync(string? phrase);

        Task<NoteResponseDto> CreateNoteAsync(CreateNoteRequestDto request, Guid actorUserId);

        Task<bool> UpdateNoteAsync(Guid id, UpdateNoteRequestDto request, Guid actorUserId);

        Task<bool> DeleteNoteAsync(Guid id, Guid actorUserId);

        Task<IReadOnlyCollection<NoteResponseDto>> GetNotesByCreatedByUserIdAsync(Guid userId);
    }
}