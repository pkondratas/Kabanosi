using System.Security.Claims;
using Kabanosi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Authorization;

public class ProjectRoleHandler(DatabaseContext db)
    : AuthorizationHandler<ProjectRoleRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext ctx,
        ProjectRoleRequirement req)
    {
        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return;

        // project‑id must be present in the http header
        if (ctx.Resource is not HttpContext http) return;
        var header = http.Request.Headers["X-Project-Id"].FirstOrDefault();
        if (!Guid.TryParse(header, out var projectId)) return;

        var projectMember = await db.ProjectMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(pm =>
                pm.UserId == userId && pm.ProjectId == projectId);

        // success if a role is in an allowed list
        if (projectMember is not null &&
            req.AllowedRoles.Contains(projectMember.ProjectRole.ToString(),
                StringComparer.OrdinalIgnoreCase))
        {
            ctx.Succeed(req);
        }
    }
}