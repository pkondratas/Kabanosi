using Kabanosi.Dtos.AssignmentLabel;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/assignment-labels")]
public class AssignmentLabelController : ControllerBase
{
    private readonly IAssignmentLabelService _assignmentLabelService;

    public AssignmentLabelController(IAssignmentLabelService assignmentLabelService)
    {
        _assignmentLabelService = assignmentLabelService;
    }

    [HttpGet]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> GetAssignmentLabelsAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentLabelService.GetAssignmentLabelsAsync(
            projectId, 
            cancellationToken);
        
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> CreateAssignmentLabelAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        AssignmentLabelRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentLabelService.CreateAssignmentLabelAsync(
            projectId,
            request,
            cancellationToken);
        
        return Created($"api/v1/assignment-labels/{response.Id}", response);
    }

    [HttpPatch("{id}/rename")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> RenameAssignmentLabelAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] int id,
        AssignmentLabelRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentLabelService.RenameAssignmentLabelAsync(id, request, cancellationToken);
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> DeleteAssignmentLabelAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        await _assignmentLabelService.DeleteAssignmentLabelAsync(id, cancellationToken);

        return NoContent();
    }
}