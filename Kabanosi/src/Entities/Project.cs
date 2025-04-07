using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("Projects")]
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; } = [];
        public virtual ICollection<AssignmentStatus> AssignmentStatuses { get; set; } = [];
        public virtual ICollection<AssignmentLabel> AssignmentLabels { get; set; } = [];
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = [];
        public virtual ICollection<Invitation> Invitations { get; set; } = [];
    }
}