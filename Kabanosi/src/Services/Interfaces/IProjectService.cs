using Kabanosi.Dtos.Project;
using Microsoft.AspNetCore.JsonPatch;

namespace Kabanosi.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto, CancellationToken cancellationToken);
    Task<IList<ProjectResponseDto>> GetProjectsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    Task<ProjectResponseDto> UpdateProjectAsync(Guid projectId, JsonPatchDocument<ProjectRequestDto> projectDoc, CancellationToken cancellationToken);
    Task DeleteProjectAsync(Guid projectId, CancellationToken cancellationToken);
}