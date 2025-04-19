using Kabanosi.Constants;

namespace Kabanosi.Dtos.Invitation;

public record UserInvitesResponseDto
{
    public Guid InvitationId { get; init; }
    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = null!;
    public ProjectRole RoleOffered { get; init; }
    public DateTime ValidUntil { get; init; }
}