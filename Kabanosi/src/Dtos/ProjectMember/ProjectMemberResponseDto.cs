using Kabanosi.Constants;

namespace Kabanosi.Dtos.ProjectMember;

public record ProjectMemberResponseDto()
{
    public Guid Id { get; init; }
    public ProjectRole ProjectRole { get; init; }
    public string UserId { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}