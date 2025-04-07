using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kabanosi.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProjectMemberId { get; set; }
        public Guid AssignmentId { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(300)]
        public required string Content { get; set; }

        [Timestamp]
        public required byte[] Version { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;
        public virtual ProjectMember ProjectMember { get; set; } = null!;
    }
}