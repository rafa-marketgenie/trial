using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using trial.utils;

namespace trial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        // In-memory notes list for demo purposes
        private static readonly List<Note> Notes = new List<Note>();
        private static readonly Saver saver = new Saver("notesData.json");

        // Static constructor to load notes only once
        static NotesController() {
            string persistedData = saver.Load();
            if (persistedData != ""){
                var loadedNotes = JsonSerializer.Deserialize<List<Note>>(persistedData);
                if (loadedNotes != null) {
                    Notes.AddRange(loadedNotes);
                }
            }
        }

        public NotesController() { }

        [HttpGet]
        public ActionResult<IEnumerable<Note>> GetAll(string sortBy = "id", string sortOrder = "asc")
        {
            IEnumerable<Note> sortedNotes = Notes;

            switch (sortBy.ToLower()) {
                case "title":
                    sortedNotes = (sortOrder == "desc") ? Notes.OrderByDescending(n => n.Title) : Notes.OrderBy(n => n.Title);
                    break;
                case "content":
                    sortedNotes = (sortOrder == "desc") ? Notes.OrderByDescending(n => n.Content) : Notes.OrderBy(n => n.Content);
                    break;
                default:
                    sortedNotes = (sortOrder == "desc") ? Notes.OrderByDescending(n => n.Id) : Notes.OrderBy(n => n.Id);
                    break;
            }

            return Ok(sortedNotes);
        }

        [HttpGet("{id}")]
        public ActionResult<Note> GetById(int id)
        {
            var note = Notes.Find(n => n.Id == id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpPost]
        public ActionResult<Note> Create(Note note)
        {
            // Console.WriteLine("Post");
            note.Id = Notes.Count + 1;
            Notes.Add(note); 

            SaveData();

            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Note updatedNote)
        {
            var note = Notes.Find(n => n.Id == id);
            if (note == null) return NotFound();
            note.Title = updatedNote.Title;
            note.Content = updatedNote.Content;

            SaveData();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var note = Notes.Find(n => n.Id == id);
            if (note == null) return NotFound();
            Notes.Remove(note);

            SaveData();

            return NoContent();
        }



        private void SaveData()
        {
            string jsonData = JsonSerializer.Serialize(Notes);
            saver.Save(jsonData);
        }
    }

    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
