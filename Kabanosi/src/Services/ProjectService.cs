using AutoMapper;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class ProjectService(
    ProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IProjectService
{
    public async Task<ProjectResponseDto> CreateProjectAsync(
        ProjectRequestDto projectDto,
        CancellationToken cancellationToken)
    {
        var project = mapper.Map<Project>(projectDto);

        project = await projectRepository.InsertAsync(project, cancellationToken);
        await unitOfWork.SaveAsync();

        return mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<IList<ProjectResponseDto>> GetProjectsAsync(
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var projects = await projectRepository.GetAllAsync(pageSize, pageNumber, cancellationToken);

        return mapper.Map<IList<ProjectResponseDto>>(projects);
    }
}