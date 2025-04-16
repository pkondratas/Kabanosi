using System.Security.Claims;
using AutoMapper;
using Kabanosi.Constants;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class ProjectService(
    ProjectRepository projectRepository,
    ProjectMemberRepository projectMemberRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
) : IProjectService
{
    private readonly string _userId =
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<ProjectResponseDto> CreateProjectAsync(
        ProjectRequestDto projectDto,
        CancellationToken cancellationToken)
    {
        var project = mapper.Map<Project>(projectDto);
        project = await projectRepository.InsertAsync(project, cancellationToken);

        var adminMember = new ProjectMember
        {
            UserId = _userId,
            ProjectId = project.Id,
            ProjectRole = ProjectRole.ProjectAdmin
        };
        await projectMemberRepository.InsertAsync(adminMember, cancellationToken);

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