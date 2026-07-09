using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using trial.utils;
using trial.Models;
using trial.Services;
using trial.Contracts.NoteGroups;
using Microsoft.AspNetCore.Authorization;

namespace trial.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NoteGroupController : ControllerBase
    {
        private readonly INoteGroupService _noteGroupService;

        public NoteGroupController(INoteGroupService noteGroupService)
        {
            _noteGroupService = noteGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NoteGroupResponseDto>>> GetAll()
        {
            var noteGroups = await _noteGroupService.GetAllNoteGroupsAsync();
            return Ok(noteGroups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteGroupResponseDto>> GetById(int id)
        {
            var noteGroup = await _noteGroupService.GetNoteGroupByIdAsync(id);
            if (noteGroup == null) return NotFound();
            return Ok(noteGroup);
        }

        [HttpPost]
        public async Task<ActionResult<NoteGroupResponseDto>> Create([FromBody] CreateNoteGroupRequestDto request)
        {
            var noteGroup = await _noteGroupService.CreateNoteGroupAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = noteGroup.Id }, noteGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteGroupRequestDto request)
        {
            var success = await _noteGroupService.UpdateNoteGroupAsync(id, request);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return NoContent();
            var success = await _noteGroupService.DeleteNoteGroupAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}