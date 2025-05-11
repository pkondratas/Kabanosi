using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Entities
{
    [Table("ProjectMemberAssignments")]
    [PrimaryKey("ProjectMemberId", "AssignmentId", "IsReporter")]
    public class ProjectMemberAssignment
    {
        public Guid ProjectMemberId { get; set; }
        public Guid AssignmentId { get; set; }

        public bool IsReporter { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public ProjectMember ProjectMember { get; set; } = null!;
        public Assignment Assignment { get; set; } = null!;
    }
}