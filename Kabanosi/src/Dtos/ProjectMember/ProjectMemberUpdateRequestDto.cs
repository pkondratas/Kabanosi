using System.ComponentModel.DataAnnotations;
using Kabanosi.Constants;

namespace Kabanosi.Dtos.ProjectMember;

public class ProjectMemberUpdateRequestDto
{
    [Required]
    public ProjectRole ProjectRole { get; init; }
}