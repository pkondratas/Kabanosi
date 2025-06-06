using Kabanosi.Dtos.ProjectMember;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/project-members")]
public class ProjectMemberController : ControllerBase
{
    private readonly IProjectMemberService _projectMemberService;

    public ProjectMemberController(IProjectMemberService projectMemberService)
    {
        _projectMemberService = projectMemberService;
    }

    [HttpGet]
    [Authorize(Policy = "ProjectMemberAndAdminOrMember")]
    public async Task<IActionResult> GetProjectMembersByProjectIdAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var response = await _projectMemberService.GetProjectMembersAsync(
        projectId,
        pageSize,
        pageNumber,
        cancellationToken);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> DeleteProjectMemberAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _projectMemberService.DeleteProjectMemberAsync(id, cancellationToken);
        
        return Ok();
    }
    
    [HttpPatch("{id}")]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> UpdateProjectMemberAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")] [FromHeader(Name = "X-Project-Id")]
        Guid projectId,
        [FromRoute] Guid id,
        [FromBody] ProjectMemberUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _projectMemberService.UpdateProjectMemberAsync(id, request, cancellationToken);
        return Ok(result);
    }
}