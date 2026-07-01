namespace trial.Contracts.Notes
{
    public class NoteResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int GroupId { get; set; }

        public int CreatedByUserId { get; set; }
    }
}
