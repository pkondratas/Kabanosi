using Kabanosi.Dtos.Assignment;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/assignments")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> CreateAssignmentAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromBody] AssignmentRequestDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assignmentService.CreateAssignmentAsync(
            projectId,
            assignmentDto, 
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> GetAssignmentByIdAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id, cancellationToken);
        if (assignment == null)
            return NotFound();

        return Ok(assignment);
    }
    
    [HttpGet]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> GetAssignmentsByProjectIdAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentService.GetAssignmentsByProjectIdAsync(
            projectId,
            pageSize, 
            pageNumber, 
            cancellationToken);

        return Ok(assignments);
    }

    [HttpPatch("{id}/change-status")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> ChangeAssignmentStatusAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] Guid id,
        ChangeAssignmentStatusRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentService.ChangeAssignmentStatusAsync(id, request, cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id}/change-label")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> ChangeAssignmentLabelAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] Guid id,
        ChangeAssignmentLabelRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _assignmentService.ChangeAssignmentLabelAsync(id, request, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> DeleteAssignmentAsync(
    [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
    Guid _,
    [FromRoute] Guid id,
    CancellationToken cancellationToken = default)
    {
        await _assignmentService.DeleteAssignmentAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> UpdateAssignmentAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid _,
        [FromRoute] Guid id,
        [FromBody] AssignmentUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _assignmentService.UpdateAssignmentAsync(id, request, cancellationToken);
        return Ok(result);
    }
} 