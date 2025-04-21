namespace Kabanosi.Dtos.Assignment;

public record ChangeAssignmentStatusRequestDto
{
    public Guid NewAssignmentStatusId { get; init; }
}