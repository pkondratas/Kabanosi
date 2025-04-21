namespace Kabanosi.Dtos.AssignmentStatus;

public class AssignmentStatusResponseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}