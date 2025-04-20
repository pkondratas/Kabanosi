using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Assignment;

public record AssignmentRequestDto
{
    [Required]
    public required string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    [Required]
    public required Guid ProjectId { get; init; }

    [Required]
    public required int AssignmentStatusId { get; init; }

    public int? AssignmentLabelId { get; init; }
    public bool IsPlanned { get; init; } = false;
    public int? Estimation { get; init; }
    public DateOnly? Deadline { get; init; }
} 