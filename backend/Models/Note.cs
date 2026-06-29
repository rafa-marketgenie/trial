using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using trial.Models;

namespace trial.Models
{
    public class Note
    {
        [Key]
        public int Id {get; set;}

        [StringLength(128)]
        public string Title {get; set;} = string.Empty;

        [StringLength(25565)]
        // TODO: encrypt Content (password? passkey? another field?)
        public string Content {get; set;} = string.Empty;

        [ForeignKey("NoteGroup")]
        public int GroupId {get; set;}

        public NoteGroup? NoteGroup {get; set;}

        [ForeignKey("CreatedBy")]
        public int CreatedByUserId {get; set;}

        public User? CreatedBy {get; set;}

        public ICollection<PermissionPolicy> PermissionPolicies {get; set;} = new List<PermissionPolicy>();
    }
}
