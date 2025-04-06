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

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual Project Project { get; set; } = null!;
    }
}