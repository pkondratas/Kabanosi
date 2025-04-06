using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("AssignmentStatuses")]
    public class AssignmentStatus
    {
        [Key]
        public int Id { get; set; }

        public Guid ProjectId { get; set; }
        public required string Name { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<Assignment> Assignments { get; set; } = [];
    }
}