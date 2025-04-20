using Kabanosi.Dtos.Assignment;

namespace Kabanosi.Services.Interfaces;

public interface IAssignmentService
{
    Task<AssignmentResponseDto> CreateAssignmentAsync(AssignmentRequestDto assignmentDto, CancellationToken cancellationToken);
    Task<AssignmentResponseDto?> GetAssignmentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IList<AssignmentResponseDto>> GetAssignmentsByProjectIdAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
} 