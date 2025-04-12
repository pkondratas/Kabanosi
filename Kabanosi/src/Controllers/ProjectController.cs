using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kabanosi.Controllers;

[ApiController]
[Route("v1/projects")]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjectsAsync(
        [FromQuery] int pageSize, 
        [FromQuery] int pageNumber)
    {
        return Ok(await projectService.GetProjectsAsync(pageSize, pageNumber));
    }
}