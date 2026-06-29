using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trial.Models
{
    public class NoteGroup
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name {get; set;} = string.Empty;

        public string Color {get; set;} = string.Empty;

        public ICollection<Note> GroupNotes {get; set;} = new List<Note>();
    }
}