using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using trial.Models;

namespace trial.Models
{
    public class PermissionPolicy
    {
        [Key]
        public int Id {get; set;}

        // [ForeignKey(nameof(Owner))]
        // public Guid OwnerId {get; set;}

        // public User? Owner {get; set;}

        [ForeignKey(nameof(User))]
        public Guid UserId {get; set;}

        public User? User {get; set;}

        [ForeignKey(nameof(Note))]
        public Guid NoteId {get; set;}

        public Note? Note {get; set;}

        [Required]
        public PermissionType PermissionType {get; set;} = PermissionType.View;
    }
}