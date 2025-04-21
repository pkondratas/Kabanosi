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
    private readonly AssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignmentService(
        AssignmentRepository assignmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AssignmentResponseDto> CreateAssignmentAsync(
        Guid projectId,
        AssignmentRequestDto assignmentDto,
        CancellationToken cancellationToken)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);
        assignment.ProjectId = projectId;
        
        assignment = await _assignmentRepository.InsertAsync(assignment, cancellationToken);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<AssignmentResponseDto?> GetAssignmentByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id, cancellationToken);
        return assignment == null ? null : _mapper.Map<AssignmentResponseDto>(assignment);
    }

    public async Task<IList<AssignmentResponseDto>> GetAssignmentsByProjectIdAsync(
        Guid projectId,
        int pageSize, 
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var assignments = await _assignmentRepository.GetAllAsync(
            pageSize,
            pageNumber,
            cancellationToken,
            filter: a => a.ProjectId == projectId);

        return _mapper.Map<IList<AssignmentResponseDto>>(assignments);
    }

    public async Task<AssignmentResponseDto> ChangeAssignmentStatusAsync(
        Guid id, 
        ChangeAssignmentStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id, cancellationToken);

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
        var assignment = await _assignmentRepository.GetByIdAsync(id, cancellationToken);

        if (assignment == null)
            throw new NotFoundException($"Assignment {id} not found.");
        
        assignment.AssignmentLabelId = request.NewAssignmentLabelId;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentResponseDto>(assignment);
    }
} 