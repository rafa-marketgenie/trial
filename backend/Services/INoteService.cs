using trial.Contracts.Notes;

namespace trial.Services
{
    public interface INoteService
    {
        Task<IReadOnlyCollection<NoteResponseDto>> GetAllNotesAsync();

        Task<NoteResponseDto?> GetNoteByIdAsync(int id);

        Task<IReadOnlyCollection<NoteResponseDto>> SearchNotesAsync(string? phrase);

        Task<NoteResponseDto> CreateNoteAsync(CreateNoteRequestDto request, int actorUserId);

        Task<bool> UpdateNoteAsync(int id, UpdateNoteRequestDto request, int actorUserId);

        Task<bool> DeleteNoteAsync(int id, int actorUserId);

        Task<IReadOnlyCollection<NoteResponseDto>> GetNotesByCreatedByUserIdAsync(int userId);
    }
}