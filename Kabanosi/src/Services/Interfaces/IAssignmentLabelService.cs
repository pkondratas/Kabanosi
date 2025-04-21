using Kabanosi.Dtos.AssignmentLabel;

namespace Kabanosi.Services.Interfaces;

public interface IAssignmentLabelService
{
    Task<AssignmentLabelResponseDto> CreateAssignmentLabelAsync(
        Guid projectId,
        AssignmentLabelRequestDto request,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<AssignmentLabelResponseDto>> GetAssignmentLabelsAsync(
        Guid projectId,
        CancellationToken cancellationToken);
    
    Task<AssignmentLabelResponseDto> RenameAssignmentLabelAsync(
        int id,
        AssignmentLabelRequestDto request,
        CancellationToken cancellationToken);
    
    Task DeleteAssignmentLabelAsync(
        int id, 
        CancellationToken cancellationToken);
}