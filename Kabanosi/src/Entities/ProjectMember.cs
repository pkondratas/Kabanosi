using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("ProjectMembers")]
    public class ProjectMember
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual ICollection<ProjectMemberAssignment> ProjectMemberAssignments { get; set; } = [];
        public virtual ICollection<Project> Projects { get; set; } = [];
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}