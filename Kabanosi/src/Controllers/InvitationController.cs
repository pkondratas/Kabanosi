using Kabanosi.Dtos.Invitation;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/invitations")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpGet("user-invites")]
    [Authorize]
    public async Task<IActionResult> GetUserInvitesAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var result = await _invitationService.GetUserInvitesAsync(pageSize, pageNumber, cancellationToken);
        return Ok(result);
    }

    [HttpGet("project-invites")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> GetProjectInvitesAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var result = await _invitationService.GetProjectInvitesAsync(projectId, pageSize, pageNumber, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> CreateInviteAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromBody] CreateInvitationDto invitationDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _invitationService.CreateInviteAsync(
            projectId,
            invitationDto,
            cancellationToken);

        return Ok(result);
    }

    // Accept invitation (User)

    // Decline invitation (User)

    // Cancel invitation (Project Admin)

    // Expired invitation = hosted service (like a cron job)
}