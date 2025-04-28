using Kabanosi.Dtos.Project;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kabanosi.Controllers;

[ApiController]
[Route("api/v1/projects")]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProjectAsync(
        [FromBody] ProjectRequestDto projectDto,
        CancellationToken cancellationToken = default)
    {
        return ModelState.IsValid ? Ok(await projectService.CreateProjectAsync(projectDto, cancellationToken)) : BadRequest(ModelState);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProjectsAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        return Ok(await projectService.GetProjectsAsync(pageSize, pageNumber, cancellationToken));
    }

    [HttpPatch()]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> UpdateProjectAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")]
        [FromHeader(Name = "X-Project-Id")] Guid projectId,
        [FromBody] JsonPatchDocument<ProjectRequestDto> projectDoc,
        CancellationToken cancellationToken = default)
    {
        return ModelState.IsValid ? Ok(await projectService.UpdateProjectAsync(projectId, projectDoc, cancellationToken)) : BadRequest(ModelState);
    }

    [HttpDelete]
    [Authorize(Policy = "ProjectMemberAndAdmin")]
    public async Task<IActionResult> DeleteProjectAsync(
        [SwaggerParameter("Project ID used for project-scoped authorization")]
        [FromHeader(Name = "X-Project-Id")] Guid projectId,
        CancellationToken cancellationToken = default)
    {
        await projectService.DeleteProjectAsync(projectId, cancellationToken);

        return Ok();
    }
}