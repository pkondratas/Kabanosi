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

        public ICollection<Assignment> Assignments { get; set; } = [];
        public ICollection<AssignmentStatus> AssignmentStatuses { get; set; } = [];
        public ICollection<AssignmentLabel> AssignmentLabels { get; set; } = [];
        public ICollection<ProjectMember> ProjectMembers { get; set; } = [];
        public ICollection<Invitation> Invitations { get; set; } = [];
    }
}