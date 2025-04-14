using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("Invitations")]
    public class Invitation
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public required DateTime ValidUntil { get; set; }
        public required bool IsAccepted { get; set; }
        
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Timestamp]
        public required byte[] Version { get; set; }

        public Project Project { get; set; } = null!;
    }
}