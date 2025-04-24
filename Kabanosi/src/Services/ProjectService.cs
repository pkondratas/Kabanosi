using System.Security.Claims;
using AutoMapper;
using Kabanosi.Constants;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;
using Kabanosi.Specifications;
using Microsoft.AspNetCore.JsonPatch;

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

        await unitOfWork.CreateTransactionAsync();

        var project = mapper.Map<Project>(projectDto);
        project = await projectRepository.InsertAsync(project, cancellationToken);

        var adminMember = new ProjectMember
        {
            UserId = userId,
            ProjectId = project.Id,
            ProjectRole = ProjectRole.ProjectAdmin
        };

        await Task.WhenAll(
            projectMemberRepository.InsertAsync(adminMember, cancellationToken),
            assignmentStatusService.CreateDefaultAssignmentStatusesAsync(project.Id, cancellationToken)
        );
        
        await unitOfWork.SaveAsync();
        await unitOfWork.CommitAsync();

        return mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<IList<ProjectResponseDto>> GetProjectsAsync(
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var projects = await projectRepository.GetAllAsync(
            pageSize,
            pageNumber,
            cancellationToken,
            filter: ProjectSpecifications.MemberBy(userId)
        );

        return mapper.Map<IList<ProjectResponseDto>>(projects);
    }

    public async Task<ProjectResponseDto> UpdateProjectAsync(
        Guid projectId,
        JsonPatchDocument<ProjectRequestDto> projectDoc,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(projectId, cancellationToken);
        var mappedProjectDto = mapper.Map<ProjectRequestDto>(project);

        projectDoc.ApplyTo(mappedProjectDto);
        mapper.Map(mappedProjectDto, project);

        await unitOfWork.SaveAsync();
    
        return mapper.Map<ProjectResponseDto>(project);
    }

    public async Task DeleteProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        await projectRepository.DeleteAsync(projectId, cancellationToken);

        await unitOfWork.SaveAsync();
    }
}