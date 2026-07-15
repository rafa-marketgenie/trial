using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using trial.utils;
using trial.Models;
using trial.Services;
using trial.Contracts.Notes;
using Microsoft.AspNetCore.Authorization;
using trial.Utils;

namespace trial.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NoteResponseDto>>> GetAll()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseDto>> GetById(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IReadOnlyCollection<NoteResponseDto>>> GetByUserId(Guid userId)
        {
            var notes = await _noteService.GetNotesByCreatedByUserIdAsync(userId);
            return Ok(notes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IReadOnlyCollection<NoteResponseDto>>> Search([FromQuery] string query)
        {
            var notes = await _noteService.SearchNotesAsync(query);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create([FromBody] CreateNoteRequestDto request)
        {
            var actorUserId = User.GetUserId();
            var note = await _noteService.CreateNoteAsync(request, actorUserId);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteRequestDto request)
        {
            var actorUserId = User.GetUserId();
            var success = await _noteService.UpdateNoteAsync(id, request, actorUserId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actorUserId = User.GetUserId();
            var success = await _noteService.DeleteNoteAsync(id, actorUserId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
