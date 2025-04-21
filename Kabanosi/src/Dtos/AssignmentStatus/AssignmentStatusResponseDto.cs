namespace Kabanosi.Dtos.AssignmentStatus;

public record AssignmentStatusResponseDto
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Order { get; init; }
}