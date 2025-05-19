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

        public required ProjectRole ProjectRole { get; set; }

        public required string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public required Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [Timestamp] 
        public byte[] Version { get; set; } = [];
        
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<Assignment> ReportedAssignments { get; set; } = [];
        public ICollection<Assignment> AssignedAssignments { get; set; } = [];
    }
}