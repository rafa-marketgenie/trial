using trial.Contracts.Notes;
using trial.Models;
using trial.Repositories;

namespace trial.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly INoteGroupRepository _noteGroupRepository;
        private readonly IPermissionPolicyRepository _permissionPolicyRepository;

        public NoteService(INoteRepository noteRepository, INoteGroupRepository noteGroupRepository, IPermissionPolicyRepository permissionPolicyRepository)
        {
            _noteRepository = noteRepository;
            _noteGroupRepository = noteGroupRepository;
            _permissionPolicyRepository = permissionPolicyRepository;
        }

        public async Task<IReadOnlyCollection<NoteResponseDto>> GetAllNotesAsync()
        {
            var notes = await _noteRepository.GetAllAsync();
            return notes.Select(n => new NoteResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task<NoteResponseDto?> GetNoteByIdAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return null;

            return new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                GroupId = note.GroupId,
                CreatedByUserId = note.CreatedByUserId
            };
        }

        public async Task<IReadOnlyCollection<NoteResponseDto>> SearchNotesAsync(string? phrase)
        {
            var notes = await _noteRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(phrase))
            {
                notes = notes
                    .Where(n => n.Content != null &&
                                n.Content.Contains(phrase, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return notes.Select(n => new NoteResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt,
                GroupId = n.GroupId,
                CreatedByUserId = n.CreatedByUserId
            }).ToList();
        }

        public async Task<NoteResponseDto> CreateNoteAsync(CreateNoteRequestDto request, int actorUserId)
        {
            NoteGroup? noteGroup = null;

            if (request.GroupId.HasValue)
            {
                noteGroup = await _noteGroupRepository.GetByIdAsync(request.GroupId.Value);
                if (noteGroup == null)
                {
                    throw new ArgumentException($"Note group with ID {request.GroupId} does not exist.");
                }
            }

            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                GroupId = request.GroupId,
                CreatedByUserId = actorUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _noteRepository.AddAsync(note);

            return new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                GroupId = note.GroupId,
                CreatedByUserId = note.CreatedByUserId
            };
        }

        public async Task<bool> UpdateNoteAsync(int id, UpdateNoteRequestDto request, int actorUserId)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return false;

            // TODO implement permissions
            // Check if the actor has permission to update the note
            // var hasPermission = await _permissionPolicyRepository.HasPermissionAsync(actorUserId, note.Id, "update");
            // if (!hasPermission) return false;

            note.Title = request.Title ?? note.Title;
            note.Content = request.Content ?? note.Content;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        public async Task<bool> DeleteNoteAsync(int id, int actorUserId)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return false;

            // TODO implement permissions
            // Check if the actor has permission to delete the note
            // var hasPermission = await _permissionPolicyRepository.HasPermissionAsync(actorUserId, note.Id, "delete");
            // if (!hasPermission) return false;

            await _noteRepository.DeleteAsync(note);
            return true;
        }
    }
}