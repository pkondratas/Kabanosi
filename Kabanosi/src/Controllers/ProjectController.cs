using Kabanosi.Dtos.Project;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await projectService.CreateProjectAsync(projectDto, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProjectsAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        // TODO: filter projects that user is a member of
        return Ok(await projectService.GetProjectsAsync(pageSize, pageNumber, cancellationToken));
    }
}