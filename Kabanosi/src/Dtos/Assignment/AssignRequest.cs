namespace Kabanosi.Dtos.Assignment;

public record AssignRequest
{
    public Guid? AssigneeId { get; init; }
}