using Kabanosi.Dtos.AssignmentStatus;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/assignment-statuses")]
public class AssignmentStatusController : ControllerBase
{
    private readonly IAssignmentStatusService _assignmentStatusService;

    public AssignmentStatusController(IAssignmentStatusService assignmentStatusService)
    {
        _assignmentStatusService = assignmentStatusService;
    }
    
    [HttpGet]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> GetAssignmentStatusesAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentStatusService.GetAssignmentStatusesAsync(
            projectId, 
            cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> CreateAssignmentStatusAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        AssignmentStatusRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _assignmentStatusService.CreateAssignmentStatusAsync(
            projectId,
            request,
            cancellationToken);
        
        return Created($"api/v1/assignment-statuses/{response.Id}", response);
    }

    [HttpPatch("{id}/rename")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> RenameAssignmentStatusAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] Guid id,
        RenameAssignmentStatusRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _assignmentStatusService.RenameAssignmentStatusAsync(
            id,
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("reorder")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> ReorderAssignmentStatusesAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        ReorderAssignmentStatusesRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentStatusService.ReorderAssignmentStatusesAsync(
            projectId,
            request,
            cancellationToken);
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> DeleteAssignmentStatusAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _assignmentStatusService.DeleteAssignmentStatusAsync(id, cancellationToken);

        return NoContent();
    }
}