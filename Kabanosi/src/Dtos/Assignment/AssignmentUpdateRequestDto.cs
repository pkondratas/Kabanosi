using System.ComponentModel.DataAnnotations;

namespace Kabanosi.Dtos.Assignment;

public record AssignmentUpdateRequestDto
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsPlanned { get; init; }
    public int? Estimation { get; init; }
    public DateOnly? Deadline { get; init; }
    public DateOnly? CompletedDate { get; set; }
    public int? AssignmentLabelId { get; init; }
    public Guid? AssigneeId { get; set; }
    public Guid AssignmentStatusId { get; init; }
} 