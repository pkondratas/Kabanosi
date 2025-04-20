using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/auth-test")]
public class AuthTestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<string>> GetAuthMessage()
    {
        return Ok("You are authenticated");
    }

    [HttpGet("project-admin-test")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public IActionResult TestAdminAccess(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _)
    {
        return Ok(new
        {
            Message = "You are a Project Admin."
        });
    }

    [HttpGet("project-admin-or-member-test")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public IActionResult TestAdminOrMemberAccess(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _)
    {
        return Ok(new
        {
            Message = "You are either a Project Admin or Member."
        });
    }
}