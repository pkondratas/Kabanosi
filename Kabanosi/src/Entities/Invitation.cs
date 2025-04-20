using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kabanosi.Constants;

namespace Kabanosi.Entities
{
    [Table("Invitations")]
    public class Invitation
    {
        [Key]
        public Guid Id { get; set; }
        public required DateTime ValidUntil { get; set; }
        public required InvitationStatus InvitationStatus { get; set; } = InvitationStatus.Pending;
        public required ProjectRole ProjectRole { get; set; }
        
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Timestamp]
        public byte[] Version { get; set; }
    }
}