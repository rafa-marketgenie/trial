using System.ComponentModel.DataAnnotations;

namespace trial.Contracts.Notes
{
    public class UpdateNoteRequestDto
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(25565, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int? GroupId { get; set; }
    }
}
