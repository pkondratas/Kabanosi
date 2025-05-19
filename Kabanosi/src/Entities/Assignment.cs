using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{   
    [Table("Assignments")]
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPlanned { get; set; }
        public int? Estimation { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? CompletedDate { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }
        
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        
        public Guid AssignmentStatusId { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; } = null!;
        
        public int? AssignmentLabelId { get; set; }
        public AssignmentLabel? AssignmentLabel { get; set; }
        
        public Guid ReporterId { get; set; }
        public ProjectMember Reporter { get; set; } = null!;
        
        public Guid? AssigneeId { get; set; }
        public ProjectMember? Assignee { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = [];
    }
}