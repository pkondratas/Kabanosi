using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kabanosi.Constants;

namespace Kabanosi.Entities
{
    [Table("ProjectMembers")]
    public class ProjectMember
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ProjectRole ProjectRole { get; set; }
        
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Timestamp]
        public required byte[] Version { get; set; }

        public ICollection<ProjectMemberAssignment> ProjectMemberAssignments { get; set; } = [];
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}