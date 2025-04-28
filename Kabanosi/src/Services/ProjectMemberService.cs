using AutoMapper;
using Kabanosi.Dtos.ProjectMember;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class ProjectMemberService : IProjectMemberService
{
    private readonly ProjectMemberRepository _projectMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectMemberService(
        ProjectMemberRepository projectMemberRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _projectMemberRepository = projectMemberRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<ProjectMemberResponseDto>> GetProjectMembersAsync(
        Guid projectId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var members = await _projectMemberRepository.GetAllAsync(
            pageSize,
            pageNumber,
            cancellationToken,
            filter: m => m.ProjectId == projectId);
        
        return _mapper.Map<List<ProjectMemberResponseDto>>(members);
    }

    public async Task DeleteProjectMemberAsync(
        Guid projectMemberId,
        CancellationToken cancellationToken)
    {
        await _projectMemberRepository.DeleteAsync(projectMemberId, cancellationToken);

        await _unitOfWork.SaveAsync();
    }
}