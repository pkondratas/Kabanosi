using AutoMapper;
using Kabanosi.Dtos.Assignment;
using Kabanosi.Entities;
using Kabanosi.Exceptions;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentStatusService _assignmentStatusService;
    private readonly AssignmentRepository _assignmentRepository;
    private readonly ProjectMemberRepository _projectMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignmentService(
        IAssignmentStatusService assignmentStatusService,
        AssignmentRepository assignmentRepository,
        ProjectMemberRepository projectMemberRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _assignmentStatusService = assignmentStatusService;
        _assignmentRepository = assignmentRepository;
        _projectMemberRepository = projectMemberRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AssignmentResponseDto> CreateAssignmentAsync(
        string reporterUserId,
        Guid projectId,
        AssignmentRequestDto assignmentDto,
        CancellationToken cancellationToken)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);
        assignment.ProjectId = projectId;
        
        if (assignmentDto.AssignmentStatusId is null)
        {
            var initialAssignmentStatus = await _assignmentStatusService.GetInitialAssignmentStatus(projectId, cancellationToken);
            assignment.AssignmentStatusId = initialAssignmentStatus.Id;
        }
        
        var reporterProjectMember = await _projectMemberRepository.FirstOrDefaultAsync(
            pm => pm.ProjectId == projectId && pm.UserId == reporterUserId,
            cancellationToken);

        if (reporterProjectMember is null)
            throw new NotFoundException("Project member is not found");
        
        assignment.ReporterId = reporterProjectMember.Id;
        
        assignment = await _assignmentRepository.InsertAsync(assignment, cancellationToken);
        
        await _unitOfWork.SaveAsync();

        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<AssignmentResponseDto?> GetAssignmentByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id, cancellationToken);
        
        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");
        
        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<IList<AssignmentResponseDto>> GetAssignmentsByProjectIdAsync(
        Guid projectId,
        int pageSize, 
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var assignments = await _assignmentRepository.GetProjectAssignmentsAsync(projectId, cancellationToken);

        return _mapper.Map<IList<AssignmentResponseDto>>(assignments);
    }

    public async Task<IList<AssignmentResponseDto>> GetPlannedAssignmentsByProjectIdAsync(
        Guid projectId,
        int pageSize, 
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var assignments = await _assignmentRepository.GetProjectPlannedAssignmentsAsync(projectId, cancellationToken);

        return _mapper.Map<IList<AssignmentResponseDto>>(assignments);
    }

    public async Task<AssignmentResponseDto> ChangeAssignmentStatusAsync(
        Guid id, 
        ChangeAssignmentStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id, cancellationToken);

        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");
        
        assignment.AssignmentStatusId = request.NewAssignmentStatusId;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<AssignmentResponseDto> ChangeAssignmentLabelAsync(
        Guid id, 
        ChangeAssignmentLabelRequestDto request, 
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id, cancellationToken);
        
        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");
        
        assignment.AssignmentLabelId = request.NewAssignmentLabelId;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<AssignmentResponseDto> UpdateAssignmentAsync(
        Guid id,
        AssignmentUpdateRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id, cancellationToken);

        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");

        if (request.Name is not null)
            assignment.Name = request.Name;

        assignment.AssignmentStatusId = request.AssignmentStatusId;
        assignment.Description = request.Description;
        assignment.Estimation = request.Estimation;
        assignment.IsPlanned = request.IsPlanned;
        assignment.Deadline = request.Deadline;
        assignment.CompletedDate = request.CompletedDate;
        assignment.AssignmentLabelId = request.AssignmentLabelId;
        assignment.AssigneeId = request.AssigneeId;

        await _unitOfWork.SaveAsync();

        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id, cancellationToken);
    
        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");
    
        _assignmentRepository.Delete(assignment);
    
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<AssignmentResponseDto> AssignAssignmentAsync(
        Guid id, 
        Guid projectId,
        AssignRequest request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id, cancellationToken);
        
        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");

        assignment.AssigneeId = request.AssigneeId;

        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentResponseDto>(assignment);
    }
}