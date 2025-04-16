using Microsoft.AspNetCore.Authorization;

namespace Kabanosi.Authorization;

public class ProjectRoleRequirement(string[] allowedRoles) : IAuthorizationRequirement
{
    public IReadOnlyCollection<string> AllowedRoles { get; } = allowedRoles;
}