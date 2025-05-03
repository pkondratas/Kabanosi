using Kabanosi.Dtos.Assignment;

namespace Kabanosi.Services.Interfaces;

public interface IAssignmentService
{
    Task<AssignmentResponseDto> CreateAssignmentAsync(
        Guid projectId,
        AssignmentRequestDto assignmentDto, 
        CancellationToken cancellationToken);
    Task<AssignmentResponseDto?> GetAssignmentByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);
    
    Task<IList<AssignmentResponseDto>> GetAssignmentsByProjectIdAsync(
        Guid projectId,
        int pageSize, 
        int pageNumber, 
        CancellationToken cancellationToken);
    
    Task<AssignmentResponseDto> ChangeAssignmentStatusAsync(
        Guid id, 
        ChangeAssignmentStatusRequestDto request, 
        CancellationToken cancellationToken);
    
    Task<AssignmentResponseDto> ChangeAssignmentLabelAsync(
        Guid id,
        ChangeAssignmentLabelRequestDto request,
        CancellationToken cancellationToken);

    Task DeleteAssignmentAsync(
        Guid id,
        CancellationToken cancellationToken);
        
    Task<AssignmentResponseDto> UpdateAssignmentAsync(
        Guid id,
        AssignmentUpdateRequestDto request,
        CancellationToken cancellationToken);
} 