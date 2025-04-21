namespace Kabanosi.Dtos.AssignmentLabel;

public record AssignmentLabelResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public Guid ProjectId { get; init; }
}