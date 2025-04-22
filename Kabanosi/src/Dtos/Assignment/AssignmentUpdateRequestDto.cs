using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Assignment;

public record AssignmentUpdateRequestDto
{
    public string? Name { get; init; }

    public string? Description { get; init; }
    
    public int? Estimation { get; init; }
    
    public DateOnly? Deadline { get; init; }

    public bool? IsPlanned { get; init; }
} 