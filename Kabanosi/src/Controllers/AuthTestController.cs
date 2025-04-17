using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Policy = "ProjectAdminOnly")]
    public IActionResult TestAdminAccess()
    {
        return Ok(new
        {
            Message = "You are a Project Admin."
        });
    }
    
    [HttpGet("project-admin-or-member-test")]
    [Authorize(Policy = "ProjectAdminOrMember")]
    public IActionResult TestAdminOrMemberAccess()
    {
        return Ok(new
        {
            Message = "You are either a Project Admin or Member."
        });
    }
}