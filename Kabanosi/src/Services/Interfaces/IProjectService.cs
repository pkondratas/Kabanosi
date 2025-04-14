using Kabanosi.Dtos.Project;

namespace Kabanosi.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateProjectAsync(ProjectRequestDto projectDto, CancellationToken cancellationToken);
    Task<IList<ProjectResponseDto>> GetProjectsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
}