using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.AssignmentLabel;

public record AssignmentLabelRequestDto
{
    [Required]
    public required string Name { get; init; }
    
    public string? Description { get; init; }
}