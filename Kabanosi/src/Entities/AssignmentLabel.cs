using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("AssignmentLabels")]
    public class AssignmentLabel
    {
        [Key]
        public int Id { get; set; }

        public Guid ProjectId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public Project Project { get; set; } = null!;
        public ICollection<Assignment> Assignments { get; set; } = [];
    }
}