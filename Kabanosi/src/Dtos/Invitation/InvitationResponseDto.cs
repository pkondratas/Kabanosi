using Kabanosi.Constants;

namespace Kabanosi.Dtos.Invitation;

public record InvitationResponseDto
{
    public Guid InvitationId { get; init; }
    public Guid ProjectId { get; init; }
    public string TargetEmail { get; init; } = null!;
    public ProjectRole Role { get; init; }
    public InvitationStatus Status { get; init; }
    public DateTime ValidUntil { get; init; }
}