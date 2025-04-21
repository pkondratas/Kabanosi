using AutoMapper;
using Kabanosi.Dtos.AssignmentLabel;
using Kabanosi.Entities;
using Kabanosi.Exceptions;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;

namespace Kabanosi.Services;

public class AssignmentLabelService : IAssignmentLabelService
{
    private readonly AssignmentLabelRepository _assignmentLabelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignmentLabelService(
        AssignmentLabelRepository assignmentLabelRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _assignmentLabelRepository = assignmentLabelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<AssignmentLabelResponseDto> CreateAssignmentLabelAsync(
        Guid projectId, 
        AssignmentLabelRequestDto request, 
        CancellationToken cancellationToken)
    {
        var assignmentLabel = new AssignmentLabel
        {
            ProjectId = projectId,
            Name = request.Name,
            Description = request.Description
        };
        
        await _assignmentLabelRepository.InsertAsync(assignmentLabel, cancellationToken);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentLabelResponseDto>(assignmentLabel);
    }

    public async Task<IEnumerable<AssignmentLabelResponseDto>> GetAssignmentLabelsAsync(
        Guid projectId, 
        CancellationToken cancellationToken)
    {
        var assignmentLabels = await _assignmentLabelRepository.GetAllAsync(
            filter: l => l.ProjectId == projectId,
            cancellationToken: cancellationToken);
        
        return assignmentLabels.Select(_mapper.Map<AssignmentLabelResponseDto>);
    }

    public async Task<AssignmentLabelResponseDto> RenameAssignmentLabelAsync(
        int id, 
        AssignmentLabelRequestDto request, 
        CancellationToken cancellationToken)
    {
        var assignmentLabel = await _assignmentLabelRepository.FirstOrDefaultTrackedAsync(
            a => a.Id == id, cancellationToken);

        if (assignmentLabel is null)
        {
            throw new NotFoundException($"Assignment label {id} not found.");
        }
        
        assignmentLabel.Name = request.Name;
        assignmentLabel.Description = request.Description;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<AssignmentLabelResponseDto>(assignmentLabel);
    }

    public async Task DeleteAssignmentLabelAsync(int id, CancellationToken cancellationToken)
    {
        await _assignmentLabelRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveAsync();
    }
}