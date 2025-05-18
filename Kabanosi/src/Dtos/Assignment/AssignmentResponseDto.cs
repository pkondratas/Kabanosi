namespace Kabanosi.Dtos.Assignment;

public record AssignmentResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid ProjectId { get; init; }
    public int? AssignmentLabelId { get; init; }
    public Guid AssignmentStatusId { get; init; }
    public bool IsPlanned { get; init; }
    public int? Estimation { get; init; }
    public DateOnly? Deadline { get; init; }
    public DateOnly? CompletedDate { get; init; }

    public string? AssignmentLabelName { get; init; }
    public string? AssigneeUsername { get; init; }
    public string? ReportedUsername { get; init; }
} 