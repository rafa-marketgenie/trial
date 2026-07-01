using trial.Contracts.NoteGroups;

namespace trial.Services
{
    public interface INoteGroupService
    {
        Task<IReadOnlyCollection<NoteGroupResponseDto>> GetAllNoteGroupsAsync();

        Task<NoteGroupResponseDto?> GetNoteGroupByIdAsync(int id);

        Task<NoteGroupResponseDto> CreateNoteGroupAsync(CreateNoteGroupRequestDto request);

        Task<bool> UpdateNoteGroupAsync(int id, UpdateNoteGroupRequestDto request);

        Task<bool> DeleteNoteGroupAsync(int id);
    }
}