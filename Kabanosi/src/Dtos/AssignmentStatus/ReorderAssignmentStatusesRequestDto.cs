namespace Kabanosi.Dtos.AssignmentStatus;

public record ReorderAssignmentStatusesRequestDto
{
    public IList<Guid> IdsInOrder { get; init; }
}