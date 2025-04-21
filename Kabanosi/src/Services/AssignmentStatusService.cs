using System.Xml.Linq;
using AutoMapper;
using Kabanosi.Dtos.AssignmentStatus;
using Kabanosi.Entities;
using Kabanosi.Exceptions;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class AssignmentStatusService : IAssignmentStatusService
{
    private static readonly IList<string> DefaultAssignmentStatusNames = new List<string> { "To Do", "In Progress", "Done" };
    
    private readonly AssignmentStatusRepository _assignmentStatusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignmentStatusService(
        AssignmentStatusRepository assignmentStatusRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _assignmentStatusRepository = assignmentStatusRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<AssignmentStatusResponseDto> CreateAssignmentStatusAsync(
        Guid projectId, 
        AssignmentStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignmentStatuses = (await _assignmentStatusRepository.GetAllAsync(
            filter: a => a.ProjectId == projectId,
            asTracking: true,
            cancellationToken: cancellationToken)).ToList();
        
        var assignmentStatus = new AssignmentStatus
        {
            ProjectId = projectId,
            Name = request.Name,
            Order = assignmentStatuses.Count
        };
        
        await _assignmentStatusRepository.InsertAsync(assignmentStatus, cancellationToken);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentStatusResponseDto>(assignmentStatus);
    }

    public async Task<IEnumerable<AssignmentStatusResponseDto>> CreateDefaultAssignmentStatusesAsync(
        Guid projectId, 
        CancellationToken cancellationToken)
    {
        var assignmentStatuses = DefaultAssignmentStatusNames.Select((name, index) =>
            new AssignmentStatus
            {
                ProjectId = projectId,
                Name = name,
                Order = index
            }).ToList();

        await _assignmentStatusRepository.InsertRangeAsync(assignmentStatuses, cancellationToken);
        await _unitOfWork.SaveAsync();
        
        return assignmentStatuses.Select(_mapper.Map<AssignmentStatusResponseDto>);
    }

    public async Task<IEnumerable<AssignmentStatusResponseDto>> GetAssignmentStatusesAsync(
        Guid projectId, 
        CancellationToken cancellationToken)
    {
        var assignmentStatuses = await _assignmentStatusRepository.GetAllAsync(
            filter: a => a.ProjectId == projectId,
            cancellationToken: cancellationToken);
        
        return assignmentStatuses.Select(_mapper.Map<AssignmentStatusResponseDto>);
    }

    public async Task<IEnumerable<AssignmentStatusResponseDto>> ReorderAssignmentStatusesAsync(
        Guid projectId, 
        ReorderAssignmentStatusesRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignmentStatuses = (await _assignmentStatusRepository.GetAllAsync(
            filter: a => a.ProjectId == projectId,
            asTracking: true,
            cancellationToken: cancellationToken)).ToList();

        if (assignmentStatuses.Count != request.IdsInOrder.Count)
            throw new ConflictException("Assignment status counts do not match.");
        
        for (var i = 0; i < request.IdsInOrder.Count; i++)
        {
            var id = request.IdsInOrder[i];
            var assignmentStatus = assignmentStatuses.FirstOrDefault(a => a.Id == id);

            if (assignmentStatus == null)
                throw new NotFoundException($"Assignment status {id} not found.");

            assignmentStatus.Order = i;
        }

        await _unitOfWork.SaveAsync();
        
        return assignmentStatuses.Select(_mapper.Map<AssignmentStatusResponseDto>);
    }

    public async Task<AssignmentStatusResponseDto> RenameAssignmentStatusAsync(
        Guid id, 
        RenameAssignmentStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignmentStatus = await _assignmentStatusRepository
            .FirstOrDefaultTrackedAsync(a => a.Id == id, cancellationToken);

        if (assignmentStatus is null)
        {
            throw new NotFoundException($"Assignment status {id} not found.");
        }
        
        assignmentStatus.Name = request.NewName;
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentStatusResponseDto>(assignmentStatus);
    }

    public async Task DeleteAssignmentStatusAsync(Guid id, CancellationToken cancellationToken)
    {
        await _assignmentStatusRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveAsync();
    }
}