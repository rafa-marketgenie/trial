using System.ComponentModel.DataAnnotations;

namespace trial.Contracts.NoteGroups
{
    public class CreateNoteGroupRequestDto
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [StringLength(32)]
        public string Color { get; set; } = string.Empty;
    }
}
