using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{   
    [Table("Assignments")]
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public int AssignmentLabelId { get; set; }
        public int AssignmentStatusId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsPlanned { get; set; }
        public int Estimation { get; set; }
        public DateOnly Deadline { get; set; }
        public DateOnly CompletedDate { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual AssignmentStatus AssignmentStatus { get; set; } = null!;
        public virtual AssignmentLabel? AssignmentLabel { get; set; }
        public virtual ICollection<ProjectMemberAssignment> ProjectParticipantAssignments { get; set; } = [];
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}