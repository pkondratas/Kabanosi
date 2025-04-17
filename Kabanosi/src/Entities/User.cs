using Microsoft.AspNetCore.Identity;

namespace Kabanosi.Entities;

public class User : IdentityUser
{
    public ICollection<Invitation> Invitations { get; set; } = [];
    public ICollection<ProjectMember> ProjectMembers { get; set; } = [];
}