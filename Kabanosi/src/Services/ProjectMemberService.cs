using AutoMapper;
using Kabanosi.Dtos.ProjectMember;
using Kabanosi.Exceptions;
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
            filter: m => m.ProjectId == projectId,
            includes: m => m.User);
        
        return _mapper.Map<List<ProjectMemberResponseDto>>(members);
    }

    public async Task DeleteProjectMemberAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _projectMemberRepository.DeleteAsync(id, cancellationToken);

        await _unitOfWork.SaveAsync();
    }

    public async Task<ProjectMemberResponseDto> UpdateProjectMemberAsync(
        Guid id,
        ProjectMemberUpdateRequestDto request,
        CancellationToken cancellationToken)
    {
        var member = await _projectMemberRepository.GetByIdAsync(id, cancellationToken);

        if (member == null)
            throw new NotFoundException($"Project member {id} not found");

        member.ProjectRole = request.ProjectRole;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<ProjectMemberResponseDto>(member);
    }
}