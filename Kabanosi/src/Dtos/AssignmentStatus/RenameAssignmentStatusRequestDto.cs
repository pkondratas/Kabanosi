using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.AssignmentStatus;

public record RenameAssignmentStatusRequestDto
{
    [Required]
    public required string NewName { get; set; }
}