using trial.Contracts.NoteGroups;
using trial.Models;
using trial.Repositories;

namespace trial.Services
{
    public class NoteGroupService : INoteGroupService
    {
        private readonly INoteGroupRepository _noteGroupRepository;

        public NoteGroupService(INoteGroupRepository noteGroupRepository)
        {
            _noteGroupRepository = noteGroupRepository;
        }

        public async Task<IReadOnlyCollection<NoteGroupResponseDto>> GetAllNoteGroupsAsync()
        {
            var noteGroups = await _noteGroupRepository.GetAllAsync();
            return noteGroups.Select(ng => new NoteGroupResponseDto
            {
                Id = ng.Id,
                Name = ng.Name,
                Color = ng.Color
            }).ToList();
        }

        public async Task<NoteGroupResponseDto?> GetNoteGroupByIdAsync(int id)
        {
            var noteGroup = await _noteGroupRepository.GetByIdAsync(id);
            if (noteGroup == null) return null;

            return new NoteGroupResponseDto
            {
                Id = noteGroup.Id,
                Name = noteGroup.Name,
                Color = noteGroup.Color
            };
        }

        public async Task<NoteGroupResponseDto> CreateNoteGroupAsync(CreateNoteGroupRequestDto request)
        {
            var newNoteGroup = new NoteGroup
            {
                Name = request.Name,
                Color = request.Color
            };

            await _noteGroupRepository.AddAsync(newNoteGroup);

            return new NoteGroupResponseDto
            {
                Id = newNoteGroup.Id,
                Name = newNoteGroup.Name,
                Color = newNoteGroup.Color
            };
        }

        public async Task<bool> UpdateNoteGroupAsync(int id, UpdateNoteGroupRequestDto request)
        {
            var existingNoteGroup = await _noteGroupRepository.GetByIdAsync(id);
            if (existingNoteGroup == null) return false;

            existingNoteGroup.Name = request.Name ?? existingNoteGroup.Name;
            existingNoteGroup.Color = request.Color ?? existingNoteGroup.Color;

            await _noteGroupRepository.UpdateAsync(existingNoteGroup);
            return true;
        }

        public async Task<bool> DeleteNoteGroupAsync(int id)
        {
            var existingNoteGroup = await _noteGroupRepository.GetByIdAsync(id);
            if (existingNoteGroup == null) return false;

            await _noteGroupRepository.DeleteAsync(existingNoteGroup);
            return true;
        }
    }
}