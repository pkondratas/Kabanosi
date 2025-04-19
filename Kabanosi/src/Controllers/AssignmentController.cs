using Kabanosi.Dtos.Assignment;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Policy = "ProjectAdminOrMember")]
    public async Task<IActionResult> CreateAssignmentAsync(
        [FromBody] AssignmentRequestDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assignmentService.CreateAssignmentAsync(assignmentDto, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ProjectAdminOrMember")]
    public async Task<IActionResult> GetAssignmentByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id, cancellationToken);
        if (assignment == null)
            return NotFound();

        return Ok(assignment);
    }
    
    [HttpGet]
    [Authorize(Policy = "ProjectAdminOrMember")]
    public async Task<IActionResult> GetAssignmentsByProjectIdAsync(
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentService.GetAssignmentsByProjectIdAsync(pageSize, pageNumber, cancellationToken);

        return Ok(assignments);
    }
} 