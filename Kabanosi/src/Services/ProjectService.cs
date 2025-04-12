using AutoMapper;
using Kabanosi.Dtos.Project;
using Kabanosi.Repositories;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class ProjectService(ProjectRepository projectRepository, IMapper mapper) : IProjectService
{
    public async Task<IList<ProjectResponseDto>> GetProjectsAsync(int pageSize = 0, int pageNumber = 10)
    {
        var projects = await projectRepository.GetAllAsync(pageSize, pageNumber);

        return mapper.Map<IList<ProjectResponseDto>>(projects);
    }
}