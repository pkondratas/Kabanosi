using System.ComponentModel.DataAnnotations;
using Kabanosi.Constants;

namespace Kabanosi.Dtos.Invitation;

public record CreateInvitationDto
{
    [Required, EmailAddress] 
    public required string TargetEmail { get; init; } = null!;

    [Required]
    [Range((int)ProjectRole.ProjectAdmin, (int)ProjectRole.ProjectMember)]
    public ProjectRole? TargetRole { get; init; }

    public int? ValidDays { get; init; } = 7;
}