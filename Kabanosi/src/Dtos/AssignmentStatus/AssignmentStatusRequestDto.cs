using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.AssignmentStatus;

public record AssignmentStatusRequestDto
{
    [Required]
    public required string Name { get; init; }
};