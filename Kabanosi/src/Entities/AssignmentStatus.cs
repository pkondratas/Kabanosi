using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("AssignmentStatuses")]
    public class AssignmentStatus
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public required string Name { get; set; }
        public required int Order { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public Project Project { get; set; } = null!;
        public ICollection<Assignment> Assignments { get; set; } = [];
    }
}