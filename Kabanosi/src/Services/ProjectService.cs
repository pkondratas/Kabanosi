using System.Security.Claims;
using AutoMapper;
using Kabanosi.Constants;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;
using Kabanosi.Specifications;

namespace Kabanosi.Services;

public class ProjectService(
    IAssignmentStatusService assignmentStatusService,
    ProjectRepository projectRepository,
    ProjectMemberRepository projectMemberRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
) : IProjectService
{
    public async Task<ProjectResponseDto> CreateProjectAsync(
        ProjectRequestDto projectDto,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var project = mapper.Map<Project>(projectDto);
        project = await projectRepository.InsertAsync(project, cancellationToken);

        var adminMember = new ProjectMember
        {
            UserId = userId,
            ProjectId = project.Id,
            ProjectRole = ProjectRole.ProjectAdmin
        };
        await projectMemberRepository.InsertAsync(adminMember, cancellationToken);
        await assignmentStatusService.CreateDefaultAssignmentStatusesAsync(project.Id, cancellationToken);

        await unitOfWork.SaveAsync();

        return mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<IList<ProjectResponseDto>> GetProjectsAsync(
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var projects = await projectRepository.GetAllAsync(pageSize, pageNumber, cancellationToken,
            ProjectSpecifications.MemberBy(userId));

        return mapper.Map<IList<ProjectResponseDto>>(projects);
    }
}