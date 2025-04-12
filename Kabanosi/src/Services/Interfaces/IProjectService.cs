using Kabanosi.Dtos.Project;

namespace Kabanosi.Services.Interfaces;

public interface IProjectService
{
    Task<IList<ProjectResponseDto>> GetProjectsAsync(int pageSize, int pageNumber);
}