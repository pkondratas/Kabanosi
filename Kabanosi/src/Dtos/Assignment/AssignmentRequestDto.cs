using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Assignment;

public record AssignmentRequestDto
{
    [Required]
    public required string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? AssignmentStatusId { get; init; }
    public int? AssignmentLabelId { get; init; }
    public bool IsPlanned { get; init; } = false;
    public int? Estimation { get; init; }
    public DateOnly? Deadline { get; init; }
} 