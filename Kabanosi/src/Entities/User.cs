using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Kabanosi.Entities;

[Table("User")]
public class User : IdentityUser
{
    public ICollection<Invitation> Invitations { get; set; } = [];
    public ICollection<ProjectMember> ProjectMembers { get; set; } = [];
}