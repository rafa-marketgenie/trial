using System.ComponentModel.DataAnnotations;

namespace trial.Contracts.Notes
{
    public class CreateNoteRequestDto
    {
        // [Required]
        [StringLength(128)]
        public string Title { get; set; } = string.Empty;

        // [Required]
        [StringLength(25565)]
        public string Content { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int? GroupId { get; set; }
    }
}
