using Kabanosi.Dtos.Invitation;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }
    
    [HttpPost("invitations")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> CreateInvitationAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")]
        [FromHeader(Name = "X-Project-Id")] Guid projectId,
        [FromBody] CreateInvitationDto invitationDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _invitationService.CreateInvitationAsync(
            projectId,
            invitationDto,
            cancellationToken);

        return Ok(result);
    }
}